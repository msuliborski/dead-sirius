using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControlls : MonoBehaviour
{
    
    [SerializeField] private GameObject flag;
    [SerializeField] private int maxQueue = 5;
    
    private int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private bool canSpawn;
    private PlayerManager _manager;
    private GameObject _enemyBase;
    [SerializeField] private GameObject _flag;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();

    
    
   public void Start()
   {    
        _manager = GetComponent<PlayerManager>();

        _enemyBase = GameObject.Find("Base2");

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
        
        flag.transform.position = spawns[flagCount].transform.position;
        
        
    }





    public void spawnMob(int ID) {
        
        if (canSpawn)
        {

            GameObject mob;
            if (queue.Count == 0)
            {
                mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
                AI.mobsKinds[flagCount][ID]++;
            }
                
            else
            {
                mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
                AI.mobsKinds[lanes[0]][queue[0]]++;
                queue.RemoveAt(0);
                lanes.RemoveAt(0);
                if (queue.Count < maxQueue)
                {
                    queue.Add(ID);
                    lanes.Add(flagCount);
                }
            }

            MobBehaviour enemy = mob.GetComponent<MobBehaviour>();

            enemy.target = _enemyBase.transform;
            enemy.LaneIndex = flagCount;
            enemy.ownerId = 1;

            canSpawn = false;
            StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviour>().spawnTime));
            
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
            AI.mobsKinds[lanes[0]][queue[0]]++;
            MobBehaviour enemy = mob.GetComponent<MobBehaviour>();
            enemy.target = _enemyBase.transform;
            enemy.LaneIndex = flagCount;
            enemy.ownerId = 1;
            queue.RemoveAt(0);
            lanes.RemoveAt(0);
            canSpawn = false;
            StartCoroutine(cooldown(ID, 2f));
        }
    }
}