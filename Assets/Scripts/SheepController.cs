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
    private bool hasCollidedWithPlayer = false; // Flag to track collision with the player


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
        if (target != null && !hasCollidedWithPlayer)
        {

            float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToPlayer > distanceToTarget)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }



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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollidedWithPlayer = true; // Stop the sheep's movement
            Debug.Log("Sheep collided with player!");
            StartCoroutine(ResumeChasingAfterDelay(2f)); // Wait for 2 seconds before resuming
        }
    }

    private IEnumerator ResumeChasingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasCollidedWithPlayer = false; // Resume chasing the player
        Debug.Log("Sheep resumed chasing the player!");
    }


}
