using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QueueCount : MonoBehaviour
{
    private TextMeshProUGUI _tm;
    void Start()
    {
        _tm = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _tm.text = "QUEUE:\n" + PlayerControllsNodes.queueCount;
    }
}
