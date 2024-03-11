using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StandardMine : NetworkBehaviour
{
    
   [SerializeField] GameObject minePrefab;
   private Camera _mainCamera;

   public override void OnNetworkSpawn()
   {
       _mainCamera = Camera.main;
   }
   
   void OnTriggerEnter2D(Collider2D other)
   {
       if (!IsServer) return;
       
       Health health = other.GetComponent<Health>();
       
       if(!health) return;
       health.TakeDamage(25);

       Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();

       
       GameObject newMine = Instantiate(minePrefab, randomWorldPoint, Quaternion.identity);
       NetworkObject no = newMine.GetComponent<NetworkObject>();
       no.Spawn();
       
       NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
       networkObject.Despawn();
   }
}
