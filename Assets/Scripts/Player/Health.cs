using Unity.Netcode;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public int maxHealth = 100;
    
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        currentHealth.Value = maxHealth;
    }


    public void TakeDamage(int damage){
        damage = damage<0? damage:-damage;
        currentHealth.Value += damage;
    }

    public void ReplenishHealth(int amount)
    {
        if (currentHealth.Value >= maxHealth) return;
        
        currentHealth.Value += amount;
        if (currentHealth.Value > maxHealth) currentHealth.Value = maxHealth;
    }
}
