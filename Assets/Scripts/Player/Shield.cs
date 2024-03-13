using Unity.Netcode;

public class Shield : NetworkBehaviour
{
    public NetworkVariable<int> currentShieldAmount = new NetworkVariable<int>();
    public NetworkVariable<bool> hasShield = new NetworkVariable<bool>();

    private int _maxShieldHits = 2;
    
    public override void OnNetworkSpawn()
    {
        hasShield.Value = false;
    }

    public void AddShield()
    {
        if (!IsServer) return;

        if (!hasShield.Value)
        {
            hasShield.Value = true;
            currentShieldAmount.Value = _maxShieldHits;
        }
        
    }

    public void TakeShieldDamage(int damage)
    {
        if (hasShield.Value)
        {
            currentShieldAmount.Value -= damage;

            if (currentShieldAmount.Value <= 0)
            {
                RemoveShield();
            }
        }    
    }
    
    private void RemoveShield()
    {
        hasShield.Value = false;
    }
}
