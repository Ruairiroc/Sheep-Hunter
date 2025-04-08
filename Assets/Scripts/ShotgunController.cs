using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] ShotgunData shotgunData;
    [SerializeField] private Transform muzzle;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    public void StartReload()
    {
        if (!shotgunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        shotgunData.reloading = true;
        yield return new WaitForSeconds(shotgunData.reloadTime);
        shotgunData.currentAmmo = shotgunData.maxAmmo;
        shotgunData.reloading = false;
    }


    private bool canShoot() => !shotgunData.reloading && timeSinceLastShot > 1f / (shotgunData.fireRate / 60f);

    public void Shoot()
    {
        if (shotgunData.currentAmmo > 0)
        {
            if (canShoot())
            {
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, shotgunData.range))
                {
                    Debug.Log($"Hit: {hit.transform.name}");
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.Damage(shotgunData.damage);
                }

                shotgunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward * shotgunData.range, Color.red);
    }

    private void OnGunShot()
    {
    }
}
