using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public List<EnemyConfig> possibleEnemies;
    public float spawnInterval = 2;
    public int maxEnemies = 20;

    public GameObject SpawnRandomEnemy()
    {
        return SpawnEnemy(Random.Range(0, possibleEnemies.Count));
    }

    public GameObject SpawnEnemy(int enemyIndex)
    {
        GameObject spawnedEnemy = GameObject.Instantiate(enemyPrefab);
        EnemyConfig enemyConfig = possibleEnemies[enemyIndex];

        spawnedEnemy.transform.Find("Sprite").GetComponent<Animator>().runtimeAnimatorController = enemyConfig.animatorController;
        spawnedEnemy.transform.Find("HealthBar").GetComponent<HealthBar>().maxHealth = enemyConfig.health;
        spawnedEnemy.transform.GetComponent<MoveEnemy>().speed = enemyConfig.speed;

        return spawnedEnemy;
    }
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject testEnemyPrefab;
    public Wave[] waves;
    public int timeBetweenWaves = 5;

    private GameManagerBehaviour gameManager;

    private float lastSpawnTime;
    private int enemiesSpawned = 0;

    private void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    private void Update()
    {
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;
            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || (enemiesSpawned != 0 && timeInterval > spawnInterval)) &&
            (enemiesSpawned < waves[currentWave].maxEnemies))
            {
                lastSpawnTime = Time.time;
                GameObject newEnemy = waves[currentWave].SpawnRandomEnemy();
                newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                enemiesSpawned++;
            }
            if (enemiesSpawned == waves[currentWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
}
