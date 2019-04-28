using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
  

    public int ownerId;
    public GameObject Owner;
    public int selfId;
    
    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("Mob")) 
            other.GetComponent<MobBehaviour>().GetComponent<PlayerControllsNodes>().Blocked = false;
        else if (other.CompareTag("Mob"))
            other.GetComponent<MobBehaviour>().GetComponent<AINode>().Blocked = false;*/
        
        //Debug.Log("kuuuuurrrwa1");
        if (other.CompareTag("Mob")) {
            //Debug.Log("kurrrrrrwa2");
            
            
            MobBehaviourNodes mb = other.GetComponent<MobBehaviourNodes>();
            //Debug.Log("OwnerID: " + mb.ownerId + ", id: " + ownerId);
            if (mb.ownerId == ownerId)
            {


                switch (ownerId)
                {
                    case 1:

                        Owner.GetComponent<PlayerControllsNodes>().isBlocked[selfId] = true;

                        break;
                    case 2:
                        Owner.GetComponent<AINode>().isBlocked[selfId] = true;
                        break;
                }


            }
            else mb.HasArrived = true;
        }
        

    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Mob")) {
            MobBehaviourNodes mb = other.GetComponent<MobBehaviourNodes>();
            if (mb.ownerId == ownerId) {

                switch (ownerId) {
                    case 1:
                        Owner.GetComponent<PlayerControllsNodes>().isBlocked[selfId] = false;

                        break;
                    case 2:
                        Owner.GetComponent<AINode>().isBlocked[selfId] = false;
                        break;
                }
                
                
            }
        }
    }
}
