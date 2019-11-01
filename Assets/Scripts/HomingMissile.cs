using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour 
{
    public float speed = 5f;
    public float turningSpeed = 20f;

    public GameObject missileModel;
    public GameObject sparks;
	
    public ParticleSystem smoke;

    [Tooltip("Can be set or can be null.")]
    public Transform target;

    GameObject smokeParent;

    Rigidbody rb;

    void Start () 
    {
        smokeParent = GameObject.Find("Smokes");
        StartCoroutine(DestroyAfterLifetime(15f));

        rb = GetComponent<Rigidbody>();

        GameObject.FindObjectOfType<AudioManager>().PlayMissileLaunch();
    }


    void FixedUpdate ()
    {
        // A target may be null if another homing missile has already shot it down
        if (target == null)
        {
            StartCoroutine(StopSmokeAndDestroy());
            return;
        }
            
        HomeIn();
    }
        

    void HomeIn()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Look at our target
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

        // Rotate towrdsa our target
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed));
    }


    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                target = null;

                Destroy(GetComponent<Collider>());   

                StartCoroutine(StopSmokeAndDestroy());
                break;
        }
    }


    void OnDestroy()
    {
        GameObject s = Instantiate(sparks, transform.position, transform.rotation) as GameObject;
        Destroy(s, 5f);
    }

    IEnumerator StopSmokeAndDestroy()
    {
        SetSmokeEmissionRate(0f);
        smoke.transform.parent = smokeParent.transform;
        Destroy(missileModel);
        yield return new WaitForSeconds(.1f);
        Destroy(this.gameObject);
    }


    float GetSmokeEmissionRate()
    {
        return smoke.emission.rate.constantMax;
    }


    public void SetSmokeEmissionRate(float emissionRate)
    {
        ParticleSystem.EmissionModule emission = smoke.emission;
        ParticleSystem.MinMaxCurve rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }


    IEnumerator DestroyAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        StartCoroutine(StopSmokeAndDestroy());
    }
}
