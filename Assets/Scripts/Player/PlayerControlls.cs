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
    [SerializeField] private GameObject _flag;
    
   public void Setup()
   {
        
        _manager = GetComponent<PlayerManager>();
        _base = _manager.Base;

        for (int i = 0; i < _base.transform.childCount; i++)
            spawns.Add(_base.transform.GetChild(i));
        _flag.transform.position = spawns[0].position;
        for (int i = 0; i < 3; i++)
        {
            canSpawn[i] = true;
        }
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


    /*[Command]
    void CmdSpawnEnemy(int ID)
    {
        //Debug.Log("Command");
        //GameObject mob;
        
        //RpcSpawnEnemy(mob.GetComponent<NetworkIdentity>());
        
    }*/

    //[ClientRpc]
    // void RpcSpawnEnemy(NetworkIdentity id)
    // {
    //    ClientScene.FindLocalObject(id)
    // }


    [Command]
    void CmdSpawnMob(Vector3 pos, Quaternion rot, int id)
    {
        RpcSpawnMob(pos, rot, id);
    }

    [ClientRpc]
    void RpcSpawnMob(Vector3 pos, Quaternion rot, int id)
    {
        if (!isLocalPlayer)
        {
            GameObject temp = Instantiate(mobs[id], pos, rot);
        }
    }


    public void spawnMob(int ID) {
        ID -= 3; // XDDDD 
        if (canSpawn[ID])
        {
            Debug.Log("spawn");
            //CmdSpawnEnemy(ID);
            GameObject mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
            CmdSpawnMob(mob.transform.position, mob.transform.rotation, ID);
            /*if (flagCount > 2) {
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount - 3];
                enemy.GetComponent<MobBehaviour>().ownerId = 0;
            }
            else {
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount + 3];
                enemy.GetComponent<MobBehaviour>().ownerId = 1;
            }*/

            canSpawn[ID] = false;
            //StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviour>().spawnTime));
            StartCoroutine(cooldown(ID, 2f));
        }
    }


    IEnumerator cooldown(int ID, float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn[ID] = true;
    }
}