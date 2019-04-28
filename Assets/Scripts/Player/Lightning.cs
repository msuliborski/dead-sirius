using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private GameObject thunder;
    public void Smite()
    {
        
//        while (!Input.GetMouseButton(0))
//        {
//        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log("Smited");
            Instantiate(thunder, hitInfo.transform.position, Quaternion.identity);
        }
    
    }
}
