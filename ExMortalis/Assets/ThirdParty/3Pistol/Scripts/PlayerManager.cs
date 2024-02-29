using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public List <GunId> allGuns = new List <GunId> ();
	public GunId equippedGun;
	public int equippedGunIndex;
	[SerializeField] int startingGunIndex = 0;

	public Transform gunsContainer;

	public Transform playerBase;

	bool isSwitching = false;
	int nearGun = -1;
	int nextGun = -1;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		foreach (GunId gun in allGuns) {
			gun.gameObject.SetActive (false);
		}
		TakeWeapon (startingGunIndex);

		initialPos = playerBase.localPosition;
		target = initialPos;
	}

	void OnEnable ()
	{
		PlayerSyncLegacy.onFinishedDisablingAnimation 	+= OnDisableWeaponCallback;
		PlayerSyncLegacy.onFinishedRaisingAnimation 	+= OnRaiseWeaponCallback;

		PlayerSyncMecanim.onFinishedDisablingAnimation 	+= OnDisableWeaponCallback;
		PlayerSyncMecanim.onFinishedRaisingAnimation 	+= OnRaiseWeaponCallback;

//		CustomSightView.onAdjustSightView 				+= OnAdjustSightView;
	}

	void OnDisable ()
	{
		PlayerSyncLegacy.onFinishedDisablingAnimation 	-= OnDisableWeaponCallback;
		PlayerSyncLegacy.onFinishedRaisingAnimation 	-= OnRaiseWeaponCallback;

		PlayerSyncMecanim.onFinishedDisablingAnimation 	-= OnDisableWeaponCallback;
		PlayerSyncMecanim.onFinishedRaisingAnimation 	-= OnRaiseWeaponCallback;

//		CustomSightView.onAdjustSightView 				-= OnAdjustSightView;
	}

	//bool adjustSightOn = false;
	Vector3 target;
	Vector3 initialPos;

	void OnAdjustSightView (float amount)
	{
		if (amount != 0)
			target = new Vector3 (playerBase.localPosition.x, initialPos.y - amount, playerBase.localPosition.z);
		else
			target = new Vector3 (playerBase.localPosition.x, initialPos.y, playerBase.localPosition.z);	
	}

	public void UpdateAimedPosition (bool isAimed)
	{
		if (isAimed)
			playerBase.localPosition = Vector3.Slerp (playerBase.localPosition, target, Time.deltaTime * 10f);
		else
			playerBase.localPosition = initialPos;
	}

	void Update ()
	{
		//		if (equippedGun.isAimed)
		//			playerBase.localPosition = Vector3.Slerp (playerBase.localPosition, target, Time.deltaTime * 10f);
		//		else
		//			playerBase.localPosition = initialPos;

		if (Input.GetKeyDown(KeyCode.E) && CanSwitch)
		{
			isSwitching = true;
			nextGun = nearGun;
			//			equippedGun.DisableWeapon ();
			BroadcastMessage ("DisableWeapon");
		}
	}

	void OnTriggerEnter (Collider hit)
	{
		if (hit.transform.tag == "GunAtDisplay") {
			nearGun = hit.GetComponent <GunAtDisplay> ().gunID;
		}
	}

	void OnTriggerExit (Collider hit)
	{
		if (hit.transform.tag == "GunAtDisplay") {
			nearGun = -1;
		}
	}

	void OnDisableWeaponCallback ()
	{
		equippedGun.gameObject.SetActive (false);

		int index = allGuns.FindIndex (x => x.gunID == nextGun);

		nextGun = -1;

		if (index != -1)
			TakeWeapon (index);
		else
			Debug.LogWarning ("Could not find weapon ID on the list!");
	}

	void OnRaiseWeaponCallback ()
	{
		nextGun = -1;
		isSwitching = false;
	}

	void TakeWeapon (int index)
	{
		equippedGunIndex = index;
		equippedGun = allGuns [index];
		equippedGun.gameObject.SetActive (true);
		//		playerSync.WpState = WeaponState.Raising;
	}

	static PlayerManager instance = null;
	public static PlayerManager Instance
	{
		get 
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType <PlayerManager> ();
			return instance;
		}
	}

	PlayerSyncLegacy playerSync
	{
		get { return PlayerSyncLegacy.Instance; }
	}

	bool CanSwitch 
	{
		//		get { return ((!isSwitching && !equippedGun.IsReloading && nearGun != -1 && nearGun != equippedGun.gunID) ? true : false); }
		get { return ((!isSwitching && nearGun != -1 && nearGun != equippedGun.gunID) ? true : false); }
	}

	public bool IsSwitching 
	{ 
		get { return isSwitching; } 
	}

}
