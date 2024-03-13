using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> currentLives = new NetworkVariable<int>();
    [SerializeField]private int _maxHealth = 100;

    private Shield _shield;
    public int MaxHealth => _maxHealth;

    public override void OnNetworkSpawn()
    {
        currentHealth.Value = _maxHealth;
        currentLives.Value = 3;
        _shield = GetComponent<Shield>();
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        if (_shield.hasShield.Value)
        {
            _shield.TakeShieldDamage(1);
            Debug.Log("Shield amount: " + _shield.currentShieldAmount.Value);
            return;
        }
        
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;

        if (currentHealth.Value <= 0)
        {
            PlayerDeath();
            Debug.Log("Health amount: " + currentHealth.Value);
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
        transform.position = RandomPointUtility.GetRandomWorldPointInCamera();
        transform.rotation = quaternion.identity;
    }
}