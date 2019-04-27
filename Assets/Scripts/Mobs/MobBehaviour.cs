using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MobBehaviour : NetworkBehaviour {
    private NavMeshAgent agent;
    public Transform target;


    private const string ENEMY_ID_PREFIX = "Enemy ";

    [SyncVar] public int health;
    public int healthCost;
    public int healthReward;
    public int damage;
    public float attackSpeed;
    public float movingSpeed;
    public float spawnTime;
    public float attackRange;

    public int ownerId;

    private bool isAttacking = false;

    void Start() {
        if (isServer)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }


    void Update() {

        if (isServer)
        {
            GameObject targetMob = getClosestEnemyInRange();
            GameObject lockTarget = targetMob;

            if (health <= 0) Destroy(gameObject);

            if (targetMob != null || lockTarget != null)
            {
                if ((lockTarget.transform.position - transform.position).sqrMagnitude < attackRange)
                {
                    agent.enabled = false;
                    if (!isAttacking) StartCoroutine(attack(lockTarget));
                    Debug.Log("chuj");
                }
                else
                {
                    agent.enabled = true;
                    agent.SetDestination(lockTarget.transform.position);
                }

            }
            else
            {
                if (transform.position != target.position)
                {
                    agent.enabled = true;
                    agent.SetDestination(target.position);
                }
            }

            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
        }
        
        
    }

    IEnumerator attack(GameObject target) {
        isAttacking = true;
        yield return new WaitForSeconds(attackSpeed);
        //animacja
        target.GetComponent<MobBehaviour>().health -= damage;
        isAttacking = false;
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