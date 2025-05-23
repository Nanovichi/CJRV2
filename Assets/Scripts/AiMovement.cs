using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public float stoppingDistance = 0.5f;

    public float speedChangeInterval = 3f;
    private float speedChangeTimer;
    private float minSpeed = 3f;
    private float maxSpeed = 8f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.avoidancePriority = Random.Range(50, 100);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        speedChangeTimer = speedChangeInterval;

        if (waypoints != null && waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        speedChangeTimer -= Time.deltaTime;
        if (speedChangeTimer <= 0f)
        {
            agent.speed = Random.Range(minSpeed, maxSpeed);
            speedChangeTimer = speedChangeInterval;
        }

        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex < waypoints.Count)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }
}
