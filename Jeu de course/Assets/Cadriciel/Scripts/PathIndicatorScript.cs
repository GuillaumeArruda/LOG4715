using UnityEngine;
using System.Collections;

public class PathIndicatorScript : MonoBehaviour {
	
	private float pathBoxWidth;
	private float pathBoxHeight;
	private float pathAngle;
	private Vector2 pivotPoint;

	private Transform car;
	
	[SerializeField]
	private Texture pathBox;
	[SerializeField]
	private Texture pathArrow;
	
	// Use this for initialization
	void Start () {
		car = GetComponent<Transform> ();
		pathBoxWidth = Screen.width / 7;
		pathBoxHeight = Screen.height / 7;
	}
	
	public void UpdateWaypoint(Transform target) {
		// Get the next waypoint target to show on the GUI
		// Ignore the Y coord to ignore height differences between 
		Vector3 direction = target.position - car.position;
		direction.y = 0;

		Vector3 carForward = car.forward;
		carForward.y = 0;

		pathAngle = Vector3.Angle (direction, carForward);

		if (Vector3.Cross (direction, carForward).y > 0) {
			pathAngle = -pathAngle;
		}
	}
	
	public void OnGUI()
	{
		// Base Box
		GUI.DrawTexture(new Rect(0, ((Screen.height/2) - (pathBoxHeight/2)), pathBoxWidth, pathBoxHeight), pathBox);
		
		// Direction Arrow
		pivotPoint = new Vector2 (pathBoxWidth / 2, Screen.height/2);
		GUIUtility.RotateAroundPivot(pathAngle, pivotPoint);
		GUI.DrawTexture (new Rect (pathBoxWidth / 2, Screen.height/2, pathBoxHeight/10, -0.90f * pathBoxHeight), pathArrow);
	}
}
