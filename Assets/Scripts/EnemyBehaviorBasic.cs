using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviorSight : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float wanderDuration = 5f; 
    public float persistence = 10f;
    public float orbitDistance = 3f;
    public float health = 100f;
    private float maxHealth = 0;
    public float panickNum = 20f;

    bool panicked = false;
    public Transform target;
    private Vector3 initialPosition;
    private Vector3 destination;
    private Vector3 searchPoint;
    private float wanderDelay;
    private float persistenceTimer = 0f;

    private NavMeshAgent agent;
    private EnemySight enemySight;

    int randomNumber = 0;

    void Start()
    {
        maxHealth = health;
        initialPosition = transform.position;
        destination = initialPosition;
        agent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
    }

    void Update()
    {
        if(health <= panickNum && panicked == false){
            randomNumber = Random.Range(1, 6);
            if(randomNumber == 1){
                Panick();
                randomNumber = 0;
            }
        }
        if (enemySight.canSeePlayer)
        {
            persistence = 3;
            Pursue();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        //This loop discerns if we just saw the enemy and have started looking for him.
        Debug.Log("Wander Mode.");
        if(persistence > 0)
        {
            Debug.Log("We are searching!");
            // Enemy should choose a random location within a short distance around searchPoint
            if (Vector3.Distance(transform.position, destination) <= 1f)
            {
                destination = GetNewSearchDestination();
                wanderDelay = 0f;
            }
            agent.speed = walkSpeed;
            agent.destination = destination;
            StartCoroutine(WaitForWanderDuration());
        }
        else
        {
        wanderDelay += Time.deltaTime;
        if (Vector3.Distance(transform.position, destination) <= 1f)
        {
            destination = GetNewWanderDestination();
            wanderDelay = 0f;
        }
        agent.speed = walkSpeed;
        StartCoroutine(WaitForWanderDuration());
        agent.destination = destination;
        }
    }

    IEnumerator WaitForWanderDuration()
    {
        yield return new WaitForSeconds(wanderDuration);
    }


    IEnumerator Search()
    {
        //Debug.Log("We are searching!");
        Debug.Log(persistence + " Hello!");
        searchPoint = target.transform.position;
        //Enemy should choose a random location within a short distance around searchPoint
        yield return StartCoroutine(SearchDelay());
        persistence--;
    }

    IEnumerator SearchDelay()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(5);
    }

    void Pursue()
    {
        agent.speed = runSpeed;
        searchPoint = target.transform.position;
        if (Time.time % 5f == 0f) 
        {
            randomNumber = Random.Range(1, 6);
        }
        // Get the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= orbitDistance)
        {
            /*Vector3 orbitDirection = (transform.position - target.position).normalized;
            Quaternion orbitRotation = Quaternion.AngleAxis(Time.time * walkSpeed, Vector3.up);
            Vector3 orbitPosition = target.position + orbitRotation * orbitDirection * orbitDistance;

            agent.destination = orbitPosition;

            float sinValue = Mathf.Sin(Time.time * walkSpeed);
            Vector3 offset = Vector3.right * sinValue;
            agent.Move(offset);*/
            agent.speed = 0f;
            if (randomNumber == 5)
            {
                MeleeAttack();
                return;
            }
            agent.speed = 0f;
        }
        else
        {
            agent.destination = target.position;
        }
    }

        void MeleeAttack()
    {
        agent.speed = runSpeed;
        agent.destination = target.position;

        if (Vector3.Distance(transform.position, target.position) <= 2f) // If we hit the player, return to Pursue()
        {
            Pursue();
        }
        randomNumber = 0;
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
            persistenceTimer = 0f;
        }
    }

    Vector3 GetNewWanderDestination()
    {
        Vector3 newDestination = Random.insideUnitSphere * (wanderRadius/2);
        newDestination += initialPosition;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(newDestination, out navMeshHit, wanderRadius, NavMesh.AllAreas);
        return navMeshHit.position;
    }

    Vector3 GetNewSearchDestination()
    {
        Vector3 newDestination = Random.insideUnitSphere * wanderRadius;
        newDestination += searchPoint;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(newDestination, out navMeshHit, wanderRadius, NavMesh.AllAreas);
        return navMeshHit.position;
    }

    private bool isPanicking = false;
    private float panicTimer = 0f;
    private float panicDuration = 10f;
    private float panicDirectionChangeInterval = 2f;
    private float panicStopDuration = 5f;
    private float panicHealthRegenRate = 2.5f;
    private Vector3 panicDestination;
    private float originalSpeed;

    void Panick()
    {
        // Set the panic destination as the opposite direction from the player's position
        panicDestination = transform.position - (target.position - transform.position);

        // Start the panic behavior
        isPanicking = true;
        originalSpeed = agent.speed;
        agent.speed = runSpeed;

        StartCoroutine(PanicRoutine());
    }

    IEnumerator PanicRoutine()
    {
        while (isPanicking)
        {
            // Check if the player is too close or health has decreased
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= 10f || health < maxHealth)
            {
                // Stop panicking and return to normal behavior
                StopPanic();
                yield break;
            }

            // Flee from the player's position
            agent.destination = panicDestination;

            // Randomly change the panic direction every panicDirectionChangeInterval seconds
            if (panicTimer % panicDirectionChangeInterval == 0f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 5f;
                randomDirection.y = 0f;
                panicDestination = transform.position - randomDirection;
            }

            // Increase health by panicHealthRegenRate every second
            health += panicHealthRegenRate * Time.deltaTime;

            // Check if the panic duration has ended
            if (panicTimer >= panicDuration)
            {
                // Stop panicking and hold still for panicStopDuration seconds
                agent.speed = 0f;
                yield return new WaitForSeconds(panicStopDuration);
                StopPanic();
                yield break;
            }

            panicTimer += Time.deltaTime;
            yield return null;
        }
    }

    void StopPanic()
    {
        isPanicking = false;
        agent.speed = originalSpeed;
        panicTimer = 0f;
    }
}