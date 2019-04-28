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


    public enum EnemyState { Fighting, Moving, Rotating };
    public EnemyState CurrentState = EnemyState.Moving;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (ownerId == 1)
        {
            Owner = GameObject.Find("Player");
            baseTarget = GameObject.Find("Base2").transform;
        }
        else
        {
            Owner = GameObject.Find("Enemy");
            baseTarget = GameObject.Find("Base1").transform;

        }


        Vector3 dir = (Nodes[0].position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

    }





    void Update()
    {


        Debug.Log(CurrentState);
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
                if (dist < minDist && t.GetComponent<MobBehaviour>().ownerId != ownerId && LaneIndex == t.GetComponent<MobBehaviour>().LaneIndex)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }





    
}