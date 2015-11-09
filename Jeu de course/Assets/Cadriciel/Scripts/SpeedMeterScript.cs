using UnityEngine;
using System.Collections;

public class SpeedMeterScript : MonoBehaviour {

	private float speedMeterWidth;
	private float speedMeterHeight;
	private float speedDegSpin;
	private Vector2 pivotPoint;

	[SerializeField]
	private Texture speedMeterGoodHealth;
	[SerializeField]
	private Texture speedMeterMediumHealth;
	[SerializeField]
	private Texture speedMeterLowHealth;
	[SerializeField]
	private Texture speedMeterBar;
	
	// Use this for initialization
	void Start () {
		speedMeterWidth = Screen.width / 5;
		speedMeterHeight = Screen.height / 5;
	}
	
	public void UpdateSpeed(float currSpeed, float maxSpeed) {
		// Calculate the speed percentage to show on the GUI
		// 100% speed = 1 = 180 degree spin
		speedDegSpin = (Mathf.Abs(currSpeed)  / maxSpeed) * 180;

		//Clip the speed needle rotation
		if (speedDegSpin < 0)
			speedDegSpin = 0;
		else if (speedDegSpin > 180)
			speedDegSpin = 180;
	}

	public void OnGUI()
	{
		// Base Meter
		DamageScript damage = GetComponent<DamageScript> ();
		switch (damage.getHealthStatus ()) {
		case DamageScript.DamageStatus.GoodHealth:
			GUI.DrawTexture(new Rect(0, (Screen.height - speedMeterHeight), speedMeterWidth, speedMeterHeight), speedMeterGoodHealth);
			break;
		case DamageScript.DamageStatus.MediumHealth:
			GUI.DrawTexture(new Rect(0, (Screen.height - speedMeterHeight), speedMeterWidth, speedMeterHeight), speedMeterMediumHealth);
			break;
		case DamageScript.DamageStatus.LowHealth:
			GUI.DrawTexture(new Rect(0, (Screen.height - speedMeterHeight), speedMeterWidth, speedMeterHeight), speedMeterLowHealth);
			break;
		default:
			break;
		}

		// Speed Needle
		pivotPoint = new Vector2 (speedMeterWidth / 2, (Screen.height - speedMeterHeight/20));
		GUIUtility.RotateAroundPivot(speedDegSpin, pivotPoint);
		GUI.DrawTexture (new Rect (speedMeterWidth / 2, (Screen.height - speedMeterHeight/10), -0.90f * speedMeterHeight, speedMeterHeight/10), speedMeterBar);
	}
}
