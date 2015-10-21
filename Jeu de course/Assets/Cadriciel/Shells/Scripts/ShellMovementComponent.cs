using UnityEngine;

class ShellMovementComponent : MonoBehaviour
{
    [SerializeField] public float shellVelocity = 60;
    private float fallAcceleration = 30;
    private float fallSpeed = 0.0f;

    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime * shellVelocity;

        // Drop the shell to the ground
        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;
        if(Physics.Raycast(transform.position, -Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask))
        {
            transform.position -= Vector3.up * (groundCheckRaycast.distance - transform.collider.bounds.extents.y);
            /*
            if((groundCheckRaycast.distance - transform.collider.bounds.extents.y) < 0)
            {
                //fallSpeed = 0.0f;
                print("GOTEM");
            }
            /*
            /*
            else
            {
                transform.position -= Vector3.up * Mathf.Min(fallSpeed * Time.deltaTime, groundCheckRaycast.distance - transform.collider.bounds.extents.y);
                fallSpeed += fallAcceleration * Time.deltaTime;
            }
            */
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        Rigidbody shell = GetComponent<Rigidbody>();

        if(collision.gameObject.name == "Track")
        {
            //shell.velocity = shell.velocity.normalized * shellVelocity;
            return;
        }

        foreach(ContactPoint contact in collision.contacts)
        {
            transform.forward = transform.forward - 2 * Vector3.Dot(transform.forward, contact.normal) * contact.normal;
            transform.forward = Vector3.Normalize(new Vector3(transform.forward.x, 0.0f, transform.forward.z));
            shell.velocity = transform.forward * shellVelocity;
            return;
        }
    }

    public float GetMaxShellVelocity {
        get {
            return shellVelocity;
        }
    }
}