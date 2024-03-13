using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class ShieldProp : NetworkBehaviour
{
    [SerializeField] private GameObject shieldPropPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        Shield shield = other.GetComponent<Shield>();

        if (!shield) return;
        
        if(shield.hasShield.Value) return;
        
        shield.AddShield();
        
        Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();

        GameObject newShieldProp =
            Instantiate(shieldPropPrefab, randomWorldPoint, quaternion.identity);

        NetworkObject networkObject = newShieldProp.GetComponent<NetworkObject>();
        networkObject.Spawn();

        NetworkObject healthNetworkObject = gameObject.GetComponent<NetworkObject>();
        healthNetworkObject.Despawn();
    }
}

