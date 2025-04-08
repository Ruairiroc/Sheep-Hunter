using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerGravity : MonoBehaviour
{
    WorldGravity planet;
    void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<WorldGravity>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

    }

    void FixedUpdate()
    {
        planet.ApplyGravity(transform);
    }
}
