using UnityEngine;
using System.Collections;

public class NitroScript : MonoBehaviour {

    [SerializeField]
    private float recuperationRate = 10.0f;
    [SerializeField]
    private float nitroAccelerationBonus = 20.0f;
    [SerializeField]
    private float expenditureRate = 20.0f;
    [SerializeField]
    private float nitroMaxCapacity = 100.0f;
    
    [SerializeField]
    private Texture nitroEmptyBar;
    [SerializeField]
    private Texture nitroFullBar;
    
    private float nitroRemaining;
    private bool isUsingNitro = false;
    private Rigidbody car;
    private float nitroBarWidth;
    private float nitroBarHeight;

    // Use this for initialization
	void Start () {
        car = GetComponent<Rigidbody>();
        nitroBarWidth = Screen.width / 5;
        nitroBarHeight = Screen.height / 10;
	}

    void FixedUpdate() {
        if (isUsingNitro && nitroRemaining > 0)
        {
            car.AddForce(car.rotation * new Vector3(0, 0, nitroAccelerationBonus));
            nitroRemaining -= expenditureRate * Time.fixedDeltaTime;
        }
        else
        {
            nitroRemaining = nitroRemaining + recuperationRate * Time.fixedDeltaTime;
            if(nitroRemaining > nitroMaxCapacity)
            {
                nitroRemaining = nitroMaxCapacity;
            }
        }
    }

    public void Accelerate(bool nitro)
    {
        isUsingNitro = nitro;
    }
    public void FillNitro()
    {
        nitroRemaining = nitroMaxCapacity;
    }
    public void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width - nitroBarWidth/2) / 4, (Screen.height - nitroBarHeight + 20), nitroBarWidth, nitroBarHeight), nitroEmptyBar);
        GUI.DrawTexture(new Rect((Screen.width - nitroBarWidth/2) / 4, (Screen.height - nitroBarHeight + 20), (nitroRemaining / nitroMaxCapacity) * nitroBarWidth, nitroBarHeight), nitroFullBar);
    }
}
