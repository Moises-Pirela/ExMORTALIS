using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AudioSource))]
public class GunControlMecanim : MonoBehaviour	// to be attached to the gun prefab when using Mechanim animation mode, if Legacy, use GunControl.cs instead
{
	#region Fields

	public RuntimeAnimatorController properArmsAnimation;

	bool canFire = true;
	public enum weaponType { Shotgun, Machinegun, Sniper, Pistol };
	public weaponType typeOfGun;

	public AudioClip TakeSound;
	public AudioClip FireSound;

	public GameObject bullet = null;  
	public Transform ejectPoint = null;  
	public Rigidbody shell = null;      
	 
	bool firePressed = false;  
	public bool FirePressed { get { return firePressed; } }

	bool isAimed;
	public bool IsAimed { get { return isAimed; } } 
		
	bool canAim;
	public bool useSmooth = true;
	[Range (1f, 10f)]
	public float aimSmooth;
	[Range(0, 179)]
	public float aimFov = 35;
	float defaultFov;
	float currentFov;
	Quaternion camPosition;
	Quaternion defaultCamRot;

	public int pelletsPerShot = 10; 

	// basic stats
	public int range = 300; 
	public float damage = 20.0f;
	public float maxPenetration = 3.0f;
	public float fireRate = 0.5f; 
	public int impactForce = 50;   
	public float bulletSpeed = 200.0f;  

	[HideInInspector]
	public float baseSpread = 1.0f;  
	public float maxSpread = 4.0f; 
	public float spreadPerSecond = 0.2f; 
	public float spread = 0.0f;   
	public float decreaseSpreadPerSec = 0.5f;
	float defaultSpread;
	float defaultMaxSpread;

	float nextFireTime = 0.0f; 

	AudioSource Source;

	Animator[] myAnims;

	#endregion

	void Awake()
	{
		Source = GetComponent<AudioSource>();
		defaultSpread = baseSpread;
		defaultMaxSpread = maxSpread;
		myAnims = GetComponentsInChildren <Animator> ();
	}

	void Start()
	{
		defaultCamRot = Camera.main.transform.localRotation;
		defaultFov = Camera.main.fieldOfView;
		canAim = true;
//		playerSync.UpdateCurrentGun (GetComponent <GunControlMecanim> ());
	}

	void Update()
	{
		InputUpdate();
		Aim();

		if (firePressed)
			spread += spreadPerSecond;
		else
			spread -= decreaseSpreadPerSec; 

		UpdateAnimatorParameters ();
	}

	void LateUpdate()
	{
		if (spread >= maxSpread)
		{
			spread = maxSpread; 
		}
		else
		{
			if (spread <= baseSpread)
				spread = baseSpread; 
		}
	}

	void InputUpdate()
	{
		if (Input.GetMouseButton (0) && CanFire)
			firePressed = true;
		else
			firePressed = false;
		
		switch (typeOfGun) {
			case weaponType.Shotgun:
				if (Input.GetMouseButtonDown(0) && CanFire)
				{
					ShotGunFire(); 
				}
				break;
			case weaponType.Machinegun:
				if (Input.GetMouseButton (0) && CanFire) 
				{
					GunFire ();        	
				} 
				break;
			case weaponType.Sniper:
			case weaponType.Pistol:
				if (Input.GetMouseButtonDown (0) && CanFire) 
				{
					ShotGunFire (); 
				} 
				break;
			default :
				break;
		}

		if (Input.GetButton ("Fire2") && CanAim)
			isAimed = true;
		else
			isAimed = false;
	}
		
