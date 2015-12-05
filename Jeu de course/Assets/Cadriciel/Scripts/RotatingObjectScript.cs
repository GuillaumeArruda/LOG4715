using UnityEngine;
using System.Collections;

public class RotatingObjectScript : MonoBehaviour
{
	[SerializeField]
	private float rotateSpeed = 10.0f;
	
	void FixedUpdate() {
		// Rotate the object
		transform.Rotate(Vector3.forward * rotateSpeed);
	}
}
