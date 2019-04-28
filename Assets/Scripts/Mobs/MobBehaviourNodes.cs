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
    public int health;
    public int healthCost;
    public int healthReward;
    public int damage;
    public float attackSpeed;
    public float movingSpeed;
    public float spawnTime;
    public int LaneIndex;
    public int ownerId;
    private bool isAttacking = false;
    public MobBehaviour FightEnemy;


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
                    if (nodeIndex < Nodes.Count - 1) nodeIndex++;
                    else Destroy(gameObject);
                }
                break;

            case EnemyState.Rotating:

                //Quaternion destinationRotation = Vector3.Angle(transform.position, Nodes[nodeIndex + 1].position);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, destinationRotation, 80f * Time.deltaTime);

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