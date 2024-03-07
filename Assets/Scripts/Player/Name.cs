using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Name : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>();
    
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        string userName = SavedClientInformationManager.GetUserData(NetworkObject.OwnerClientId).userName;
        playerName.Value = userName;
    }
}
