using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float _hp;
    private int maxQueue = 5;
    public static int[][] mobsKinds = new int[3][];
    private int[] laneChances = new int[3];
    private int[] kindChances = new int[3];
    private float _randomLane = 0;
    private float _randomMob = 0;
    private bool canSpawn = true;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();
    public List<Transform> spawns;
    public List<GameObject> mobs;

    private int chosenLane;
    private int chosenKind;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            laneChances[i] = 10;
            kindChances[i] = 10;
        }
    }
    
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            laneChances[i] = 10;
            kindChances[i] = 10;
        }
        
        //////////////Choosing lane////////////////
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                laneChances[i] += mobsKinds[i][j] * 3;
            }
        }

        float sum = 0;
        for (int i = 0; i < 3; i++)
        {
            sum += laneChances[i];
        }

        _randomLane = Random.Range(0, 1);

        int temp = 0;
        for (int i = 0; i < 3; i++)
        {
            if (_randomLane <= temp + (laneChances[i] / sum))
            {
                chosenLane = i;
                break;
            }
            temp += laneChances[i];
        }
        
        
        /////////////Choosing mob/////////////////
        for (int i = 0; i < 3; i++)
        {
            kindChances[i] += mobsKinds[chosenLane][i] * 4;
        }

        sum = 0;
        for (int i = 0; i < 3; i++)
        {
            sum += kindChances[i];
        }

        _randomMob = Random.Range(0, 1);

        temp = 0;
        for (int i = 0; i < 3; i++)
        {
            if (_randomMob <= temp + (kindChances[i] / sum))
            {
                chosenKind = i;
                break;
            }
            temp += kindChances[i];
        }
        
        spawnMob(chosenKind, chosenLane);
        

    }
    
    public void spawnMob(int ID, int lane) {
        
        if (canSpawn)
        {
             GameObject mob;
            if (queue.Count == 0)
            {
                mob = Instantiate(mobs[ID], spawns[lane].position, Quaternion.identity);
            }
                
            else
            {
                mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
                queue.RemoveAt(0);
                lanes.RemoveAt(0);
                if (queue.Count < maxQueue)
                {
                    queue.Add(ID);
                    lanes.Add(lane);
                }
            }
            


            canSpawn = false;
            
            StartCoroutine(cooldown(ID, 2f));
        }
        else
        {
            if (queue.Count < maxQueue)
            {
                queue.Add(ID);
                lanes.Add(lane);
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
            queue.RemoveAt(0);
            lanes.RemoveAt(0);
            
            canSpawn = false;
            StartCoroutine(cooldown(ID, 2f));
        }
    }
}
