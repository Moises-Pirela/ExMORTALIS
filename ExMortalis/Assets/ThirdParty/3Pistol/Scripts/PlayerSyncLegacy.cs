using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponState 
{ 
	AimedIdle,
	AimedWalking,
	AimedFiring,
	Idle,
	Walking,
	Running, 
	Firing, 
	Reloading,
	Lowering,
	Raising
}

public class PlayerSyncLegacy : MonoBehaviour {
	
	Animation myAnim = null;
	GunControlLegacy curGun = null;
	
	bool areArmsReloading = false;
	
	void Awake ()
	{
		instance = this;
		myAnim = GetComponent <Animation> ();
		UpdateAnimation ();
	}
	
	void Start ()
	{
		UpdateAnimation ();
	}
	
	public void UpdateCurrentGun (GunControlLegacy _curGun)
	{
		this.curGun = _curGun;
	}
	
	public void UpdateCurrentAnimation (Animation curAnim)
	{
		this.myAnim = curAnim;
	}
	
	void UpdateAnimation ()
	{
		if (!curGun || !myAnim)
			return;
		
		switch (wpState) 
		{
		case WeaponState.Idle:
			myAnim.CrossFade (curGun.gunAnimations.Arms_Idle.name);
			break;
		case WeaponState.Walking:
			myAnim.CrossFade (curGun.gunAnimations.Arms_Walk.name);
			break;
		case WeaponState.Running:
			myAnim.CrossFade (curGun.gunAnimations.Arms_Sprint.name);
			break;
		case WeaponState.Firing:
			myAnim.Rewind ();
			myAnim.Play (curGun.gunAnimations.Arms_Fire.name);
			break; 
		case WeaponState.Reloading:
			if (curGun.typeOfGun == GunControlLegacy.weaponType.Shotgun) {
				StartCoroutine (ShotgunReload ());
			} else {
				if (curGun.gunAnimations.Arms_Reload)
					myAnim.Play (curGun.gunAnimations.Arms_Reload.name);
				curGun.StartCoroutine (curGun.ReloadNormal ());
			}
			break;
		case WeaponState.Lowering:
			StartCoroutine (DisableGun (curGun.gunAnimations.Arms_Lower));
			break;
		case WeaponState.Raising:
			StartCoroutine (RaiseGun (curGun.gunAnimations.Arms_Raise));
			break;
		case WeaponState.AimedIdle:
			myAnim.CrossFade (curGun.gunAnimations.ADS_Idle.name, 0.2f);
			break;
		case WeaponState.AimedWalking:
			myAnim.CrossFade (curGun.gunAnimations.ADS_Walk.name, 0.2f);
			break;
		case WeaponState.AimedFiring:
			myAnim.Rewind ();
			myAnim.Play (curGun.gunAnimations.ADS_Fire.name);
			break;
		default :
			break;
		}
	}
	
	bool isSwitching = false;
	
	IEnumerator DisableGun (AnimationClip clip)
	{
		if (isSwitching) yield break;
		isSwitching = true;
		myAnim.Play (clip.name);
		yield return StartCoroutine (WaitAnimationFinish (clip));
		isSwitching = false;
		onFinishedDisablingAnimation ();
	}
	
	IEnumerator RaiseGun (AnimationClip clip)
	{
		myAnim.Play (clip.name);
		yield return StartCoroutine (WaitAnimationFinish (clip));
		onFinishedRaisingAnimation ();
	}
	
	IEnumerator ShotgunReload ()
	{
		if (areArmsReloading) yield break;
		
		areArmsReloading = true;
		
		if (curGun.typeOfGun == GunControlLegacy.weaponType.Shotgun) {
			curGun.ShotgunReloadStart ();
			myAnim.Play (curGun.gunAnimations.Shotgun_Arms_ReloadStart.name);
			yield return StartCoroutine (WaitAnimationFinish (curGun.gunAnimations.Shotgun_Arms_ReloadStart));
			
			for (int i = 0; i < 4; i++) 
			{
				curGun.ShotgunReloadLoop ();
				if (i != 0)
					myAnim.Rewind ();
				myAnim.CrossFade (curGun.gunAnimations.Shotgun_Arms_ReloadLoop.name);
				yield return StartCoroutine (WaitAnimationFinish (curGun.gunAnimations.Shotgun_Arms_ReloadLoop));
			}
			
			curGun.ShotgunReloadEnd ();
			myAnim.Play (curGun.gunAnimations.Shotgun_Arms_ReloadEnd.name);
			yield return StartCoroutine (WaitAnimationFinish (curGun.gunAnimations.Shotgun_Arms_ReloadEnd));
		} 
		
		areArmsReloading = false;
	}
	
	IEnumerator WaitAnimationFinish (AnimationClip clip)
	{
		float reloadTime = clip.length;
		yield return new WaitForSeconds (reloadTime);
	}
	
	public static event Action onFinishedDisablingAnimation;
	public static event Action onFinishedRaisingAnimation;
	
	private static PlayerSyncLegacy instance = null;
	public static PlayerSyncLegacy Instance
	{
		get 
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType <PlayerSyncLegacy> ();
			return instance;
		}
	}
	
	private WeaponState wpState;
	public WeaponState WpState 
	{ 
		get 
		{ 
			return wpState; 
		} 
		set 
		{ 
			wpState = value; 
			UpdateAnimation ();
		} 
	}
	
	PlayerManager pManager 
	{ 
		get 
		{ 
			return PlayerManager.Instance;
		} 
	}
	
	public bool AreArmsReloading 
	{ 
		get 
		{ 
			return areArmsReloading; 
		} 
	}
}
