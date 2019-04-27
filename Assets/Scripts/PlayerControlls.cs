using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] private GameObject mob;
    public List<Transform> spawns;

    void Update()
    {
        int temp = 100;

        if (Input.GetKeyDown(KeyCode.Q))
            temp = 0;
        else if (Input.GetKeyDown(KeyCode.W))
            temp = 1;
        else if (Input.GetKeyDown(KeyCode.E))
            temp = 2;
        else if (Input.GetKeyDown(KeyCode.I))
            temp = 3;
        else if (Input.GetKeyDown(KeyCode.O))
            temp = 4;
        else if (Input.GetKeyDown(KeyCode.P))
            temp = 5;

        if (temp != 100)
        {
            GameObject enemy = Instantiate(mob, spawns[temp].position, Quaternion.identity);

            if (temp + 3 > 5)
                enemy.GetComponent<MobMovement>().target = spawns[temp - 3];

            else
                enemy.GetComponent<MobMovement>().target = spawns[temp + 3];
        }
        
    }
}
