using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private GameObject mob;
    public List<Transform> spawns;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
            spawnMod(0);
        else if (Input.GetKeyDown(KeyCode.W))
            spawnMod(1);
        else if (Input.GetKeyDown(KeyCode.E))
            spawnMod(2);
        else if (Input.GetKeyDown(KeyCode.I))
            spawnMod(3);
        else if (Input.GetKeyDown(KeyCode.O))
            spawnMod(4);
        else if (Input.GetKeyDown(KeyCode.P))
            spawnMod(5);
    }

    void spawnMod(int id) {
        GameObject enemy = Instantiate(mob, spawns[id].position, Quaternion.identity);

        if (id > 2)
            enemy.GetComponent<MobBehaviour>().target = spawns[id - 3];
        else
            enemy.GetComponent<MobBehaviour>().target = spawns[id + 3];
    }
}
