using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSyncMecanim : MonoBehaviour {

	Rigidbody rigibody;
	GunControlMecanim curGun = null;

	Animator curAnim;
	public Animator Anim { get { return curAnim; } }

	void Awake ()
	{
		instance = this;
		curAnim = GetComponent <Animator> ();
		rigibody = GetComponentInParent <Rigidbody> ();
	}
			
	void Update () 
	{
		UpdateAnimatorParameters ();
	}

	void UpdateAnimatorParameters ()
	{
		curAnim.SetBool ("isWalking", controller.IsWalking);
		curAnim.SetBool ("isRunning", controller.IsRunning);
		curAnim.SetBool ("isQuiet", controller.IsQuiet);
		curAnim.SetBool ("isJumping", controller.IsJumping);

		curAnim.SetBool ("firePressed", curGun.FirePressed);
		curAnim.SetBool ("isAimed", curGun.IsAimed);

		if (Input.GetKeyDown (KeyCode.R) && curAnim.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
			curAnim.SetTrigger ("reload");

		if (isLoweringGun)
		{
			if (curAnim.GetCurrentAnimatorStateInfo (0).IsName ("Lower") &&
				(curAnim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9f))
			{
				// disable callback
				onFinishedDisablingAnimation ();
				isLoweringGun = false;
			}
		}

		if (isRaisingGun)
		{
			if (curAnim.GetCurrentAnimatorStateInfo (0).IsName ("Raise") &&
				(curAnim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9f))
			{
				// raise callback
				onFinishedRaisingAnimation ();
				isRaisingGun = false;
			}
		}
	}

	public void FireTriggered ()
	{
		curAnim.SetTrigger ("fireTrigger");
	}

	bool isLoweringGun;

	public void TriggerLowerGunAnimation ()
	{		
		curAnim.SetTrigger ("disable");
		isLoweringGun = true;
	}

	bool isRaisingGun;
		
	public void UpdateCurrentGun (GunControlMecanim _curGun)
	{
		this.curGun = _curGun;

		if (curAnim != null)
			curAnim.runtimeAnimatorController = _curGun.properArmsAnimation as RuntimeAnimatorController;
	
		isRaisingGun = true;
	}

	FirstPersonController controller
	{
		get
		{
			return transform.root.GetComponent<FirstPersonController>();
		}
	}

	public static event Action onFinishedDisablingAnimation;
	public static event Action onFinishedRaisingAnimation;

	private static PlayerSyncMecanim instance = null;
	public static PlayerSyncMecanim Instance
	{
		get 
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType <PlayerSyncMecanim> ();
			return instance;
		}
	}
}
