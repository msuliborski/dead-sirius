using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobBehaviour : MonoBehaviour {
    private NavMeshAgent agent;
    public Transform target;

    public int health;
    public int healthCost;
    public int healthReward;
    public int damage;
    public int attackSpeed;
    public int movingSpeed;
    public int spawnTime;
    public bool atackRange;

    public int ownerId;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update() {

        GameObject targetMob = getClosestEnemyInRange();
        GameObject lockTarget = targetMob;
        
//        if ()
        
        if (targetMob != null || lockTarget != null) {
            //wwwweqwewqqqweqweqweqwelockTarget = targetMob;
            agent.SetDestination(lockTarget.transform.position);
        }
        else {
            if (transform.position != target.position) {
                agent.SetDestination(target.position);
            }
        }
        
    }
    
    GameObject getClosestEnemyInRange() {
        GameObject[] mobs; 
        GameObject closestMob = null; 
        mobs = GameObject.FindGameObjectsWithTag("Mob"); 
        
        
        var radiusDistance = 200; // the range of distance
        var position = transform.position; 
        
        // Iterate through them and find the objects within range 
        foreach (var mob in mobs) {
            //Debug.Log(mobs.Length);
            Vector3 diffToMob = mob.transform.position - position;
            Vector3 diffToClosestMob = new Vector3(9999, 9999, 9999) - position;
            if (closestMob != null) diffToClosestMob = closestMob.transform.position - position;
            
            var distanceToMob = diffToMob.sqrMagnitude; 
            var distanceToClosestMob = diffToClosestMob.sqrMagnitude; 
            
            if (distanceToMob < radiusDistance && distanceToMob < distanceToClosestMob && mob.GetComponent<MobBehaviour>().ownerId != ownerId) {
                closestMob = mob;
            } 
        }
        if (closestMob != null) Debug.Log("length to closest: " + (closestMob.transform.position - position).sqrMagnitude);
        return closestMob;



    }
}