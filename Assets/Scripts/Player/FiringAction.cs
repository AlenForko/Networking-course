using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
        
    public float shootingCooldown = 0.5f;
    private float lastShootTime;
    
    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
    }

    private void Fire(bool isShooting)
    {
        if (isShooting && Time.time >= lastShootTime + shootingCooldown)
        {
            lastShootTime = Time.time;
            ShootLocalBullet();
        }
    }
    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        Debug.Log("Shooting locally");
        ShootBulletServerRpc();
    }
    
    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        Debug.Log("Shooting from serverRPC");
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        Debug.Log("Shooting from clientRPC");
    }
}
