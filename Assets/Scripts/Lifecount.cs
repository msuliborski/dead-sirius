using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lifecount : MonoBehaviour
{
    private PlayerControllsNodes _player;
    private AINode _ai;
    [SerializeField] private TextMeshProUGUI _tmPlayer;
    [SerializeField] private TextMeshProUGUI _tmEnemy;
    void Start()
    {
        _player = GameObject.Find("PlayerNode").GetComponent<PlayerControllsNodes>();
        _ai = GameObject.Find("EnemyNode").GetComponent<AINode>();
    }

    void Update()
    {
        _tmPlayer.text = _player.health.ToString();
        _tmEnemy.text = _ai.health.ToString();
    }
}
