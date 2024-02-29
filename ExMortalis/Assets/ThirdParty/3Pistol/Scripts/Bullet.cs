using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
	#region Variables
	[HideInInspector]
	public Vector3 directionFrom = Vector3.zero;
	[HideInInspector]
	private int hitCount = 0;         // hit counter for counting bullet impacts for bullet penetration
	private float damage;             // damage bullet applies to a target
	private float maxHits;            // number of collisions before bullet gets destroyed
	private float impactForce;        // force applied to a rigid body object
	private float maxInaccuracy;      // maximum amount of inaccuracy
	private float variableInaccuracy; // used in machineguns to decrease accuracy if maintaining fire
	private float speed;              // bullet speed
	private float lifetime = 1.5f;    // time till bullet is destroyed
	[HideInInspector]
	public string gunName = ""; //Weapon name
	private Vector3 velocity = Vector3.zero; // bullet velocity
	private Vector3 newPos = Vector3.zero;   // bullet's new position
	private Vector3 oldPos = Vector3.zero;   // bullet's previous location
	private bool hasHit = false;             // has the bullet hit something?
	private Vector3 direction;               // direction bullet is travelling
	[Space(5)]
	public AudioSource AudioReferences;
	[SerializeField]private AudioClip concreteSound;
	[SerializeField]private AudioClip genericSound;
	public Vector2 mPicht = new Vector2(1.0f, 1.5f);

	[HideInInspector]
	public enum HitType
	{
		CONCRETE,
		GENERIC
	};

	//HitType type;

	//impact effects for materials
	public GameObject concreteParticle;
	public GameObject genericParticle;

	#endregion // end of variables

	#region Bullet Set Up

	public void SetUp(BulletInfo info) // information sent from gun to bullet to change bullet properties
	{
		damage = info.damage;              // bullet damage
		impactForce = info.impactForce;         // force applied to rigid bodies
		maxHits = info.maxPenetration;             // max number of bullet impacts before bullet is destroyed
		maxInaccuracy = info.maxspread;       // max inaccuracy of the bullet
		variableInaccuracy = info.spread;  // current inaccuracy... mostly for machine guns that lose accuracy over time
		speed = info.speed;               // bullet speed
		directionFrom = info.position;
		lifetime = info.speed;
		// drection bullet is traveling
		direction = transform.TransformDirection(Random.Range(-maxInaccuracy, maxInaccuracy) * variableInaccuracy, Random.Range(-maxInaccuracy, maxInaccuracy) * variableInaccuracy, 1);
		newPos = transform.position;   // bullet's new position
		oldPos = newPos;               // bullet's old position
		velocity = speed * transform.forward; // bullet's velocity determined by direction and bullet speed

		// schedule for destruction if bullet never hits anything
		Destroy(gameObject, lifetime);
	}

	#endregion



	void Update()
	{
		if (hasHit)
			return; // if bullet has already hit its max hits... exit

		// assume we move all the way
		newPos += (velocity + direction) * Time.deltaTime;

		// Check if we hit anything on the way
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;

		if (dist > 0)
		{
			// normalize
			dir /= dist;

			RaycastHit[] hits = Physics.RaycastAll(oldPos, dir, dist);

			Debug.DrawLine(oldPos, newPos, Color.red);
			// Find the first valid hit
			for (int i = 0; i < hits.Length; i++)
			{
				RaycastHit hit = hits[i];

				newPos = hit.point;

                if (hit.rigidbody != null)
                    if (hit.rigidbody.gameObject.tag == "Player")
                        continue;

                OnHit(hit);

				if (hitCount >= maxHits)
				{
					hasHit = true;
					Destroy(gameObject);                    
				}
			}
		}

		oldPos = transform.position;  // set old position to current position
		transform.position = newPos;  // set current position to the new position
	}

	#region Bullet On Hits

	void OnHit(RaycastHit hit)
	{        
		GameObject go = null;
		Ray mRay = new Ray(transform.position, transform.forward);

		if (hit.rigidbody != null && !hit.rigidbody.isKinematic) // if we hit a rigi body... apply a force
		{
			float mAdjust = 1.0f / (Time.timeScale * (0.02f / Time.fixedDeltaTime));
			hit.rigidbody.AddForceAtPosition(((mRay.direction * impactForce) / Time.timeScale) / mAdjust, hit.point);
		}

		switch (hit.transform.tag) // decide what the bullet collided with and what to do with it
		{
		case "Concrete":
			hitCount += 2; // add 2 hits to counter... concrete is hard
			//type = HitType.CONCRETE;
			go = GameObject.Instantiate(concreteParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
			go.transform.parent = hit.transform;
			break;
		case "Invisible": //do nothing
			break;
		case "Generic" :
			hitCount++; // add a hit
			//type = HitType.GENERIC;
			go = GameObject.Instantiate(genericParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
			go.transform.parent = hit.transform;
			break;
		case "Projectile":
			// do nothing if 2 bullets collide
		default:
			break;
		}
	}

	#endregion
}