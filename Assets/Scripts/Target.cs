using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 10f;

    public void Damage(float damage)
    {
        Debug.Log($"Taking damage: {damage}");
        health -= damage;
        Debug.Log($"Remaining health: {health}"); // Log the updated health
        if (health <= 0)
        {
            Debug.Log("Target destroyed");
            Destroy(gameObject);
        }
    }
}


