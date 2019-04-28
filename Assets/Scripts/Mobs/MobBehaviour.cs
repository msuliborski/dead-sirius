using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MobBehaviour : MonoBehaviour {
    private NavMeshAgent agent;

    public Transform baseTarget;
    public List<GameObject> bases;
    public GameObject Owner;
    private const string ENEMY_ID_PREFIX = "Enemy ";

    public int health;
    public int healthCost;
    public int healthReward;
    public int damage;
    public float attackSpeed;
    public float movingSpeed;
    public float spawnTime;
    public float attackRange;
    public int LaneIndex;
    public int ownerId;
    private bool isAttacking = false;
    public MobBehaviour FightEnemy;


    public enum EnemyState { Fighting, Moving };
    public EnemyState CurrentState = EnemyState.Moving;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        if (ownerId == 1) {
            Owner = GameObject.Find("Player");
            baseTarget = GameObject.Find("Base2").transform;
        }
        else {
            Owner = GameObject.Find("Enemy");
            baseTarget = GameObject.Find("Base1").transform;

        }
        

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mob" && FightEnemy == null)
        {

            MobBehaviour otherMb = GetComponent<MobBehaviour>();
            if (otherMb.ownerId != ownerId)
            {
                //otherMb.agent.enabled = false;
                //otherMb.CurrentState = EnemyState.Fighting;
                //otherMb.FightEnemy = this;
                FightEnemy = otherMb;
                CurrentState = EnemyState.Fighting;
                agent.enabled = false;
            }
        }
    }



    void Update() {

        if (health < 0) Destroy(gameObject);
        Debug.Log(CurrentState);
        switch (CurrentState)
        {
            case EnemyState.Moving:
                GameObject targetMob = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Mob"));
                if (targetMob != null) agent.SetDestination(targetMob.transform.position);
                else agent.SetDestination(baseTarget.transform.position);
                break;

            case EnemyState.Fighting:
                if (FightEnemy == null)
                {
                    CurrentState = EnemyState.Moving;
                    agent.enabled = true;

                }
                FightEnemy.health -= Mathf.RoundToInt(attackSpeed * damage * Time.deltaTime);
               

                break;
        }
       



        /*GameObject lockTarget = targetMob;

            if (health <= 0) Destroy(gameObject);

            if (targetMob != null || lockTarget != null)
            {
                if ((lockTarget.transform.position - transform.position).sqrMagnitude < attackRange)
                {
                    agent.enabled = false;
                    if (!isAttacking) StartCoroutine(attack(lockTarget));
                }
                else
                {
                    agent.enabled = true;
                    agent.SetDestination(lockTarget.transform.position);
                }

            }
            else
            {
                if ((baseTarget.transform.position - transform.position).sqrMagnitude < attackRange - baseTarget.localScale.x)
                {
                    agent.enabled = false;
                    if (!isAttacking) StartCoroutine(attack(baseTarget.gameObject));
                } else {
                    agent.enabled = true;
                    agent.SetDestination(baseTarget.position);
                }
            }

            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }*/
        
        
        
    }

    /*IEnumerator attack(GameObject target) {
        isAttacking = true;
        yield return new WaitForSeconds(attackSpeed);
        //animacja
        target.GetComponent<MobBehaviour>().health -= damage;
        isAttacking = false;
    }*/


    GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && t.GetComponent<MobBehaviour>().ownerId != ownerId && LaneIndex == t.GetComponent<MobBehaviour>().LaneIndex)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }




    /*GameObject getClosestEnemyInRange() {
        Debug.Log("aaaa");
        GameObject[] mobs; 
        GameObject closestMob = null; 
        
        
        
        var radiusDistance = 2000; // the range of distance
        var position = transform.position; 
        
        // Iterate through them and find the objects within range 
        foreach (var mob in mobs) {
            //Debug.Log(mobs.Length);
            Vector3 diffToMob = mob.transform.position - position;
            Vector3 diffToClosestMob = new Vector3(9999, 9999, 9999) - position;
            if (closestMob != null) diffToClosestMob = closestMob.transform.position - position;
            
            var distanceToMob = diffToMob.sqrMagnitude; 
            var distanceToClosestMob = diffToClosestMob.sqrMagnitude; 
            


            //if (distanceToMob < radiusDistance && distanceToMob < distanceToClosestMob && ) {
             //   closestMob = mob;
            //} 
        }
        if (closestMob != null) Debug.Log("length to closest: " + (closestMob.transform.position - position).sqrMagnitude);
        return closestMob;



    }*/
}