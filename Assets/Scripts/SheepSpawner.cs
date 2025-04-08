using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{

    public GameObject sheepObject;
    public GameObject planet;

    public int amountToSpawn;
    public float spawnHeight = 10f;

    SheepController sheepController;
    // Start is called before the first frame update
    void Start()
    {
        float planetRadius = planet.GetComponent<SphereCollider>().radius * planet.transform.localScale.x; // Get the scaled radius of the planet
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 surfacePoint = planet.transform.position + Random.onUnitSphere * planetRadius;

            // Offset the spawn point by the spawnHeight above the surface
            Vector3 spawnPoint = surfacePoint + (surfacePoint - planet.transform.position).normalized * spawnHeight; GameObject sheep = Instantiate(sheepObject, spawnPoint, Quaternion.identity); // Create a terrain object at the random point
            sheep.transform.LookAt(planet.transform.position); // Look at the planet        
            Vector3 normal = (spawnPoint - planet.transform.position).normalized;
            sheep.transform.rotation = Quaternion.LookRotation(Vector3.forward, normal); SheepController sheepController = sheep.GetComponent<SheepController>();
            if (sheepController == null)
            {
                sheepController = sheep.AddComponent<SheepController>();
            }

            // Assign public variables to the SheepController
            sheepController.target = GameObject.FindGameObjectWithTag("Player");
            sheepController.speed = 5f; // Example speed value
            sheepController.rotationSpeed = 5f; // Example rotation speed value
            sheepController.distanceToTarget = 1f; // Example distance to target value        }
        }


    }
}
