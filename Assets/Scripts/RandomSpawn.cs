using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{

    public GameObject terrainObject;
    public GameObject planet;
    public int amountToSpawn = 10;
    // Start is called before the first frame update
    void Start()
    {
        float planetRadius = planet.GetComponent<SphereCollider>().radius * planet.transform.localScale.x; // Get the scaled radius of the planet
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPoint = Random.onUnitSphere * planetRadius; // Get a random point on the surface of the planet
            GameObject trees = Instantiate(terrainObject, spawnPoint, Quaternion.identity) as GameObject; // Create a terrain object at the random point
            trees.transform.LookAt(planet.transform.position); // Look at the planet        
            trees.transform.rotation = trees.transform.rotation * Quaternion.Euler(-90, 0, 0); // Rotate the terrain object to be upright
        }
    }


}
