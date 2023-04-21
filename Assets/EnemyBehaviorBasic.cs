using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviorSight : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float wanderDuration = 5f;

    public Transform target;
    private Vector3 initialPosition;
    private Vector3 destination;
    private float wanderDelay;

    private NavMeshAgent agent;
    private EnemySight enemySight;

    void Start()
    {
        initialPosition = transform.position;
        destination = initialPosition;
        agent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
    }

    void Update()
    {
        if (!enemySight.canSeePlayer)
        {
            Wander();
        }
        else
        {
            Pursue();
        }
    }

    void Wander()
    {
        wanderDelay += Time.deltaTime;

        if (Vector3.Distance(transform.position, destination) <= 1f || wanderDelay >= wanderDuration)
        {
            destination = GetNewWanderDestination();
            wanderDelay = 0f;
        }

        agent.speed = walkSpeed;
        agent.destination = destination;
    }

    void Pursue()
    {
        agent.speed = runSpeed;
        agent.destination = target.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }

    Vector3 GetNewWanderDestination()
    {
        Vector3 newDestination = Random.insideUnitSphere * wanderRadius;
        newDestination += initialPosition;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(newDestination, out navMeshHit, wanderRadius, NavMesh.AllAreas);
        return navMeshHit.position;
    }
}