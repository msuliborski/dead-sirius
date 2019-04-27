using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] _toDisable;
    private Camera _sceneCamera;
    
   
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
          
        }
        else
        {
            _sceneCamera = GameObject.Find("Scene Camera").GetComponent<Camera>();
            if (_sceneCamera != null)
                _sceneCamera.gameObject.SetActive(false);
           
            GameManager.LocalPlayer = GetComponent<PlayerManager>();
            
            
        }
        GetComponent<PlayerManager>().Setup();
        GetComponent<PlayerControlls>().Setup();
    }


   

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<PlayerManager>());
    }

 

    private void DisableComponents()
    {
        for (int i = 0; i < _toDisable.Length; i++)
            _toDisable[i].enabled = false;
    }

    private void OnDisable()
    {
        if (isLocalPlayer)
        {
            if (_sceneCamera != null)
                _sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }

    private void OnEnable()
    {
        if (isLocalPlayer)
        {
            if (_sceneCamera != null)
                _sceneCamera.gameObject.SetActive(false);
        }
    }
}


