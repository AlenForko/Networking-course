using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthKit : NetworkBehaviour
{
    [SerializeField] private GameObject healthkitPrefab;

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        _mainCamera = Camera.main;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        Health health = other.GetComponent<Health>();

        if (!health) return;
        
        if(health.currentHealth.Value >= health.MaxHealth) return;
        
        health.ReplenishHealth(10);
        
        Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera(_mainCamera);

        GameObject newHealthKit =
            Instantiate(healthkitPrefab, randomWorldPoint, quaternion.identity);

        NetworkObject networkObject = newHealthKit.GetComponent<NetworkObject>();
        networkObject.Spawn();

        NetworkObject healthNetworkObject = gameObject.GetComponent<NetworkObject>();
        healthNetworkObject.Despawn();
    }
}
