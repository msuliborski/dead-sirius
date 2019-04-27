using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobBehaviour : MonoBehaviour {
    private NavMeshAgent agent;
    public Transform target;

    public int health;
    public int healthCost;
    public int damage;
    public int attackSpeed;
    public int movingSpeed;
    public int spawnTime;
    public bool isRanged;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update() {
        if (transform.position != target.position) {
            agent.SetDestination(target.position);
        }
    }
}