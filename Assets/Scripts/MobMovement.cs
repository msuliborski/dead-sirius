using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobMovement : MonoBehaviour
{
    
    private NavMeshAgent agent;
    public Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        if (transform.position != target.position)
        {
            agent.SetDestination(target.position);
        }
    }
}
