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
        _manager = GetComponent<PlayerManager>();
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

           
            _manager.Canvas = GameObject.Find("Canvas");

           
            _manager.Buttons.Add(_manager.Canvas.transform.GetChild(0).GetComponent<Button>());
                
            _manager.Buttons[0].onClick.AddListener(() => GetComponent<PlayerControlls>().spawnMob(0));

            _manager.Buttons.Add(_manager.Canvas.transform.GetChild(1).GetComponent<Button>());

            _manager.Buttons[1].onClick.AddListener(() => GetComponent<PlayerControlls>().spawnMob(1));


            _manager.Buttons.Add(_manager.Canvas.transform.GetChild(2).GetComponent<Button>());

            _manager.Buttons[2].onClick.AddListener(() => GetComponent<PlayerControlls>().spawnMob(2));

            
            


        }
        _manager.Setup();
       // GetComponent<PlayerControlls>().Setup();
    }

   
    public override void OnStartClient()
    {
        base.OnStartClient();
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<PlayerManager>());
    }

 

    private void DisableComponents()
    {
        transform.GetChild(0).gameObject.SetActive(false);
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


