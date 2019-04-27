using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] _toDisable;
    private Camera _sceneCamera;
    private PlayerManager _manager;
    
   
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

            _manager = GetComponent<PlayerManager>();
            _manager.Canvas = GameObject.Find("Canvas");

            for (int id = 0; id < _manager.Canvas.transform.childCount; id++)
            {
                _manager.Buttons.Add(_manager.Canvas.transform.GetChild(id).GetComponent<Button>());
              
               _manager.Buttons[id].onClick.AddListener(() => GetComponent<PlayerControlls>().spawnMob(id));
                Debug.Log(id);
            }

            

            
        }
        _manager.Setup();
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


