using UnityEngine;

class ShellMovementComponent : MonoBehaviour
{
    [SerializeField] private float m_shellVelocity = 60;
    [SerializeField] private float m_rotationSpeed = 90;
    [SerializeField] private int   m_maxNumberOfBounces = 1;
    [SerializeField] private float m_maxTimeAlive = 10.0f;
    public ParticleSystem destroyParticleSystem;
    private int m_currentNumberOfBounces = 0;
    private int m_currentWaypoint = int.MaxValue;
    private float m_timeAlive = 0;
    private bool m_delayedDestroy = false;
    private float m_timeSpentInDestroy = 0.0f;
    private float m_destroyTimer = 2.0f;
    const int m_vehiclesLayer = 8;
    Vector3 m_rotationAxis = Vector3.zero;

    void Update()
    {
        if(m_delayedDestroy && m_timeSpentInDestroy < m_destroyTimer)
        {
            print("Waiting");
            m_timeSpentInDestroy += Time.deltaTime;
        }
        else if(m_delayedDestroy && m_timeSpentInDestroy >= m_destroyTimer)
        {
            print("Destroyed");
            Destroy(gameObject);
        }

        RaycastHit groundCheckRaycast;
        int layerMask = 1 << 11;

        m_timeAlive += Time.deltaTime;
        if(m_timeAlive > m_maxTimeAlive)
        {
            DelayedDestroy();
            return;
        }

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

        // Give a slight turn to the shell if it has a target (Red shell)
        if(Target != null && Color == ShellColors.Red)
        {
            Vector3 distanceToTarget = Vector3.Normalize(Target.transform.position - transform.position);
            Vector3 projectileDirection = Vector3.Normalize(transform.forward);
            float rotationScaleFactor = 1.5f - Vector3.Dot(distanceToTarget, projectileDirection);

            // Apply rotation while vectors are not parallel
            if(Vector3.Dot(distanceToTarget, projectileDirection) < distanceToTarget.magnitude * projectileDirection.magnitude)
            {
                if(m_rotationAxis == Vector3.zero)
                {
                    m_rotationAxis = Vector3.Normalize(Vector3.Cross(projectileDirection, distanceToTarget));
                }

                rigidbody.velocity = Quaternion.AngleAxis(rotationScaleFactor * m_rotationSpeed * Time.deltaTime, m_rotationAxis) * rigidbody.velocity;
            }
        }

        // Follow a target if there's one selected and there's no rotation speed
        if(Target != null && Color == ShellColors.Blue)
        {
            // Check the list of waypoint and target. If target is closer than nearest waypoint go for the 
            // target else, keep following the waypoints
            Transform[] waypoints = Target.GetComponent<WaypointProgressTracker>().circuit.Waypoints;

            if(m_currentWaypoint == int.MaxValue)
            {
                float distanceToClosestWaypoint = float.MaxValue;
                for(int i = 0; i < waypoints.Length; ++i)
                {
                    Vector3 distanceToWaypoint = waypoints[i].position - transform.position;
                    float dot = Vector3.Dot(distanceToWaypoint, transform.forward);
                    if(Vector3.Magnitude(distanceToWaypoint) < distanceToClosestWaypoint && dot > 0)
                    {
                        distanceToClosestWaypoint = Vector3.Magnitude(distanceToWaypoint);
                        m_currentWaypoint = i;
                    }
                }
            }

            if(Vector3.Magnitude(waypoints[m_currentWaypoint].position - transform.position) < 3.0)
            {
                if(++m_currentWaypoint >= waypoints.Length)
                {
                    m_currentWaypoint = 0;
                }
            }

            float distanceToTarget = Vector3.Magnitude(Target.transform.position - transform.position);
            float distanceToCurrentWaypoint = Vector3.Magnitude(waypoints[m_currentWaypoint].position - transform.position);

            if(distanceToTarget < 20.0)
            {
                // Go for the target
                rigidbody.velocity = Vector3.Magnitude(rigidbody.velocity) * Vector3.Normalize(Target.transform.position - transform.position);
            }
            else
            {
                // Go for the waypoint
                rigidbody.velocity = Vector3.Magnitude(rigidbody.velocity) * Vector3.Normalize(waypoints[m_currentWaypoint].position - transform.position);
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
        if((collision.gameObject.layer & m_vehiclesLayer) > 0 && Color != ShellColors.Blue)
        {
            DelayedDestroy();
            return;
        }

        if(Color == ShellColors.Blue && collision.gameObject == Target)
        {
            DelayedDestroy();
            return;
        }

        foreach(ContactPoint contact in collision.contacts)
        {
            transform.forward = transform.forward - 2 * Vector3.Dot(transform.forward, contact.normal) * contact.normal;
            transform.forward = Vector3.Normalize(new Vector3(transform.forward.x, 0.0f, transform.forward.z));
            transform.position += transform.forward * 2.0f;
            shell.velocity = transform.forward * m_shellVelocity;

            if(++m_currentNumberOfBounces > m_maxNumberOfBounces)
            {
                DelayedDestroy();
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

    public ShellColors Color
    {
        get;
        set;
    }

    void DelayedDestroy()
    {
        if(m_delayedDestroy)
        {
            return;
        }

        m_delayedDestroy = true;
        renderer.enabled = false;
        rigidbody.active = false;
        collider.enabled = false;

        ParticleSystem explosion  = Instantiate(destroyParticleSystem, transform.position, transform.rotation) as ParticleSystem;
        explosion.enableEmission = true;
    }
}