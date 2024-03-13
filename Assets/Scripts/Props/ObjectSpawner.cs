using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject minePref;
    [SerializeField] private GameObject healthKitPref;
    [SerializeField] private GameObject shieldPref;
    
    public override void OnNetworkSpawn()
    {   
        if (!IsServer) return;
        
        if (minePref != null)
        {
            Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();
            
            GameObject mine = Instantiate(minePref, randomWorldPoint, quaternion.identity);
            
            NetworkObject networkObject = mine.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }

        if (healthKitPref != null)
        {
            Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();
            
            GameObject health = Instantiate(healthKitPref, randomWorldPoint, quaternion.identity);
            NetworkObject networkObject = health.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }

        if (shieldPref != null)
        {
            Vector3 randomWorldPoint = RandomPointUtility.GetRandomWorldPointInCamera();
            
            GameObject shield = Instantiate(shieldPref, randomWorldPoint, quaternion.identity);
            NetworkObject networkObject = shield.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
}
