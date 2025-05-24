using rayzngames;
using UnityEngine;
using UnityEngine.AI;

public class AIBicycleController : MonoBehaviour
{
    public NavMeshAgent agent;  // NavMeshAgent on the same GameObject or child
    public Transform target;


    public float speedChangeInterval = 3f;
    private float speedChangeTimer;
    private float minSpeed = 3f;
    private float maxSpeed = 8f;// Destination target

    public float reachThreshold = 1.5f;


    public float speedGrowthRate = 0.1f; // how much to grow per interval
    public float speedGrowthInterval = 10f; // how often (in seconds) to grow speed
    private float speedGrowthTimer;// How close is "close enough" to target

    void Start()
    {

        speedGrowthTimer = speedGrowthInterval;


        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.avoidancePriority = Random.Range(50, 100);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = true;  // Let agent move transform
        agent.updateRotation = true;


        string[] lanes = { "Walkable", "Walkable3", "Walkable2", "Walkable4" };
        string chosenLane = lanes[Random.Range(0, lanes.Length)];
        agent.areaMask = 1 << NavMesh.GetAreaFromName(chosenLane);

        Debug.Log(agent.areaMask);


        if (target != null)
            agent.SetDestination(target.position);
    }

    void Update()
    {
        if (target == null)
            return;
        speedChangeTimer -= Time.deltaTime;
        if (speedChangeTimer <= 0f)
        {
            agent.speed = Random.Range(minSpeed, maxSpeed);
            speedChangeTimer = speedChangeInterval;
        }

        // Update destination in case target moves
        agent.SetDestination(target.position);

        // Optionally, check if agent reached destination
        if (!agent.pathPending && agent.remainingDistance <= reachThreshold)
        {
            // Reached target - you can stop or do something
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        // Speed scaling over time
        speedGrowthTimer -= Time.deltaTime;
        if (speedGrowthTimer <= 0f)
        {
            minSpeed += speedGrowthRate;
            maxSpeed += speedGrowthRate;
            speedGrowthTimer = speedGrowthInterval;

            // Optionally clamp to prevent it from getting too fast
            minSpeed = Mathf.Min(minSpeed, 15f);
            maxSpeed = Mathf.Min(maxSpeed, 20f);
        }
    }
}
