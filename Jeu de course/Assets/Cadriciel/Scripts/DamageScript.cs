using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {
	
	private float currentHealth;
	private float maxHealth = 100.0f;

	const int shellsLayer = 7;
	const int vehiclesLayer = 8;
	
	[SerializeField]
	private float damageMultiplier = 1.0f; // Higher multiplier means damage taken will increase
	[SerializeField]
	private float damageFromVehicleCollision = 5.0f;
	[SerializeField]
	private float damageFromGreenShellCollision = 5.0f;
	[SerializeField]
	private float damageFromRedShellCollision = 5.0f;
	[SerializeField]
	private float damageFromBlueShellCollision = 5.0f;
	
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "Track")
		{
			return;
		}
		
		// Collision with a vehicle
		if((collision.gameObject.layer & vehiclesLayer) > 0)
		{
			currentHealth -= damageFromVehicleCollision * damageMultiplier;
			return;
		}
		
		// Collision with a shell
		if((collision.gameObject.layer & shellsLayer) > 0)
		{
			currentHealth -= 5.0f * damageMultiplier;
			return;
		}
	}

	public float DamageFactor
	{
		get;
		set;
	}
}
