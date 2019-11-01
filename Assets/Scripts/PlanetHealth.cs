using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHealth : MonoBehaviour 
{
    public float health;

    public Image healthBar;

    public GameObject explosion;

    public void DecreaseHealth(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / 100f;

        GameObject.FindObjectOfType<AudioManager>().loseHealth.Play();

        if (health <= 0)
        {
            GameObject.FindObjectOfType<AudioManager>().planetExplosion.Play();

            Destroy(this.gameObject);

            Instantiate(explosion, transform.position, Quaternion.identity);

            if (this.gameObject.name == "Earth")
            {
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.canSpawn = false;
                gameManager.ShowGameOver();
            }
        }
    }
}
