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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        Health health = other.GetComponent<Health>();

        if (!health) return;
        
        if(health.currentHealth.Value >= health.maxHealth) return;
        
        health.ReplenishHealth(10);
        
        float xPosition = Random.Range(-4, 4);
        float yPosition = Random.Range(-2, 2);

        GameObject newHealthKit =
            Instantiate(healthkitPrefab, new Vector3(xPosition, yPosition, 0), quaternion.identity);

        NetworkObject networkObject = newHealthKit.GetComponent<NetworkObject>();
        networkObject.Spawn();

        NetworkObject healthNetworkObject = gameObject.GetComponent<NetworkObject>();
        healthNetworkObject.Despawn();
    }
}
