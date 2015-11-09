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

	// Above 75% HP, 			Car goes at 100% of max speed
	// Between 75% and 40% HP, 	Car goes at 80% of max speed
	// Under 40% HP, 			Car goes at 60% of max speed
	[SerializeField]
	private float mediumHealthRatio = 0.75f;
	[SerializeField]
	private float mediumHealthFactor = 0.8f;
	[SerializeField]
	private float lowHealthRatio = 0.4f;
	[SerializeField]
	private float lowHealthFactor = 0.6f;

	public enum DamageStatus
	{
		GoodHealth,
		MediumHealth,
		LowHealth,
	}
	DamageStatus damageStatus;
	
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		damageStatus = DamageStatus.GoodHealth;
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
			UpdateDamageFactor();
			return;
		}

		// Collision with a shell
		if((collision.gameObject.layer & shellsLayer) > 0)
		{
			switch (collision.gameObject.name)
			{
			case "Red Shell":
				currentHealth -= damageFromRedShellCollision * damageMultiplier;
				break;
			case "Green Shell":
				currentHealth -= damageFromGreenShellCollision * damageMultiplier;
				break;
			case "Blue Shell":
				currentHealth -= damageFromBlueShellCollision * damageMultiplier;
				break;
			default:
				break;
			}
			UpdateDamageFactor();
			return;
		}
	}

	public void UpdateDamageFactor() {
		float healthRatio = currentHealth / maxHealth;

		if (healthRatio > mediumHealthRatio) {
			// 100-medium
			DamageFactor = 1.0f;
			damageStatus = DamageStatus.GoodHealth;
		} else if (healthRatio > lowHealthRatio) {
			// medium-low
			DamageFactor = mediumHealthFactor;
			damageStatus = DamageStatus.MediumHealth;
		} else {
			//low-broken
			DamageFactor = lowHealthFactor;
			damageStatus = DamageStatus.LowHealth;
		}
	}

	public float getCurrentHealth()
	{
		return currentHealth;
	}
	
	public void ResetHealth()
	{
		currentHealth = maxHealth;
	}

	public DamageStatus getHealthStatus()
	{
		return damageStatus;
	}

	public float DamageFactor
	{
		get;
		set;
	}
}
