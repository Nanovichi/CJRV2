using System.Collections;
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

    private bool isSpeedBoosted = false;
    private Coroutine speedBoostCoroutine;
    public float speedGrowthRate = 0.1f; // how much to grow per interval
    public float speedGrowthInterval = 10f; // how often (in seconds) to grow speed
    private float speedGrowthTimer;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        speedGrowthTimer = speedGrowthInterval;


        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.avoidancePriority = Random.Range(50, 100);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        string[] lanes = { "Walkable", "Walkable3", "Walkable2", "Walkable4" };
        string chosenLane = lanes[Random.Range(0, lanes.Length)];
        agent.areaMask = 1 << NavMesh.GetAreaFromName(chosenLane);

        Debug.Log(agent.areaMask);


        speedChangeTimer = speedChangeInterval;

        if (waypoints != null && waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }
    public void ApplySpeedBoost(float boostMultiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(boostMultiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostMultiplier, float duration)
    {
        isSpeedBoosted = true;
        float originalSpeed = agent.speed;

        agent.speed *= boostMultiplier;
      

        yield return new WaitForSeconds(duration);

       agent.speed = originalSpeed; 

        isSpeedBoosted = false;
        speedBoostCoroutine = null;
    }
    void Update()
    {
        if (waypoints.Count == 0) return;

        // Speed randomizer every few seconds
        speedChangeTimer -= Time.deltaTime;
        if (speedChangeTimer <= 0f && !isSpeedBoosted)
        {
            agent.speed = Random.Range(minSpeed, maxSpeed);
            speedChangeTimer = speedChangeInterval;
        }

        // Waypoint navigation
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < waypoints.Count)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
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
