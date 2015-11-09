using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {
	
	private float currentHealth;
	private float maxHealth = 100.0f;
	
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

	public GUIText damageText;

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
		UpdateDamageFactor ();
	}

	void OnCollisionEnter(Collision collision)
	{
		if(((1 << collision.gameObject.layer) & LayerMask.GetMask("Track")) > 0)
		{
			return;
		}

		// Collision with a vehicle
		if(((1 << collision.gameObject.layer) & LayerMask.GetMask("Vehicles")) > 0)
		{
			currentHealth -= damageFromVehicleCollision * damageMultiplier;
			UpdateDamageFactor();
			return;
		}

		// Collision with a shell
		if(((1 << collision.gameObject.layer) & LayerMask.GetMask("Shell")) > 0)
		{
			switch (collision.gameObject.name)
			{
			case "RedShell":
			case "RedShell(Clone)":
				currentHealth -= damageFromRedShellCollision * damageMultiplier;
				break;
			case "GreenShell":
			case "GreenShell(Clone)":
				currentHealth -= damageFromGreenShellCollision * damageMultiplier;
				break;
			case "BlueShell":
			case "BlueShell(Clone)":
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
			damageText.text = "Bonne condition";
			damageText.color = Color.green;
		} else if (healthRatio > lowHealthRatio) {
			// medium-low
			DamageFactor = mediumHealthFactor;
			damageStatus = DamageStatus.MediumHealth;
			damageText.text = "Moyenne condition";
			damageText.color = Color.yellow;
		} else {
			//low-broken
			DamageFactor = lowHealthFactor;
			damageStatus = DamageStatus.LowHealth;
			damageText.text = "Mauvaise condition";
			damageText.color = Color.red;
		}
	}

	public float getCurrentHealth()
	{
		return currentHealth;
	}
	
	public void ResetHealth()
	{
		currentHealth = maxHealth;
		UpdateDamageFactor ();
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
