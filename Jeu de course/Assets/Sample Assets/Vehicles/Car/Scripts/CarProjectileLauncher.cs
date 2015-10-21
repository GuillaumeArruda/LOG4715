using UnityEngine;

public enum ShellColors {Green, Red, Blue};

[RequireComponent(typeof(CarController))]
class CarProjectileLauncher : MonoBehaviour
{
    public Rigidbody blueShell;
    public Rigidbody redShell;
    public Rigidbody greenShell;

    [SerializeField] private float greenShellShootingForce = 5000;
    [SerializeField] private float redShellShootingForce = 5000;
    [SerializeField] private float blueShellShootingForce = 5000;
    [SerializeField] private float frontOfVehiculeFireOffset = 10;

    public void Fire(bool fired, ShellColors color)
    {
        if(fired)
        {
            switch(color)
            {
                case ShellColors.Green:
                    FireGreenShell();
                    break;
                case ShellColors.Red:
                    FireRedShell();
                    break;
                case ShellColors.Blue:
                    FireBlueShell();
                    break;
            }
        }
    }

    private void FireGreenShell()
    {
        Rigidbody shell;
        shell = Instantiate(greenShell, transform.position, transform.rotation) as Rigidbody;
        shell.transform.position += shell.transform.forward * frontOfVehiculeFireOffset;
        shell.AddForce(transform.forward * greenShellShootingForce);
    }

    private void FireRedShell()
    {
        Rigidbody shell;
        shell = Instantiate(redShell, transform.position, transform.rotation) as Rigidbody;
        shell.transform.position += shell.transform.forward * frontOfVehiculeFireOffset;
        shell.AddForce(transform.forward * redShellShootingForce);
    }

    private void FireBlueShell()
    {
        Rigidbody shell;
        shell = Instantiate(blueShell, transform.position, transform.rotation) as Rigidbody;
        shell.transform.position += shell.transform.forward * frontOfVehiculeFireOffset;
        shell.AddForce(transform.forward * blueShellShootingForce);
    }
}
