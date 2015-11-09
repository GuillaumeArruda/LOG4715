using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarUserControlMP : MonoBehaviour
{
	private CarController car;  // the car controller we want to use
    private JumpScript jumpScript;
    private NitroScript nitroScript;

	[SerializeField]
	private string vertical = "Vertical";

	[SerializeField]
	private string horizontal = "Horizontal";


	private string fire1 = "Fire1";
	private string fire2 = "Fire2";
	private string fire3 = "Fire3";
    private string nitro = "Nitro";
    private string jump = "Jump";
    private string roll = "Roll";
	
	void Awake ()
	{
		// get the car controller
		car = GetComponent<CarController>();
        jumpScript = GetComponent<JumpScript>();
        nitroScript = GetComponent<NitroScript>();
	}
	
	void FixedUpdate()
	{
		// pass the input to the car!
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis(horizontal);
		float v = CrossPlatformInput.GetAxis(vertical);
        float r = CrossPlatformInput.GetAxis(roll);
#else
		float h = Input.GetAxis(horizontal);
		float v = Input.GetAxis(vertical);
#endif
        jumpScript.AirControl(h, v, r, Time.fixedDeltaTime);
		car.Move(h,v);
	}

    void Update()
    {
        bool fg = CrossPlatformInput.GetButtonDown(fire1);
        car.Fire(fg, ShellColors.Green);

        bool fr = CrossPlatformInput.GetButtonDown(fire2);
        car.Fire(fr, ShellColors.Red);

        bool fb = CrossPlatformInput.GetButtonDown(fire3);
        car.Fire(fb, ShellColors.Blue);

        bool isUsingNitro = CrossPlatformInput.GetButton(nitro);
        nitroScript.Accelerate(isUsingNitro);

        jumpScript.JumpButtonDown = CrossPlatformInput.GetButtonDown(jump);
        jumpScript.JumpButton = CrossPlatformInput.GetButton(jump);
    }
}
