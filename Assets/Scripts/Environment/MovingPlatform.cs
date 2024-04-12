using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MovingPlatformState
{
    Moving,
    Waiting
}

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject waypointHolder;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float waitTime = 1f;
    private int currentWaypointIndex = 0;
    private Vector3 currentTarget;
    private MovingPlatformState state = MovingPlatformState.Waiting;
    private List<Transform> waypoints = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        if (waypointHolder == null)
        {
            Debug.LogError("Waypoint holder not found!");
            state = MovingPlatformState.Waiting;
            return;
        }
        waypoints = waypointHolder.GetComponentsInChildren<Transform>().ToList();

        // remove waypoint holder transform (will be the first)
        waypoints.RemoveAt(0);

        currentTarget = waypoints[currentWaypointIndex].position;   

        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f && state == MovingPlatformState.Moving)
        {
            StartCoroutine(Wait());
        }
        else
        {
            Move();
        }        
    }

    public IEnumerator Wait()
    {
        state = MovingPlatformState.Waiting;
        yield return new WaitForSeconds(waitTime);
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = 0;
        }
        currentTarget = waypoints[currentWaypointIndex].position;
        state = MovingPlatformState.Moving;
    }

    private void Move()
    {
        if (state == MovingPlatformState.Moving)
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }
}
