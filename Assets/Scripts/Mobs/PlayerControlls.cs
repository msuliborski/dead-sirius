using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private GameObject flag;
    
    private int flagCount = 0;
    public List<Transform> spawns;
    public List<GameObject> mobs;
    private bool[] canSpawn = new bool[3];

    void Start()
    {
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

    public void spawnMob(int ID) {
        if (canSpawn[ID])
        {
            GameObject enemy = Instantiate(mobs[ID], spawns[flagCount].position, Quaternion.identity);

            if (flagCount > 2)
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount - 3];
            else
                enemy.GetComponent<MobBehaviour>().target = spawns[flagCount + 3];

            canSpawn[ID] = false;
            StartCoroutine(cooldown(ID, enemy.GetComponent<MobBehaviour>().spawnTime));
        }
    }


    IEnumerator cooldown(int ID, int time)
    {
        yield return new WaitForSeconds(time);
        canSpawn[ID] = true;
    }
}
