using UnityEngine;
using System;

[Serializable]
public class GunAnimations {

	public AnimationClip Arms_Idle;
	public AnimationClip Arms_Walk;
	public AnimationClip Arms_Sprint;
	public AnimationClip Arms_Fire;
	public AnimationClip Arms_Lower;
	public AnimationClip Arms_Raise;
	public AnimationClip Arms_Reload;

	public AnimationClip ADS_Idle;
	public AnimationClip ADS_Walk;
	public AnimationClip ADS_Fire;

	public AnimationClip Gun_Idle;
	public AnimationClip Gun_Fire;
	public AnimationClip Gun_Reload;	// if shotgun, leave this empty

	// only for shotguns
	public AnimationClip Shotgun_ReloadStart;
	public AnimationClip Shotgun_ReloadLoop;
	public AnimationClip Shotgun_ReloadEnd;

	public AnimationClip Shotgun_Arms_ReloadStart;
	public AnimationClip Shotgun_Arms_ReloadLoop;
	public AnimationClip Shotgun_Arms_ReloadEnd;
}
