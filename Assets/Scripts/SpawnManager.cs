using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;

    public float spawnRange = 9.0f;
    public int enemyCount = 0;
    public int waveNumber = 5;


    public GameObject[] miniEnemyPrefabs;
    public GameObject[] bossPrefabs;
    public int bossWaveIntervall = 5;
    //wenn mehr verschiedene Bosse
    public int bossNumber = 0;

    public TextMeshProUGUI waveText;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        SpawnEnemyWave(waveNumber);
       //InvokeRepeating("SpawnEnemy", 0, 2);
       waveText.text = $"Wave: {waveNumber}";
    }

    // Update is called once per frame
    void Update()
    {
       
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0)
        {
            StartCoroutine(Delay(2));
            waveNumber++;
            waveText.text = $"Wave: {waveNumber}";

            if (waveNumber % 2 == 0)
            {
                SpawnPowerUp();
            }
            if (waveNumber % bossWaveIntervall == 0)
            {
                //BOSSE
                GameObject boss = bossPrefabs[Random.Range(0, bossPrefabs.Length)];
                Vector3 spawnBossPos = GenerateSpawnPosition();
                Instantiate(boss, spawnBossPos + new Vector3(0,2,0), boss.transform.rotation);
            }
            else
            {
                SpawnEnemyWave(waveNumber);
            }
        }
       
    }
    
    public void SpawnMiniEnemy()
    {
        GameObject miniEnemyPrefab = miniEnemyPrefabs[Random.Range(0, miniEnemyPrefabs.Length)];
        Vector3 miniEnemySpawnPos = GenerateSpawnPosition();
        Instantiate(miniEnemyPrefab, miniEnemySpawnPos, miniEnemyPrefab.transform.rotation);
    }
    public void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 spawnPos = GenerateSpawnPosition();
       Instantiate(enemyPrefab, spawnPos , enemyPrefab.transform.rotation);
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return spawnPos;
    }

    private void SpawnPowerUpWave(int powerUpNumber)
    {
        for (int i = 0; i < powerUpNumber; i++)
        {
            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length)];
        Vector3 powerUpPos = GenerateSpawnPosition();
        Instantiate(powerUpPrefab, powerUpPos, powerUpPrefab.gameObject.transform.rotation);
    }

    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
