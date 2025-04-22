using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "New Shotgun", menuName = "Weapon/Shotgun")]

public class ShotgunData : ScriptableObject
{
    [Header("Weapon Info")]
    public new string name;

    [Header("Shooting")]

    public float damage;
    public float range;

    [Header("Reloading")]

    public int currentAmmo;
    public int maxAmmo;
    public float spread = 0.1f;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;


}
