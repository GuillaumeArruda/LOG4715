using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour {

    //Saut multiple
    [SerializeField]
    float jumpDuration = 0.2f;				// Number of time the player can jump while in the air a value of -1 means infinite jumps
    [SerializeField]
    float jumpForceInTheAir = 500.0f;          // Force applied to the player when double jumping


    private bool isFalling;
    public bool JumpButtonDown { get; set; }
    public bool JumpButton { get; set; }
    private bool isJumping;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;
        Physics.Raycast(transform.position, -Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask);
        isFalling = groundCheckRaycast.distance > 0.10;
	    if(JumpButtonDown && !isJumping && !isFalling) {
            StartCoroutine("JumpCoroutine");
            isJumping = true;
        }
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
