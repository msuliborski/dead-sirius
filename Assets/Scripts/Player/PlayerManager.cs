﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{

    //[SyncVar] private bool _isDead = false;
    //public bool IsDead { get { return _isDead; }  protected set { _isDead = value; } }

    [SerializeField] private float _maxHealth = 100;

    [SyncVar] private float _currentHealth;
    
    public GameObject Canvas { get; set; }

    public GameObject Base { get; set; }

    private List<Button> _buttons = new List<Button>();

    public List<Button> Buttons { get { return _buttons; } }
    
    public void Setup()
    {

        if (transform.position.z < 0)
            Base = GameObject.Find("Base1");
        else Base = GameObject.Find("Base2");


        _currentHealth = _maxHealth;
       
    }

    public void SetDefaults()
    {
        //_isDead = false;

        _currentHealth = _maxHealth;

    }

    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        //if (_isDead) return;

        _currentHealth -= damage;

        Debug.Log(transform.name + " now has " + _currentHealth + " health.");

       // if (_currentHealth <= 0) Die();
    }

    
}