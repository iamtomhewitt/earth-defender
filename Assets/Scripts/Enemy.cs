using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public float speed;

    public int health;
    public int cashReward;

    public float damage;

    public GameObject explosion;

    GameObject earth;

    ShopManager shopManager;
    AudioManager audioManager;

    void Start()
    {
        earth = GameObject.Find("Earth");

        shopManager = GameObject.FindObjectOfType<ShopManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
	
	void Update () 
    {
        if (earth)
        {
            transform.LookAt(earth.transform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            DestroySelf();
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Planet")
        {
            Destroy(this.gameObject);

            DestroySelf();

            other.gameObject.GetComponent<PlanetHealth>().DecreaseHealth(damage);

            print("An enemy crashed into planet " + other.gameObject.name);
        }

        if (other.gameObject.tag == "Missile")
        {
            health--;

            GameObject.FindObjectOfType<AudioManager>().missileHit.Play();

            if (health <= 0)
            {
                shopManager.ChangeCashAmount(cashReward);

                Destroy(this.gameObject);

                DestroySelf();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Nuke")
        {
            DestroySelf();
            Destroy(this.gameObject);
        }
    }

    void DestroySelf()
    {
        audioManager.PlayExplosion();

        GameObject e = Instantiate(explosion, transform.position, transform.rotation) as GameObject;

        Destroy(e, 5f);

        // Tell all the turrets that the enemy is dead.
        TrackingSystem[] turretTrackingSystems = GameObject.FindObjectsOfType<TrackingSystem>();

        if (turretTrackingSystems == null)
            return;

        for (int i = 0; i < turretTrackingSystems.Length; i++)
        {
            turretTrackingSystems[i].RemoveEnemy(gameObject);
        }
    }
}
