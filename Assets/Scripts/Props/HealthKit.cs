using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class HealthKit : NetworkBehaviour
{
    [SerializeField] private GameObject healthkitPrefab;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        Health health = other.GetComponent<Health>();
        Shield shield = other.GetComponent<Shield>();
        
        if (!health) return;
        
        if(health.currentHealth.Value >= health.MaxHealth) return;
        
        health.ReplenishHealth(10);
        
        Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();

        GameObject newHealthKit =
            Instantiate(healthkitPrefab, randomWorldPoint, quaternion.identity);

        NetworkObject networkObject = newHealthKit.GetComponent<NetworkObject>();
        networkObject.Spawn();

        NetworkObject healthNetworkObject = gameObject.GetComponent<NetworkObject>();
        healthNetworkObject.Despawn();
    }
}
