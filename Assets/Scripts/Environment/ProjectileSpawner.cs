using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [Header("Configuration")]
    [SerializeField] private float timeBetweenSpawn = 5f;
    [Header("Projectile Parameters Override")]
    [SerializeField] private bool overrideProjectileParameters = false;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private int projectileDamage = 1;
    // ============ Private Variables ============
    private float projectileSpawnrateTimer = 0;    

    // Update is called once per frame
    void Update()
    {
        projectileSpawnrateTimer += Time.deltaTime;
        if (projectileSpawnrateTimer >= timeBetweenSpawn)
        {
            SpawnProjectile();
            projectileSpawnrateTimer = 0;
        }
    }

    private void SpawnProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        if (overrideProjectileParameters)
        {
            EnvironmentProjectile projectile = proj.GetComponent<EnvironmentProjectile>();
            projectile.projectileSpeed = projectileSpeed;
            projectile.projectileLifetime = projectileLifetime;
            projectile.projectileDamage = projectileDamage;
        }
    }
}
