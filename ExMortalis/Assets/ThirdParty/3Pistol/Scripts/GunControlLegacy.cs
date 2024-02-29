using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AudioSource))]
public class GunControlLegacy : MonoBehaviour
{
	#region Fields

	public GunAnimations gunAnimations;

	private Animation[] myAnims;

	private bool canFire = true;
	private bool m_enabled = true;
	public enum weaponType { Shotgun, Machinegun, Sniper, Pistol };
	public weaponType typeOfGun;

	public AudioClip TakeSound;
	public AudioClip FireSound;

	public GameObject bullet = null;  
	public Transform ejectPoint = null;  
	public Rigidbody shell = null;      

	[HideInInspector]
	public bool isAimed;
	private bool canAim;
	public bool useSmooth = true;
	[Range (1f, 10f)]
	public float aimSmooth;
	[Range(0, 179)]
	public float aimFov = 35;
    private float defaultFov;
	private float currentFov;
	private Quaternion camPosition;
	private Quaternion defaultCamRot;

	[HideInInspector]
	public bool firePressed = false;  

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
	private float defaultSpread;
	private float defaultMaxSpread;

	bool isReloading = false; 
	bool disableWeapon = false;

	private float nextFireTime = 0.0f; 

	private AudioSource Source;

	#endregion

	void Awake()
	{
		Source = GetComponent <AudioSource>();
		defaultSpread = baseSpread;
		defaultMaxSpread = maxSpread;
		myAnims = GetComponentsInChildren <Animation> ();
		disableWeapon = false;
	}

	void OnEnable()
	{
		Source.clip = TakeSound;
		Source.Play();
		canFire = true;
		canAim = true;
		disableWeapon = false;
		playerSync.UpdateCurrentGun (GetComponent <GunControlLegacy> ());
		playerSync.WpState = WeaponState.Raising;
	}

	void OnDisable ()
	{
		disableWeapon = false;
	}

	void Start()
	{
		defaultCamRot = Camera.main.transform.localRotation;
		defaultFov = Camera.main.fieldOfView;
		canAim = true;
	}

	bool Update()
	{
		if (!m_enabled)
			return false;

		InputUpdate();
		Aim();
		SyncState();

		if (firePressed)
			spread += spreadPerSecond;
		else
			spread -= decreaseSpreadPerSec; 

		return true;
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
		if (Input.GetMouseButton (0) && CanFire) {
			firePressed = true;
		} else {
			firePressed = false;
		}
		switch (typeOfGun)
		{
		case weaponType.Shotgun:
			if (Input.GetMouseButtonDown(0) && CanFire)
			{
				ShotGun_Fire(); 
			}
			break;
		case weaponType.Machinegun:
			if (Input.GetMouseButton (0) && CanFire) 
			{
				MachineGun_Fire ();          
			} 
			break;
		case weaponType.Sniper:
		case weaponType.Pistol:
			if (Input.GetMouseButtonDown (0) && CanFire) 
			{
				ShotGun_Fire ();  
			} 
			break;
		default :
			break;
		}

		if (Input.GetButton ("Fire2") && CanAim)
			isAimed = true;
		else
			isAimed = false;


		if (Input.GetKeyDown(KeyCode.R) && CanReload)
		{
			isAimed = false;
			canFire = false;
			isReloading = true;
		}
	}

	void SyncState()
	{
		//		if (!isAimed && firePressed && !isReloading && !disableWeapon)
		if (!isAimed && firePressed && !isReloading && !disableWeapon)
		{
			if (playerSync) {
				if (playerSync.WpState != WeaponState.Firing)
					playerSync.WpState = WeaponState.Firing;
			}
		}
		else if (isAimed && !firePressed && !isShtgFiring && !isReloading && !disableWeapon && !controller.IsWalking)
		{
			if (playerSync)
			{
				playerSync.WpState = WeaponState.AimedIdle;
				PlayGunAnimation (gunAnimations.Gun_Idle.name, WrapMode.Loop);
			}
		}
		else if (isAimed && !firePressed && !isShtgFiring && !isReloading && !disableWeapon && controller.IsWalking)
		{
			if (playerSync)
			{
				playerSync.WpState = WeaponState.AimedWalking;
			}
		}
		else if (isAimed && firePressed && !isReloading && !disableWeapon)
		{
			if (playerSync)
			{
				if (playerSync.WpState != WeaponState.AimedFiring)
					playerSync.WpState = WeaponState.AimedFiring;
			}
		}
		else if (isReloading && !disableWeapon)
		{
			if (playerSync && playerSync.WpState != WeaponState.Reloading)
			{
				playerSync.WpState = WeaponState.Reloading;
			}
		}
		else if (disableWeapon)
		{
			if (playerSync)
			{
				playerSync.WpState = WeaponState.Lowering;
			}
		}
		else if (controller.IsRunning && !isShtgFiring && !isReloading && !disableWeapon && !firePressed && !isAimed)
		{
			if (playerSync)
			{
				playerSync.WpState = WeaponState.Running;
			}
		}
		else if (controller.IsWalking && !isShtgFiring && !isReloading && !disableWeapon && !firePressed && !isAimed)
		{
			if (playerSync)
			{
				playerSync.WpState = WeaponState.Walking;
				PlayGunAnimation (gunAnimations.Gun_Idle.name, WrapMode.Loop);
			}
		}
		else if (!isShtgFiring)
		{
			if (playerSync) 
			{
				if (playerSync.WpState != WeaponState.Idle) 
				{
					playerSync.WpState = WeaponState.Idle;
					PlayGunAnimation (gunAnimations.Gun_Idle.name, WrapMode.Loop);
				}
			}
		}
	}

