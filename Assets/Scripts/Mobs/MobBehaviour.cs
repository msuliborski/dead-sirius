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
    public bool isRanged;

    public int ownerId;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update() {
        if (transform.position != target.position) {
            agent.SetDestination(target.position);
        }
        
//        if ()
        
    }
    
    void getClosestEnemyInRange() {
        GameObject[] mobs; 
        GameObject closestMob = null; 
        mobs = GameObject.FindGameObjectsWithTag("Mob"); 
        
        
        var radiusDistance = 50; // the range of distance
        var position = transform.position; 
        
//        // Iterate through them and find the objects within range 
//        foreach (var mob in mobs) {
//            
//            var diffToMob = (mob.transform.position - position);
//            if (closestMob != null)
//            var diffToClosestMob = (closestMob.transform.position - position);
//            var distanceToMob = diffToMob.sqrMagnitude; 
//            var distanceToClosestMob = diffToClosestMob.sqrMagnitude; 
//            if (distanceToMob < radiusDistance && distanceToMob < distanceToClosestMob) { 
//                cmob.GetComponent.<Renderer>().material.color = colorR; // effect on object
//                
//            } 
//        }
//
//        var go : GameObject in gos)  { 
//            var curDistance = diff.sqrMagnitude; 
//           
//        } 
     
    }
}