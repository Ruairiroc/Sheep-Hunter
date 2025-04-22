using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShotgunController : MonoBehaviour
{
    [SerializeField] ShotgunData shotgunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private TextMeshProUGUI ammoText;

    float timeSinceLastShot;

    int amountOfPellets = 8;
    public string slash = "/";
    private void Start()
    {
        Debug.Log("ShotgunController subscribed to shootInput");
        shotgunData.reloading = false;
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        ammoText.text = shotgunData.currentAmmo.ToString() + slash + shotgunData.maxAmmo.ToString();

    }

    public void StartReload()
    {
        Debug.Log("StartReload called");
        if (!shotgunData.reloading)
        {
            StartCoroutine(Reload());
        }
        else
        {
            Debug.Log("Reload already in progress");
        }
    }

    private IEnumerator Reload()
    {
        shotgunData.reloading = true;
        Debug.Log("Reloading started...");
        yield return new WaitForSeconds(shotgunData.reloadTime);
        shotgunData.currentAmmo = shotgunData.maxAmmo;
        ammoText.text = shotgunData.currentAmmo.ToString() + slash + shotgunData.maxAmmo.ToString();
        shotgunData.reloading = false;
        Debug.Log("Reloading complete!");
    }

    private bool canShoot()
    {
        float requiredTimeBetweenShots = 1f / (shotgunData.fireRate / 60f);
        Debug.Log($"Reloading: {shotgunData.reloading}, Time Since Last Shot: {timeSinceLastShot}, Required Time: {requiredTimeBetweenShots}");
        return !shotgunData.reloading && timeSinceLastShot > requiredTimeBetweenShots;
    }
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
                    ammoText.text = shotgunData.currentAmmo.ToString() + slash + shotgunData.maxAmmo.ToString();
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
