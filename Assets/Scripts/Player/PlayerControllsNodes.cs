using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerControllsNodes : MonoBehaviour
{
    
    [SerializeField] private GameObject flag;
    [SerializeField] private int maxQueue = 5;
    public static int queueCount = 0;
    public int health;
    public float maxHealth;
    public int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    public bool canSpawn;
    private PlayerManager _manager;
    [SerializeField] private GameObject _flag;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();
    public List<Transform> nodes1;
    public List<Transform> nodes2;
    public List<Transform> nodes3;
//    public bool Blocked = false;
    public bool[] isBlocked = new bool[3];
    

    
    
   public void Start()
   {
       
        _manager = GetComponent<PlayerManager>();
       // _flag.transform.position = new Vector3(spawns[0].position.x, , spawns[0].position.z);
        
        canSpawn = true;
   }

        
    

    void Update()
    {
        queueCount = queue.Count();

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            flagCount = 0;
         //   Blocked = false;
        }
            
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            flagCount = 1;
          //  Blocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            flagCount = 2;
          //  Blocked = false;
        }
        
        _flag.transform.position = new Vector3(spawns[flagCount].transform.position.x, 2f, spawns[flagCount].transform.position.z);
        
        
    }





    public void spawnMob(int ID) {
        
        if (canSpawn && !isBlocked[flagCount])
        {

            GameObject mob;
            if (queue.Count == 0)
            {
                mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
                //AINode.mobsKinds[flagCount][ID]++;

            }
                
            else
            {
                mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
                //mob.transform.name = "PlayerMob";
                //AINode.mobsKinds[lanes[0]][queue[0]]++;
                queue.RemoveAt(0);
                lanes.RemoveAt(0);
                if (queue.Count < maxQueue)
                {
                    queue.Add(ID);
                    lanes.Add(flagCount);
                }
            }

            MobBehaviourNodes enemy = mob.GetComponent<MobBehaviourNodes>();
            if (flagCount == 0) enemy.Nodes = nodes1;
            else if (flagCount == 1) enemy.Nodes = nodes2;
            else enemy.Nodes = nodes3;
            enemy.LaneIndex = flagCount;
            enemy.ownerId = 1;
            enemy.TypeIndex = ID;
            health -= enemy.healthCost;

            canSpawn = false;
            StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviourNodes> ().spawnTime));
            
        }
        else
        {
            if (queue.Count < maxQueue)
            {
                queue.Add(ID);
                lanes.Add(flagCount);
            }
        }
    }


    IEnumerator cooldown(int ID, float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn = true;
        if (queue.Count != 0)
        {
           if (!isBlocked[lanes[0]]) {
                GameObject mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
                //AINode.mobsKinds[lanes[0]][queue[0]]++;
                MobBehaviourNodes enemy = mob.GetComponent<MobBehaviourNodes>();
                if (lanes[0] == 0) enemy.Nodes = nodes1;
                else if (lanes[0] == 1) enemy.Nodes = nodes2;
                else enemy.Nodes = nodes3;
                enemy.ownerId = 1;
                enemy.LaneIndex = lanes[0];
                enemy.TypeIndex = ID;
                health -= enemy.healthCost;
                queue.RemoveAt(0);
                lanes.RemoveAt(0);
                canSpawn = false;
            }

            StartCoroutine(cooldown(ID, 2f));
        }
    }
    
    



}