using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour {

    //Saut multiple
    [SerializeField]
    float jumpDuration = 0.2f;				// Number of time the player can jump while in the air a value of -1 means infinite jumps
    [SerializeField]
    float jumpForceInTheAir = 500.0f;          // Force applied to the player when double jumping

    [SerializeField]
    float airControlFowardAxis = 120.0f;
    [SerializeField]
    float airControlRightAxis = 120.0f;
    [SerializeField]
    float airControlUpAxis = 120.0f;

    [SerializeField] float minJumpHeightForScore = 5.0f;
    [SerializeField] float scorePerSecondsForHighJump = 100f;
    [SerializeField] float scorePerSecondsForAirControl = 100f;

    private Transform car;

    private bool isFalling;
    private bool isInTheAir;
    public bool JumpButtonDown { get; set; }
    public bool JumpButton { get; set; }
    public float Score { get; set; }
    private bool isJumping;
    private const int vehiclesLayer = 8;
    public GUIText scoreText;

	// Use this for initialization
	void Start () {
        car = GetComponent<Transform>();
        Score = 0;
	}
	
    public void AirControl(float h, float v, float r, float time){
        if (isInTheAir)
        {
            car.Rotate(Vector3.right, v * airControlRightAxis * Time.fixedDeltaTime);
            car.Rotate(Vector3.up, h * airControlUpAxis * Time.fixedDeltaTime);
            car.Rotate(Vector3.forward, -r * airControlFowardAxis * Time.fixedDeltaTime);

            if((r != 0.0f) || (h != 0.0f) || (v != 0.0f))
            {
                Score += time * scorePerSecondsForAirControl;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;
        Physics.Raycast(transform.position, -Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask);
        isFalling = groundCheckRaycast.distance > 0.10;
        isInTheAir = groundCheckRaycast.distance > 0.70;
	    if(JumpButtonDown && !isJumping && !isFalling) {
            StartCoroutine("JumpCoroutine");
            isJumping = true;
        }
        
        if(isInTheAir)
        {
            // Make it so if we jump high enough we get more score
            RaycastHit jumpHeightRaycast;
            Physics.Raycast(transform.position, -Vector3.up, out jumpHeightRaycast, Mathf.Infinity, layerMask);
            if(jumpHeightRaycast.distance > minJumpHeightForScore)
            {
                Score += scorePerSecondsForHighJump * Time.deltaTime; 
            }
        }

        // Update score
        scoreText.text = "Score : " + ((int)Score).ToString("D6");
	}

    IEnumerator JumpCoroutine(){
        float timer = 0;
        while (JumpButton && timer <= jumpDuration) {
            float proportionCompleted = timer / jumpDuration;
            Vector3 jumpVector = Vector3.Lerp(new Vector3(0, jumpForceInTheAir, 0), Vector3.zero, proportionCompleted);
            GetComponent<Rigidbody>().AddForce(jumpVector);
            timer += Time.fixedDeltaTime;
            Debug.Log(timer);
            yield return new WaitForFixedUpdate();
        }
        isJumping = false;
    }
}
