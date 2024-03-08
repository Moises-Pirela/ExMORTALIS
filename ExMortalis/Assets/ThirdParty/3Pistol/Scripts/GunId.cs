using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunId : MonoBehaviour {

	public int gunID;
	public string gunName;

	public AudioClip gunfireClip;
	public AudioSource source;

	public void PlayGunFire()
	{
		source.clip = gunfireClip;
		source.Play();
	}

}
