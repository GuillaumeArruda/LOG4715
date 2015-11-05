using UnityEngine;

class ShellMovementComponent : MonoBehaviour
{
    [SerializeField] private float m_shellVelocity = 60;
    [SerializeField] private float m_rotationSpeed = 90;
    [SerializeField] private int   m_maxNumberOfBounces = 1;
    private int m_currentNumberOfBounces = 0;
    const int m_vehiclesLayer = 8;
    Vector3 m_rotationAxis = Vector3.zero;

    void Update()
    {
        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;

        // Raycast below the projectile to follow the ground
        if(Physics.Raycast(transform.position, -Vector3.up, out groundCheckRaycast, Mathf.Infinity, layerMask))
        {
            transform.position -= Vector3.up * Mathf.Min((groundCheckRaycast.distance - transform.collider.bounds.extents.y), 0.05f);
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

        // Give a slight turn to the shell if it has a target
        if(Target != null)
        {
            Vector3 distanceToTarget = Vector3.Normalize(Target.transform.position - transform.position);
            Vector3 projectileDirection = transform.forward;

            // Apply rotation while vectors are not parallel
            if(Vector3.Dot(distanceToTarget, projectileDirection) < distanceToTarget.magnitude * projectileDirection.magnitude)
            {
                if(m_rotationAxis == Vector3.zero)
                {
                    m_rotationAxis = Vector3.Normalize(Vector3.Cross(projectileDirection, distanceToTarget));
                }

                print("Turning to " + Target.name);
                rigidbody.velocity = Quaternion.AngleAxis(m_rotationSpeed * Time.deltaTime, m_rotationAxis) * rigidbody.velocity;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody shell = GetComponent<Rigidbody>();

        if(collision.gameObject.name == "Track")
        {
            return;
        }

        // Destroy projectile on collision with a vehicle
        if((collision.gameObject.layer & m_vehiclesLayer) > 0)
        {
            Destroy(gameObject);
            return;
        }

        foreach(ContactPoint contact in collision.contacts)
        {
            transform.forward = transform.forward - 2 * Vector3.Dot(transform.forward, contact.normal) * contact.normal;
            transform.forward = Vector3.Normalize(new Vector3(transform.forward.x, 0.0f, transform.forward.z));
            shell.velocity = transform.forward * m_shellVelocity;

            if(++m_currentNumberOfBounces > m_maxNumberOfBounces)
            {
                Destroy(gameObject);
            }

            return;
        }
    }

    public float GetMaxShellVelocity {
        get {
            return m_shellVelocity;
        }
    }

    public GameObject Target
    {
        get;
        set;
    }
}