	void PlayGunAnimation (string name, WrapMode wp = WrapMode.Default)
	{
//		myAnims.wrapMode = wp;
//		myAnims.Play (name);

		foreach (Animation a in myAnims)
		{
			a.wrapMode = wp;
			a.Play (name);
		}
	}

	void MachineGun_Fire()
	{
		if (Time.time - fireRate > nextFireTime)
			nextFireTime = Time.time - Time.deltaTime;

		while (nextFireTime < Time.time)
		{
			StartCoroutine(FireOneShot());
			Source.clip = FireSound;
			Source.spread = Random.Range(1.0f, 1.5f);
			Source.pitch = Random.Range(1.0f, 1.05f);
			Source.Play();
			nextFireTime += fireRate;
			EjectShell();
			PlayGunAnimation (gunAnimations.Gun_Fire.name, WrapMode.Once);
		}
	}

	bool isShtgFiring = false;

	void ShotGun_Fire()
	{
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

			EjectShell();
			PlayGunAnimation (gunAnimations.Gun_Fire.name, WrapMode.Once);
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

	/// <summary>
	/// PLayer Sync must call the following 3 methods in sequence.
	/// </summary>
	public void ShotgunReloadStart ()
	{
		PlayGunAnimation (gunAnimations.Shotgun_ReloadStart.name);
	}

	public void ShotgunReloadLoop ()
	{
		PlayGunAnimation (gunAnimations.Shotgun_ReloadLoop.name, WrapMode.Loop);
	}

	public void ShotgunReloadEnd ()
	{
		PlayGunAnimation (gunAnimations.Shotgun_ReloadEnd.name);

		isReloading = false;
		canAim = true;
		canFire = true;
	}

	/// <summary>
	/// Reload procedure for machine guns and pistols.
	/// </summary>
	public IEnumerator ReloadNormal()
	{
		PlayGunAnimation (gunAnimations.Gun_Reload.name, WrapMode.Once);
		yield return StartCoroutine (WaitAnimationFinish (gunAnimations.Gun_Reload));
		isReloading = false;
		canAim = true;
		canFire = true;
	}

	IEnumerator WaitAnimationFinish (AnimationClip clip)
	{
		float reloadTime = clip.length;
		yield return new WaitForSeconds (reloadTime);
	}

	public void DisableWeapon()
	{
		//		print ("disable weapon");
		canAim = false;
		isReloading = false;
		canFire = false;
		disableWeapon = true;
		StopAllCoroutines();
	}

	FirstPersonController controller
	{
		get
		{
			return transform.root.GetComponent <FirstPersonController>();
		}
	}

	PlayerSyncLegacy playerSync
	{
		get
		{
			return PlayerSyncLegacy.Instance;
		}
	}

	PlayerManager playerManager 
	{ 
		get 
		{ 
			return PlayerManager.Instance;
		} 
	}

	public bool CanFire
	{
		get
		{
			if (canFire && !isShtgFiring && !isReloading && !controller.IsRunning)
				return true;
			else
				return false;
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

	public bool CanReload
	{
		get
		{
			if (!isReloading && !playerSync.AreArmsReloading) 
			{
				if (controller.IsWalking || controller.IsQuiet)
					return true;

				return false;
			}
			return false;
		}
	}

	public bool IsReloading
	{
		get 
		{
			return isReloading;
		}
	}

}	// class