using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackController : MonoBehaviour
{
    MobBehaviourNodes mbn;

    // Start is called before the first frame update
    void Start()
    {
        mbn = GetComponent<MobBehaviourNodes>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
