using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] enemyExplosions;
    public AudioSource[] missiles;
    [Space()]
    public AudioSource planetExplosion;
    public AudioSource turretSpawn;
    public AudioSource missileHit;
    public AudioSource loseHealth;
    public AudioSource upgrade;

    public void PlayRandomSound(AudioSource[] arrayOfSounds)
    {
        int i = Random.Range(0, arrayOfSounds.Length);

        arrayOfSounds[i].Play();
    }

    public void PlaySound(AudioSource sound)
    {
        sound.Play();
    }

    public void PlayExplosion()
    {
        int i = Random.Range(0, enemyExplosions.Length);

        enemyExplosions[i].Play();
    }

    public void PlayMissileLaunch()
    {
        int i = Random.Range(0, missiles.Length);

        missiles[i].Play();
    }
}
