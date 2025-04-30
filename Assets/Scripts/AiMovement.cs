using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    public List<Transform> waypoints; // Assign waypoints in the Inspector
    public float stoppingDistance = 0.5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Randomize agent settings
        agent.speed = Random.Range(5f, 10f);
        agent.avoidancePriority = Random.Range(50, 100);
        agent.radius = 0.4f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        if (waypoints != null && waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        // Check if agent reached the current waypoint
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
