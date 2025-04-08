using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SheepController : MonoBehaviour
{


    public float speed = 5f;
    public float rotationSpeed = 5f;

    public GameObject target;
    public float distanceToTarget = 1f;

    public GameObject planet;

    void Start()
    {


        target = GameObject.FindGameObjectWithTag("Player");
        if (target == null)
        {
            Debug.LogError("SheepController: Target not found!");
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Rotate the sheep smoothly toward the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the sheep toward the target if it's farther than the specified distance
            if (Vector3.Distance(transform.position, target.transform.position) > distanceToTarget)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            // Keep the sheep on the surface of the planet
            Vector3 toPlanetCenter = (transform.position - planet.transform.position).normalized;
            float planetRadius = planet.GetComponent<SphereCollider>().radius * planet.transform.localScale.x;
            transform.position = planet.transform.position + toPlanetCenter * planetRadius;

            // Align the sheep's "up" direction with the planet's surface normal
            transform.rotation = Quaternion.FromToRotation(transform.up, toPlanetCenter) * transform.rotation;
        }
    }
}
