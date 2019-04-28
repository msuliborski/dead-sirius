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
    public int health;
    public float maxHealth;
    private int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private bool canSpawn;
    private PlayerManager _manager;
    [SerializeField] private GameObject _flag;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();
    public List<Transform> nodes1;
    public List<Transform> nodes2;
    public List<Transform> nodes3;

    
    
   public void Start()
   {    
        _manager = GetComponent<PlayerManager>();

        

        _flag.transform.position = spawns[0].position;
        canSpawn = true;

   }

        
    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
            flagCount = 0;
        else if (Input.GetKeyDown(KeyCode.I))
            flagCount = 1;
        else if (Input.GetKeyDown(KeyCode.O))
            flagCount = 2;
        
        flag.transform.position = new Vector3(spawns[flagCount].transform.position.x, 0.5f, spawns[flagCount].transform.position.z);
        
        
    }





    public void spawnMob(int ID) {
        
        if (canSpawn)
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
            GameObject mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
            //AINode.mobsKinds[lanes[0]][queue[0]]++;
            MobBehaviourNodes enemy = mob.GetComponent<MobBehaviourNodes>();
            if (lanes[0] == 0) enemy.Nodes = nodes1;
            else if (lanes[0] == 1) enemy.Nodes = nodes2;
            else enemy.Nodes = nodes3;
            enemy.ownerId = 1;
            enemy.LaneIndex = lanes[0];
            enemy.TypeIndex = ID;
            queue.RemoveAt(0);
            lanes.RemoveAt(0);
            canSpawn = false;
            StartCoroutine(cooldown(ID, 2f));
        }
    }



}