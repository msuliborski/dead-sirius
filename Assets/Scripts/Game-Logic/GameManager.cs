using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : NetworkBehaviour
{

    public static GameManager Instance;
    [SerializeField] private MatchSettings _matchSettings;
    [SyncVar] public int ReadyPlayersCnt = 0;            
    public MatchSettings MatchSettings { get { return _matchSettings; } set { _matchSettings = value; } }
   
   
    void Start()
    {
      
        if (Instance != null) Debug.LogError("More than one GameManager in scene!");
        else Instance = this;
    }

    #region EnemySpawning
    
   
    public static void SetLayerRecursively(GameObject obj, string layerName)
    {
        if (obj == null) return;

        obj.layer = LayerMask.NameToLayer(layerName);

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, layerName);
        }
    }


    #endregion


    #region PlayerAndEnemies

    private const string PLAYER_ID_PREFIX = "Player ";

    public static PlayerManager LocalPlayer { get; set; }
    private static Dictionary<string, PlayerManager> _players = new Dictionary<string, PlayerManager>();
    private static Dictionary<string, PlayerManager> _activePlayers = new Dictionary<string, PlayerManager>();
    public static Dictionary<string, PlayerManager> Players { get { return _players; } }
    public static Dictionary<string, PlayerManager> ActivePlayers { get { return _activePlayers; } }
    
    public static void RegisterPlayer(string netId, PlayerManager player)
    {
        string playerId = PLAYER_ID_PREFIX + netId;
        _players.Add(playerId, player);
        _activePlayers.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        _players.Remove(playerId);
        if (_activePlayers.ContainsKey(playerId)) _activePlayers.Remove(playerId);
    }

    public static PlayerManager GetPlayer(string playerId)
    {
        if (!_players.ContainsKey(playerId)) return null;
        return _players[playerId];
    }
    

    public static void DeactivatePlayer(string playerId)
    {
        _activePlayers.Remove(playerId);
    }

    public static void ActivatePlayer(string playerId, PlayerManager player)
    {
        _activePlayers.Add(playerId, player);
    }

   
    #endregion
}
