using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINode : MonoBehaviour
{
    public float health;
    public float maxHealth;
    private int maxQueue = 5;
    public static int[][] mobsKinds = new int[3][];
    private float[] laneChances = new float[3];
    private float[] kindChances = new float[3];
    private float _randomLane = 0;
    private float _randomMob = 0;
    private float _random = 0;
    private bool canSpawn = true;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private GameObject _enemyBase;
    public List<Transform> nodes1;
    public List<Transform> nodes2;
    public List<Transform> nodes3;

    private int chosenLane;
    private int chosenKind;

    void Start()
    {
        _enemyBase = GameObject.Find("Base1");
        for (int i = 0; i < 3; i++)
        {
            mobsKinds[i] = new int[3];
            laneChances[i] = 1;
            kindChances[i] = 1;
        }
    }
    
    void Update()
    {
        updateModInfo();
        for (int i = 0; i < 3; i++)
        {
            laneChances[i] = 1;
            kindChances[i] = 1;
        }
        
        //////////////Choosing lane////////////////
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                laneChances[i] += mobsKinds[i][j] * 20;
            }
            Debug.Log(i + "/" + laneChances[i]);
        }

        float sum = 0;
        for (int i = 0; i < 3; i++)
        {
            sum += laneChances[i];
        }
        Debug.Log("Sum "+ sum);

        _randomLane = Random.Range(0f, 1f);
        Debug.Log("Random "+ _randomLane);

        float L0Chance = laneChances[0] / sum;
        float L1Chance = laneChances[1] / sum;
        float L2Chance = laneChances[2] / sum;

        if (_randomLane < L0Chance)
        {
            chosenLane = 0;
        } else if (_randomLane >= L2Chance && _randomLane < L0Chance+L1Chance)
        {
            chosenLane = 1;
        }
        else
        {
            chosenLane = 2;
        }
        Debug.Log(chosenLane);
        
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

        float M0Chance = kindChances[0] / sum;
        float M1Chance = kindChances[1] / sum;
        float M2Chance = kindChances[2] / sum;

        if (_randomMob < M0Chance)
            chosenKind = 2;
        else if (_randomMob >= M2Chance && _randomMob < M0Chance+M1Chance)
            chosenKind = 1;
        else 
            chosenKind = 0;
        
        _random = Random.Range(0, 1);
        if (_random <= health / maxHealth) 
            spawnMob(chosenKind, chosenLane);
            
    }

    private void updateModInfo() {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");
        Debug.Log("CHUJUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU " + mobs.Length);
        
        mobsKinds = new int[3][];
        for (int i = 0; i < 3; i++)
            mobsKinds[i] = new int[3];
        
        foreach (var mob in mobs) {
            if (mob.GetComponent<MobBehaviourNodes>().ownerId != 2)
                mobsKinds[mob.GetComponent<MobBehaviourNodes>().LaneIndex][mob.GetComponent<MobBehaviourNodes>().TypeIndex]++;
        }

            
        

    }
    
    public void spawnMob(int ID, int lane) {
        
        Debug.Log("Spawn: ID: " + ID + " lane: " + lane + "chuju");
        
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
            
            MobBehaviourNodes enemy = mob.GetComponent<MobBehaviourNodes>();
            if (lane == 0) enemy.Nodes = nodes1;
            else if (lane == 1) enemy.Nodes = nodes2;
            else enemy.Nodes = nodes3;
            enemy.PrintNodes();
            enemy.LaneIndex = lane;
            enemy.ownerId = 2;

            canSpawn = false;
            
            StartCoroutine(cooldown(ID, 5f));
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
            MobBehaviourNodes enemy = mob.GetComponent<MobBehaviourNodes>();
            enemy.baseTarget = _enemyBase.transform;
            enemy.LaneIndex = lanes[0];
            if (lanes[0] == 0) enemy.Nodes = nodes1;
            else if (lanes[0] == 1) enemy.Nodes = nodes2;
            else enemy.Nodes = nodes3;
            enemy.PrintNodes();
            enemy.ownerId = 2;
            queue.RemoveAt(0);
            lanes.RemoveAt(0);
            canSpawn = false;
            StartCoroutine(cooldown(ID, 5f));
        }
    }
}