	void UpdateAnimatorParameters ()
	{
		foreach (Animator a in myAnims)
		{
			a.SetBool ("firePressed", firePressed);
		}

		if (Input.GetKeyDown (KeyCode.R) && playerSync.Anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
		{
			foreach (Animator a in myAnims)
			{
				a.SetTrigger ("reload");
			}
		}
	}
		
	void GunFire()
	{
		if (Time.time - fireRate > nextFireTime)
			nextFireTime = Time.time - Time.deltaTime;

		while (nextFireTime < Time.time)
		{
			SetFireTrigger ();
		
			StartCoroutine(FireOneShot());
			Source.clip = FireSound;
			Source.spread = Random.Range (1.0f, 1.5f);
			Source.pitch = Random.Range  (1.0f, 1.05f);
			Source.Play();
			nextFireTime += fireRate;
			EjectShell();
		}
	}

	void SetFireTrigger ()
	{
		//			SendMessageUpwards ("FireTriggered");
		playerSync.FireTriggered ();

		foreach (Animator a in myAnims)
		{
			a.SetTrigger ("fireTrigger");
		}
	}

	bool isShtgFiring = false;

	void ShotGunFire()
	{
//		Debug.Log ("shotgun fire");
		isShtgFiring = true;

		int pelletCounter = 0;

		if (Time.time - fireRate > nextFireTime)
			nextFireTime = Time.time - Time.deltaTime;

		while (nextFireTime < Time.time)
		{
			do {
				StartCoroutine(FireOneShot()); 
				pelletCounter++;       
			} while 
				(pelletCounter < pelletsPerShot);

			SetFireTrigger ();

			EjectShell();
			nextFireTime += fireRate;

			StartCoroutine (WaitShotgunShot (fireRate));
		}
	}

	IEnumerator WaitShotgunShot (float time)
	{
		yield return new WaitForSeconds (time);
		isShtgFiring = false;
	}

	IEnumerator FireOneShot()
	{
		Vector3 position = Camera.main.transform.position;  

		BulletInfo info = new BulletInfo();
		info.damage = damage;
		info.impactForce = impactForce;
		info.maxPenetration = maxPenetration;
		info.maxspread = maxSpread;
		info.spread = spread;
		info.speed = bulletSpeed;
		info.position = this.transform.root.position;
		info.lifeTime = range;

		GameObject newBullet = Instantiate(bullet, position, transform.rotation) as GameObject;
		newBullet.GetComponent<Bullet>().SetUp(info);

		Source.clip = FireSound;
		Source.spread = Random.Range (1.0f, 1.5f);
		Source.Play();

		yield return null;
	}

	void EjectShell()
	{
		if (!ejectPoint)
			return;

		Vector3 position = ejectPoint.position;

		if (shell)
		{
			Quaternion q = Quaternion.Euler (new Vector3 (transform.eulerAngles.x + 90f, transform.eulerAngles.y, 0));
			Rigidbody newShell = Instantiate (shell, position, q) as Rigidbody;
			newShell.velocity = transform.TransformDirection (2,0,0);
		}
	}

	void Aim()
	{
		if (isAimed)
		{
			currentFov = aimFov;
			baseSpread = defaultSpread / 2f; 
			maxSpread = defaultMaxSpread / 2f;
			SendMessageUpwards ("UpdateAimedPosition", true);
			BroadcastMessage ("UpdateSightStatus", true, SendMessageOptions.DontRequireReceiver);
		}
		else
		{      
			currentFov = defaultFov;
			baseSpread = defaultSpread;
			maxSpread = defaultMaxSpread;
			SendMessageUpwards ("UpdateAimedPosition", false);
			BroadcastMessage ("UpdateSightStatus", false, SendMessageOptions.DontRequireReceiver);
		}
		Camera.main.fieldOfView = useSmooth ? 
			Mathf.Lerp(Camera.main.fieldOfView, currentFov, Time.deltaTime * (aimSmooth * 3)) : //apply fog distance
			Mathf.Lerp(Camera.main.fieldOfView, currentFov, Time.deltaTime * aimSmooth);
	}

	IEnumerator WaitAnimationFinish (AnimationClip clip)
	{
		float reloadTime = clip.length;
		yield return new WaitForSeconds (reloadTime);
	}

	void OnEnable()
	{
		Source.clip = TakeSound;
		Source.Play();
		canFire = true;
		canAim = true;
		playerSync.UpdateCurrentGun (GetComponent <GunControlMecanim> ());
	}

	public void DisableWeapon()
	{
		canAim = false;
		canFire = false;
		StopAllCoroutines();
		playerSync.TriggerLowerGunAnimation ();
//		SendMessageUpwards ("TriggerLowerGunAnimation");
	}
		
	public bool CanFire
	{
		get {
			//		if (canFire && !isShtgFiring && !controller.IsRunning) {
			if (canFire && !isShtgFiring) 
				return true;
			
			return false;
		}		
	}

	FirstPersonController controller
	{
		get
		{
			return transform.root.GetComponent<FirstPersonController>();
		}
	}

	PlayerSyncMecanim playerSync
	{
		get
		{
			return PlayerSyncMecanim.Instance;
		}
	}

	PlayerManager playerManager 
	{ 
		get 
		{ 
			return PlayerManager.Instance;
		} 
	}
		
	public bool CanAim
	{
		get
		{
			if (canAim && !controller.IsRunning )
				return true;
			else
				return false;
		}
	}

}	// class