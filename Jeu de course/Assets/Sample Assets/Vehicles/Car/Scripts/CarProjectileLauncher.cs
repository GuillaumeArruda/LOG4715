﻿using UnityEngine;

public enum ShellColors {Green, Red, Blue};

[RequireComponent(typeof(CarController))]
class CarProjectileLauncher : MonoBehaviour
{
    public Rigidbody blueShell;
    public Rigidbody redShell;
    public Rigidbody greenShell;

	private int blueShellAmmo;
	private int redShellAmmo;
	private int greenShellAmmo;

    public int BlueShellAmmo
    {
        get { return blueShellAmmo; }
        set
        {
            blueShellAmmo = value;
            UpdateAmmo();
        }
    }

    public int RedShellAmmo
    {
        get { return redShellAmmo; }
        set
        {
            redShellAmmo = value;
            Debug.Log(redShellAmmo);
            UpdateAmmo();
        }
    }

    public int GreenShellAmmo
    {
        get { return greenShellAmmo; }
        set
        {
            greenShellAmmo = value;
            Debug.Log(greenShellAmmo);
            UpdateAmmo();
        }
    }

	public GUIText BlueShellAmmoText;
	public GUIText RedShellAmmoText;
	public GUIText GreenShellAmmoText;
	
	[SerializeField] private Texture BlueShellIcon;
	[SerializeField] private Texture RedShellIcon;
	[SerializeField] private Texture GreenShellIcon;

    [SerializeField] private float frontOfVehiculeFireOffset = 10;
    [SerializeField] private float maxTargetDistance = 10;
    [SerializeField] private float maxTargetAcquisitonAngle = 45;

    public void Fire(bool fired, ShellColors color)
    {
        if(fired)
        {
            switch(color)
            {
            case ShellColors.Green:
				if (GreenShellAmmo > 0) {
					GreenShellAmmo--;
                    FireShell(greenShell, ShellColors.Green);
				}
                break;
			case ShellColors.Red:
				if (RedShellAmmo > 0) {
					RedShellAmmo--;
                    FireShell(redShell, ShellColors.Red);
				}
                break;
			case ShellColors.Blue:
				if (BlueShellAmmo > 0) {
					BlueShellAmmo--;
                    FireShell(blueShell, ShellColors.Blue);
				}
                break;
            }
			UpdateAmmo();
        }
    }

    private void FireShell(Rigidbody shell, ShellColors color)
    {
        Rigidbody firedShell;
        firedShell = Instantiate(shell, transform.position, transform.rotation) as Rigidbody;
        firedShell.transform.position += firedShell.transform.forward * frontOfVehiculeFireOffset;
        firedShell.transform.position += firedShell.transform.up * 0.5f;
        firedShell.velocity = firedShell.transform.forward * firedShell.GetComponent<ShellMovementComponent>().GetMaxShellVelocity;
        ShellMovementComponent shellMoveComp = firedShell.GetComponent<ShellMovementComponent>();
        shellMoveComp.enabled = true;
        shellMoveComp.Color = color;

        switch (color)
        {
            case ShellColors.Green:
                return;
            case ShellColors.Red:

                GameObject cars = GameObject.Find("Cars");
                GameObject target = null;

                float distanceToTarget = 0.0f;
                bool first = true;

                foreach(Transform child in cars.transform)
                {
                    if(child.gameObject.name == gameObject.name)
                    {
                        continue;
                    }

                    if(first)
                    {
                        distanceToTarget = (child.transform.position - gameObject.transform.position).magnitude;
                        target = child.gameObject;
                        first = false;                    
                    }

                    float newDistanceToTarget = (child.position - firedShell.transform.position).magnitude;
                    if(distanceToTarget > newDistanceToTarget)
                    {
                        distanceToTarget = newDistanceToTarget;
                        target = child.gameObject;
                    }
                }

                if(distanceToTarget < maxTargetDistance)
                {
                    // Check if target is infront of player
                    Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
                    float dot = Vector3.Dot(direction, transform.forward);
                    float angle = Mathf.Acos(dot) * 180 / Mathf.PI;
                    if(dot > 0.0f && angle <= maxTargetAcquisitonAngle)
                    {
                        // Set projectile target
                        shellMoveComp.Target = target;

                        // Rotate velocity in the direction of the target for a better accuracy
                        firedShell.velocity = Quaternion.AngleAxis(angle / 2.0f, Vector3.Cross(transform.forward, direction)) * firedShell.velocity;
                    }
                }

                break;

            case ShellColors.Blue:
                // Acquire target which is the car in first position
                GameObject gameManager = GameObject.Find("Game Manager");
                GameObject shellTarget = gameManager.GetComponent<RaceManager>().GetFirstPlaceCar();
                if(shellTarget != gameObject)
                {
                    shellMoveComp.Target = shellTarget;
                }
                break;
        }
    }

	private void UpdateAmmo() {
		BlueShellAmmoText.text 	= BlueShellAmmo.ToString ("D2") + "x";
		RedShellAmmoText.text 	= RedShellAmmo.ToString ("D2") + "x";
		GreenShellAmmoText.text = GreenShellAmmo.ToString ("D2") + "x";
	}

	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width - 30, (Screen.height / 2) + 15, 25, 25), BlueShellIcon);
		GUI.DrawTexture(new Rect(Screen.width - 30, (Screen.height / 2) - 15, 25, 25), RedShellIcon);
		GUI.DrawTexture(new Rect(Screen.width - 30, (Screen.height / 2) - 45, 25, 25), GreenShellIcon);
	}
}
