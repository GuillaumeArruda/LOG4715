using UnityEngine;

public enum ShellColors {Green, Red, Blue};

[RequireComponent(typeof(CarController))]
class CarProjectileLauncher : MonoBehaviour
{
    public Rigidbody blueShell;
    public Rigidbody redShell;
    public Rigidbody greenShell;

    [SerializeField] private float frontOfVehiculeFireOffset = 10;
    [SerializeField] private float maxTargetDistance = 10;

    public void Fire(bool fired, ShellColors color)
    {
        if(fired)
        {
            switch(color)
            {
                case ShellColors.Green:
                    FireShell(greenShell, ShellColors.Green);
                    break;
                case ShellColors.Red:
                    FireShell(redShell, ShellColors.Red);
                    break;
                case ShellColors.Blue:
                    FireShell(blueShell, ShellColors.Blue);
                    break;
            }
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

        switch (color)
        {
            case ShellColors.Green:
                return;
            case ShellColors.Red:
            case ShellColors.Blue:

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
                    if(Vector3.Dot(target.transform.position - transform.position, transform.forward) > 0.0f)
                    {
                        shellMoveComp.Target = target;
                    }
                }
                break;
        }
    }
}
