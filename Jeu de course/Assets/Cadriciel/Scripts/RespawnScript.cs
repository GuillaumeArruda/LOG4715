using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {
	
	private Transform car;
	private Transform target;
	private float timeFlipped;
	
	[SerializeField]
	private float distanceToWaypointBeforeRespawn = 100.0f;
	[SerializeField]
	private float timeFlippedBeforeRespawn = 5.0f;

	
	// Use this for initialization
	void Start () {
		car = GetComponent<Transform> ();
		target = GetComponent<WaypointProgressTracker> ().getTarget ();
	}
	
	// Possible ways to require a respawn would be:
	// Car is broken
	// Car is under the map
	// Car is upside down and can no longer move
	// Car is too far away from the target waypoint
	void FixedUpdate() {

		// Check if the car is destroyed
		DamageScript damage = GetComponent<DamageScript> ();
		if (damage.getCurrentHealth () <= 0.0f) {
			damage.ResetHealth();
			Debug.Log("Respawn: Damage");
			Respawn();
		}

		// Check if the car is under the map
		if (car.position.y < -50.0f) {
			Debug.Log("Respawn: Under the map");
			Respawn();
		}

		// Check if the car is upside down
		if (Vector3.Angle (car.up, Vector3.up) > 90.0f) {
			timeFlipped += Time.fixedDeltaTime;

			if (timeFlipped > timeFlippedBeforeRespawn) {
				Debug.Log("Respawn: Car upside down");
				Respawn();
			}
		} else {
			timeFlipped = 0.0f;
		}

		// Check the distance between
		if (target != null) {
			// Get the next waypoint target to calculate distance
			Vector3 direction = target.position - car.position;
			direction.y = 0;
		
			if (direction.magnitude > distanceToWaypointBeforeRespawn) {
				Debug.Log("Respawn: Too far");
				Respawn ();
			}
		}
	}

	public void UpdateWaypoint(Transform targetArg) {
		target = targetArg;
	}

	void Respawn()
	{
		DamageScript damage = GetComponent<DamageScript> ();
		damage.UpdateDamageFactor ();

		car.transform.position = (target.position + new Vector3(0, 1, 0));

		car.transform.rotation = target.rotation;

		Rigidbody carBody = GetComponent<Rigidbody> ();
		carBody.velocity = Vector3.zero;
	}
}
