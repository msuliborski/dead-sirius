using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MobBehaviourNodes : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform baseTarget;
    public List<GameObject> bases;
    public GameObject Owner;
    private const string ENEMY_ID_PREFIX = "Enemy ";
    public List<Transform> Nodes;
    public int nodeIndex = 0;
    public Quaternion targetRot;
    public int health;
    public int healthCost;
    public int healthReward;
    public int damage;
    public float attackSpeed;
    public float movingSpeed;
    public float rotatingSpeed;
    public float spawnTime;
    public int LaneIndex;
    public int ownerId;
    private bool isAttacking = false;
    public MobBehaviour FightEnemy;
    public GameObject towerAttackEffect;
    public int TypeIndex;


    public enum EnemyState { Fighting, Moving, Rotating, Waiting };
    public EnemyState CurrentState = EnemyState.Moving;
    public EnemyState PreviousState;
    public MobBehaviourNodes Enemy;
    public void PrintNodes()
    {
        foreach (var node in Nodes) Debug.Log(node);
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (ownerId == 1)
        {
            Owner = GameObject.Find("PlayerNode");
            baseTarget = GameObject.Find("Base2").transform;
        }
        else
        {
            Owner = GameObject.Find("EnemyNode");
            baseTarget = GameObject.Find("Base1").transform;

        }


        Vector3 dir = (Nodes[0].position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mob")
        {
            MobBehaviourNodes mb = other.GetComponent<MobBehaviourNodes>();
            if(mb.ownerId != ownerId)
            {
                PreviousState = CurrentState;
                CurrentState = EnemyState.Fighting;
                Enemy = mb;
            }
            
        }
        else if (other.tag == "MobTag")
        {
            PreviousState = CurrentState;
            CurrentState = EnemyState.Waiting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MobTag")
        { 
            MobBehaviourNodes mb = other.GetComponentInParent<MobBehaviourNodes>();
            if (mb.ownerId == ownerId)
            {
                CurrentState = PreviousState;
            }
            
        }

    }




    void Update()
    {


        //Debug.Log(CurrentState);
        switch (CurrentState)
        {
            case EnemyState.Moving:
                transform.position = Vector3.MoveTowards(transform.position, Nodes[nodeIndex].position, movingSpeed);
                if (transform.position == Nodes[nodeIndex].position)
                {
                    if (nodeIndex < Nodes.Count - 1)
                    {
                        Vector3 dir = (Nodes[nodeIndex + 1].position - transform.position).normalized;
                        targetRot = Quaternion.LookRotation(dir);
                        CurrentState = EnemyState.Rotating;
                    }
                    else
                    {
                        
                        //GameObject tae = Instantiate(towerAttackEffect);
                        //tae.transform.position = transform.position;
                        Destroy(gameObject);
                        //Destroy(tae, 2f);

                    }
                }
                break;

            case EnemyState.Rotating:
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotatingSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRot) <= 0.2f)
                {
                    CurrentState = EnemyState.Moving;
                    nodeIndex++;
                }
                break;

            case EnemyState.Fighting:

                Enemy.health -= Mathf.RoundToInt(damage * Time.deltaTime);
                if (Enemy.health < 0)
                {

                    StartCoroutine(KillEnemy());
                    CurrentState = PreviousState;
                }
                break;
        }

    }




        GameObject GetClosestEnemy(GameObject[] enemies)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject t in enemies)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist && t.GetComponent<MobBehaviourNodes>().ownerId != ownerId && LaneIndex == t.GetComponent<MobBehaviourNodes>().LaneIndex)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }

        public IEnumerator KillEnemy()
        {
            //Enemy.gameObject.SetActive(false);
            Enemy.GetComponentInChildren<MeshRenderer>().enabled = false;
            Enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * 100000);
            if (ownerId == 1)
            {
                Owner.GetComponent<PlayerControllsNodes>().health += Enemy.healthReward;
            }
            else
            {
                Owner.GetComponent<AINode>().health += Enemy.healthReward;
            }
            yield return new WaitForSeconds(2f);

            Destroy(Enemy.gameObject);
           

        }



    
}