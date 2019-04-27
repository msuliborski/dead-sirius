using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEater : MonoBehaviour
{

    private MobBehaviour mb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Soul")
        {

        }
    }
}

