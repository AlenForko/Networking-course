using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject minePref;
    [SerializeField] private GameObject healthKitPref;
    
    public override void OnNetworkSpawn()
    {   
        if (minePref != null)
        {
            float xPosition = Random.Range(-4, 4);
            float yPosition = Random.Range(-2, 2);
            
            GameObject mine = Instantiate(minePref, new Vector3(xPosition, yPosition, 0), quaternion.identity);
            
            NetworkObject networkObject = mine.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }

        if (healthKitPref != null)
        {
            float xPosition = Random.Range(-4, 4);
            float yPosition = Random.Range(-2, 2);
            
            GameObject health = Instantiate(healthKitPref, new Vector3(xPosition, yPosition, 0), quaternion.identity);
            NetworkObject networkObject = health.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
}
