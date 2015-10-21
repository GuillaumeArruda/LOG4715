using UnityEngine;

public enum ShellColors {Green, Red, Blue};

[RequireComponent(typeof(CarController))]
class CarProjectileLauncher : MonoBehaviour
{
    public Rigidbody blueShell;
    public Rigidbody redShell;
    public Rigidbody greenShell;

    [SerializeField] private float frontOfVehiculeFireOffset = 10;

    public void Fire(bool fired, ShellColors color)
    {
        if(fired)
        {
            switch(color)
            {
                case ShellColors.Green:
                    FireShell(greenShell);
                    break;
                case ShellColors.Red:
                    FireShell(redShell);
                    break;
                case ShellColors.Blue:
                    FireShell(blueShell);
                    break;
            }
        }
    }

    private void FireShell(Rigidbody shell)
    {
        Rigidbody firedShell;
        firedShell = Instantiate(shell, transform.position, transform.rotation) as Rigidbody;
        firedShell.transform.position += firedShell.transform.forward * frontOfVehiculeFireOffset;
        firedShell.transform.position += firedShell.transform.up * 0.5f;
        firedShell.velocity = firedShell.transform.forward * firedShell.GetComponent<ShellMovementComponent>().GetMaxShellVelocity;
        firedShell.GetComponent<ShellMovementComponent>().enabled = true;
    }
}
