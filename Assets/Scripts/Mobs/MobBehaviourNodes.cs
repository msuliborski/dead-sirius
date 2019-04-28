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
    private Animator _animator;


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
        _animator = GetComponent<Animator>();
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
        else if (other.CompareTag("MobTag"))
        {
            MobBehaviourNodes mb = other.GetComponent<MobBehaviourNodes>();
            if (mb.ownerId == ownerId) {
                _animator.SetBool("waiting", true);
                PreviousState = CurrentState;
                CurrentState = EnemyState.Waiting;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MobTag"))
        { 
            _animator.SetBool("waiting", false);
            MobBehaviourNodes mb = other.GetComponentInParent<MobBehaviourNodes>();
            if (mb.ownerId == ownerId)
            {
                CurrentState = PreviousState;
            }
            
        }
//        else if (other.CompareTag("Spawn") && ownerId == 1 && Owner.GetComponent<PlayerControllsNodes>().flagCount == LaneIndex) {
//            Owner.GetComponent<PlayerControllsNodes>().Blocked = false;
//        }

    }




    void Update()
    {


        //Debug.Log(CurrentState);
        switch (CurrentState)
        {
            case EnemyState.Moving:
                _animator.SetBool("fighting", false);
                _animator.SetBool("waiting", false);
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
                        switch (ownerId)
                        {
                            case 1:
                                GameObject AI = GameObject.Find("EnemyNode");
                                AI.GetComponent<AINode>().health -= 300;

                                break;

                            case 2:

                                GameObject player = GameObject.Find("PlayerNode");
                                player.GetComponent<PlayerControllsNodes>().health -= 300;

                                break;
                        }
                        //Destroy(tae, 2f);

                    }
                }
                break;

            case EnemyState.Rotating:
                _animator.SetBool("fighting", false);
                _animator.SetBool("waiting", false);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotatingSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRot) <= 0.2f)
                {
                    CurrentState = EnemyState.Moving;
                    nodeIndex++;
                }
                break;

            case EnemyState.Fighting:

                _animator.SetBool("fighting", true);
                _animator.SetBool("waiting", false);
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
            Enemy.transform.GetChild(0).gameObject.SetActive(false);
            Enemy.transform.GetChild(1).gameObject.SetActive(false);
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

//        private void OnTriggerStay(Collider col) {
//            
//            if (col.CompareTag("Spawn") && ownerId == 1 && Owner.GetComponent<PlayerControllsNodes>().flagCount == LaneIndex) {
//                Owner.GetComponent<PlayerControllsNodes>().Blocked = true;
//                Debug.Log("HAHAHAHHAAH");
//            } 
//            else if (col.CompareTag("SpawnEnemy") && ownerId == 2 && Owner.GetComponent<AINode>().chosenLane == LaneIndex) {
//                Owner.GetComponent<PlayerControllsNodes>().Blocked = true;
//            }
//        }
}