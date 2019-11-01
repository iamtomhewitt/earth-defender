using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    [Header("Wave Settings")]
    public int waveSize = 5;
    [Tooltip("How much the wave size increases after each wave.")]
    public int waveIncrement;

    [Header("Spawn Settings")]
    public GameObject[] enemies;
    public GameObject[] capitalShips;
    public float spawnGap;
    public float capitalShipRepeatRate;
    public Vector3 boundary;

    [Header("Other Settings")]
    public bool canSpawn;

    public GameObject ui;
    public GameObject gameOver;

    public Text nextWaveText;
    public Text gameTimeText;

    float gameTime = 0f;

    void Start()
    {
        canSpawn = true;
        nextWaveText.text = "";
        SpawnWave();

        InvokeRepeating("SpawnCapitalShip", capitalShipRepeatRate, capitalShipRepeatRate);
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    void SpawnWave()
    {
        if (canSpawn)
            StartCoroutine(SpawnWaveCoroutine());
    }

    IEnumerator SpawnWaveCoroutine()
    {
        nextWaveText.text = "";

        for (int i = 0; i < waveSize; i++)
        {        
            //Vector3 pos = GenerateRandomPosition();//new Vector3(GenerateRandomNumber(), GenerateRandomNumber(), GenerateRandomNumber());
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            
            Instantiate(randomEnemy, GenerateRandomPosition(), Quaternion.identity);
            
            yield return new WaitForSeconds(spawnGap);
        }
            
        // Wait for x seconds after spawning the last enemy - maybe have a text component that shows this, and animates everytime the number switches (bigger to smaller)
        for (int i = 45; i > 0; i--)
        {
            nextWaveText.text = "Next Wave In " + i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Increase the wave size
        waveSize += waveIncrement;

        // Spawn a new wave
        SpawnWave();
    }

    void SpawnCapitalShip()
    {
        GameObject randomShip = capitalShips[Random.Range(0, capitalShips.Length)];

        Instantiate(randomShip, GenerateRandomPosition(), Quaternion.identity);
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 pos = Random.onUnitSphere * Random.Range(30, 60);
        return pos;
    }

    public void ShowGameOver()
    {
        ui.SetActive(false);
        gameOver.SetActive(true);
        gameTimeText.text = "You survived for " + gameTime.ToString("##.000") + " seconds!";
    }
}