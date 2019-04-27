using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControlls : NetworkBehaviour
{
    
    [SerializeField] private GameObject flag;
    [SerializeField] private int maxQueue = 5;
    
    private int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private bool canSpawn;
    private PlayerManager _manager;
    private GameObject _base;
    [SerializeField] private GameObject _flag;
    private List<int> queue = new List<int>();
    private List<int> lanes = new List<int>();

    
    
   public void Setup()
   {    
        _manager = GetComponent<PlayerManager>();
        
        _base = _manager.Base;


            
        if (isLocalPlayer)
        {
            _flag.SetActive(true);
        
            for (int i = 0; i < _base.transform.childCount; i++)
                spawns.Add(_base.transform.GetChild(i));
            _flag.transform.position = spawns[0].position;
            canSpawn = true;

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
        /*GameObject mob= Instantiate(mobs[id], pos, rot);
        MobBehaviour mb = mob.GetComponent<MobBehaviour>();
        mb.ownerId = _manager.PlayerId;
        RpcSpawnMob(pos, rot, id);*/

        GameObject mob = Instantiate(mobs[id], pos, rot);
        MobBehaviour mb = mob.GetComponent<MobBehaviour>();
        mb.ownerId = _manager.PlayerId;
        NetworkServer.Spawn(mob);
    }

    [ClientRpc]
    void RpcSpawnMob(Vector3 pos, Quaternion rot, int id)
    {
        if (!isServer)
        {

            GameObject temp = Instantiate(mobs[id], pos, rot);
        }
    }


    public void spawnMob(int ID) {
        
        if (canSpawn)
        {


            CmdSpawnMob(spawns[flagCount].position, Quaternion.identity, ID);

            //GameObject mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
            // GameObject mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
            
            

            //CmdSpawnMob(spawns[flagCount].position, Quaternion.identity , ID);

           /* GameObject mob;
            if (queue.Count == 0)
            {
                mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
            }
                
            else
            {
                mob = Instantiate(mobs[queue[0]], spawns[lanes[0]].position, Quaternion.identity);
                queue.RemoveAt(0);
                lanes.RemoveAt(0);
                if (queue.Count < maxQueue)
                {
                    queue.Add(ID);
                    lanes.Add(flagCount);
                }
            }
                

            
            //CmdSpawnEnemy(ID);
            
            //GameObject mob = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);
            MobBehaviour mb = mob.GetComponent<MobBehaviour>();
            mb.ownerId = _manager.PlayerId;

            CmdSpawnMob(mob.transform.position, mob.transform.rotation, ID); */


            canSpawn = false;
            //StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviour>().spawnTime));
            StartCoroutine(cooldown(ID, 2f));
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
            queue.RemoveAt(0);
            lanes.RemoveAt(0);
            CmdSpawnMob(mob.transform.position, mob.transform.rotation, ID);
            canSpawn = false;
            StartCoroutine(cooldown(ID, 2f));
        }
    }
}