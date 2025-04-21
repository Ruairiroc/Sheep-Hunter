using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] ShotgunData shotgunData;
    [SerializeField] private Transform muzzle;

    float timeSinceLastShot;

    int amountOfPellets = 8;

    private void Start()
    {
        Debug.Log("ShotgunController subscribed to shootInput");
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
        Debug.Log("ShotgunController.Shoot() called");
        if (shotgunData.currentAmmo > 0)
        {
            if (canShoot())
            {
                for (int i = 0; i < amountOfPellets; i++)
                {
                    Vector3 spread = muzzle.forward;
                    spread += muzzle.right * Random.Range(-shotgunData.spread, shotgunData.spread); // Horizontal spread
                    spread += muzzle.up * Random.Range(-shotgunData.spread, shotgunData.spread);   // Vertical spread
                    spread.Normalize();

                    Debug.DrawRay(muzzle.position, spread * shotgunData.range, Color.red, 1f);


                    if (Physics.Raycast(muzzle.position, spread, out RaycastHit hit, shotgunData.range))
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
