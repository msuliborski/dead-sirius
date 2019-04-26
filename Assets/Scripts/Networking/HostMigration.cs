using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;

public class HostMigration : NetworkMigrationManager
{
    public override bool FindNewHost(out PeerInfoMessage newHostInfo, out bool youAreNewHost)
    {
        bool isHost = base.FindNewHost(out newHostInfo, out youAreNewHost);
        return isHost;
    }
    
    protected override void OnServerHostShutdown()
    {
        base.OnServerHostShutdown();
      
    }
    
    

}
