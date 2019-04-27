using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEater : MonoBehaviour
{

    private MobBehaviour mb;
    private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Soul")
        {
            if (mb.ownerId == 1)
                mb.Owner.GetComponent<PlayerControlls>().Health += 450;
            else // same for AI

            Destroy(other.gameObject);


        }
    }
}

