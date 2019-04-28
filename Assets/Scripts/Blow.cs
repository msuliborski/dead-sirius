using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blow : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(blow());
    }

    IEnumerator blow()
    {
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            if (other.gameObject.GetComponent<MobBehaviour>().ownerId != 1)
                other.gameObject.GetComponent<MobBehaviour>().health = 0;
        }
    }
}
