using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    private FarmerGameController controls;

    private void Awake()
    {
        controls = new FarmerGameController();

        controls.Gameplay.Shoot.performed += ctx =>
        {
            Debug.Log("Shoot action performed");
            shootInput?.Invoke();
        };
        controls.Gameplay.Reload.performed += ctx =>
        {
            Debug.Log("Reload action performed");
            reloadInput?.Invoke();
        };
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
