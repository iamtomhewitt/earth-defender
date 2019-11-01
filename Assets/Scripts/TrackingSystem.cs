using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TurretFiringSystem))]
public class TrackingSystem : MonoBehaviour 
{
    public List<GameObject> enemiesInRange;

    public GameObject target;

    [Space()]
    public GameObject body;
    public GameObject barrell;

    public float rotationSpeed;

    Vector3 lastKnownPosition = Vector3.zero;

    Quaternion lookAtRotation;
    Quaternion barrellStartQuaternion;

    void Start()
    {
        barrellStartQuaternion = barrell.transform.localRotation;

        InvokeRepeating("FireAtFirstEnemy", 1f, GetComponent<TurretSettings>().fireRate);
    }

    void Update()
    {
        if (enemiesInRange.Count > 0)
            target = enemiesInRange[0];
        
        // If we have a target
        if (target)
        {
            // Body rotation calculation
            float targetDistance = Vector3.Dot(transform.up, target.transform.position - transform.position);
            Vector3 point = target.transform.position - transform.up * targetDistance;
            Quaternion qTurret = Quaternion.LookRotation(point - transform.position, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTurret, rotationSpeed * Time.deltaTime);

            // Barrell rotation calculation
            Vector3 vector = new Vector3(0.0f, targetDistance, (point - transform.position).magnitude);
            Quaternion qGun = Quaternion.LookRotation(vector);
            barrell.transform.localRotation = Quaternion.RotateTowards(barrell.transform.localRotation, qGun, rotationSpeed * Time.deltaTime);            
        }

        // If there are no enemies in range, then we cannot have a target.
        if (enemiesInRange.Count == 0)
        {
            target = null;
        }

        // If we don't have a target
        if (!target)
        {
            // And there are enemies in range
            if (enemiesInRange.Count > 0)
            {
                // Check if we have a first null reference.
                if (enemiesInRange[0] == null) // For some reason need to do this check.
                    RemoveNullEnemies();

                // Then make the first list entry the new target.
                else
                {
                    target = enemiesInRange[0];
                    print("Assigned New Target.");
                }
            }
            // Else we have no enemies in range, therefore no targets.
            else
            {
                target = null;
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void RemoveNullEnemies()
    {
        for (int i = enemiesInRange.Count - 1; i > -1; i--)
        {
            if (enemiesInRange[i] == null)
                enemiesInRange.RemoveAt(i);
        }
    }

    public void RemoveEnemy(GameObject e)
    {
        enemiesInRange.Remove(e);
    }

    void FireAtFirstEnemy()
    {
        if (target)
            GetComponent<TurretFiringSystem>().Shoot(target);
    }
}
