using UnityEngine;

class ShellMovementComponent : MonoBehaviour
{
    [SerializeField] public float shellVelocity = 60;

    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime * shellVelocity;

        // Drop the shell to the ground
        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;

        // Raycast below the projectile to follow the ground
        if(Physics.Raycast(transform.position, -Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask))
        {
            transform.position -= Vector3.up * (groundCheckRaycast.distance - transform.collider.bounds.extents.y);
        }
        else if(Physics.Raycast(transform.position, Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask))
        {
            transform.position += Vector3.up * (groundCheckRaycast.distance + transform.collider.bounds.extents.y);
        }

        // Raycast infront of the projectile to climb steep hills
        float radiusOfProjectile = GetComponent<SphereCollider>().radius;
        if(Physics.Raycast(transform.position, Vector3.forward, out groundCheckRaycast, radiusOfProjectile, layerMask))
        {
            transform.position -= Vector3.forward * (groundCheckRaycast.distance - transform.collider.bounds.extents.x);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        Rigidbody shell = GetComponent<Rigidbody>();

        if(collision.gameObject.name == "Track")
        {
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