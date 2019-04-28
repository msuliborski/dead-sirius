using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
   // public bool block;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        //block = false;
    }

    private void OnTriggerStay(Collider other) {
       // block = true;
    }
    
//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Mob")) 
//            other.GetComponent<MobBehaviour>().GetComponent<PlayerControllsNodes>().Blocked = false;
//        else if (other.CompareTag("Mob"))
//            other.GetComponent<MobBehaviour>().GetComponent<AINode>().Blocked = false;
//
//    }
}
