using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControlls : NetworkBehaviour
{
    [SerializeField] private GameObject flag;
    
    private int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private bool[] canSpawn = new bool[3];
    private PlayerManager _manager;
    private GameObject _base;
    
   public void Setup()
   {
        _manager = GetComponent<PlayerManager>();
        _base = _manager.Base;

        for (int i = 0; i < _base.transform.childCount; i++)
            spawns.Add(_base.transform.GetChild(i));

        for (int i = 0; i < 3; i++)
        {
            canSpawn[i] = true;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
            flagCount = 0;
        else if (Input.GetKeyDown(KeyCode.W))
            flagCount = 1;
        else if (Input.GetKeyDown(KeyCode.Q))
            flagCount = 2;
        
        flag.transform.position = spawns[flagCount].transform.position;
    }


    [Command]
    void CmdSpawnEnemy(int ID)
    {
        NetworkServer.Spawn(Instantiate(Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity)));
    }




    public void spawnMob(int ID) {
        if (canSpawn[ID])
        {
            CmdSpawnEnemy(ID);

            /*if (flagCount > 2) {
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount - 3];
                enemy.GetComponent<MobBehaviour>().ownerId = 0;
            }
            else {
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount + 3];
                enemy.GetComponent<MobBehaviour>().ownerId = 1;
            }

            canSpawn[ID] = false;
            StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviour>().spawnTime));*/
        }
    }


    IEnumerator cooldown(int ID, float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn[ID] = true;
    }
}