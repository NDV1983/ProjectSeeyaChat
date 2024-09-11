
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CatService : MonoBehaviour
{
    public Transform[] waypoints;  // Array of waypoints
    private NavMeshAgent agent;
    public float waypointTolerance = 2f;  // Distance to consider reaching a waypoint
    public float waitTime = 2f;  // Time to wait at each waypoint

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            SetRandomDestination();
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            StartCoroutine(WaitAndSetRandomDestination());
        }
    }

    private IEnumerator WaitAndSetRandomDestination()
    {
        yield return new WaitForSeconds(waitTime);
        SetRandomDestination();
    }

    private void SetRandomDestination()
    {
        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }
}