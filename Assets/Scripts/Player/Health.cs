using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> currentLives = new NetworkVariable<int>();
    [SerializeField]private int _maxHealth = 100;

    public int MaxHealth => _maxHealth;

    public override void OnNetworkSpawn()
    {
        currentHealth.Value = _maxHealth;
        currentLives.Value = 3;
    }

    public void TakeDamage(int damage)
    {
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;

        if (currentHealth.Value <= 0)
        {
            PlayerDeath();
        }
    }
    
    public void ReplenishHealth(int amount)
    {
        if (currentHealth.Value >= _maxHealth) return;

        currentHealth.Value += amount;
        if (currentHealth.Value > _maxHealth) currentHealth.Value = _maxHealth;
    }

    private void PlayerDeath()
    {
        if (!IsServer) return;

        currentLives.Value--;
        if (currentLives.Value > 0)
        {
            RespawnClientRpc();
            currentHealth.Value = _maxHealth;
        }
        else
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            networkObject.Despawn(true);
        }
    }

    [ClientRpc]
    private void RespawnClientRpc()
    {
        if (!IsOwner) return;
        Respawn();
    }
    
    private void Respawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = quaternion.identity;
    }
}