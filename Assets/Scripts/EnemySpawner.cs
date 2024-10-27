using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab de l'ennemi
    public Transform player; // Référence à la position du joueur

    public float initialSpawnDelay = 10f; // Délai initial avant la première apparition
    public float spawnRadius = 15f; // Rayon autour du joueur pour la zone de spawn
    public float minDistanceBetweenEnemies = 5f; // Distance minimale entre les ennemis

    // Paramètres de spawn par intervalle de temps
    [Header("Time Interval Settings")]
    public float interval1Time = 60f; // Première minute
    public Vector2 interval1EnemyRange = new Vector2(1, 2);
    public Vector2 interval1SpawnRate = new Vector2(10f, 25f);

    public float interval2Time = 120f; // Deuxième minute
    public Vector2 interval2EnemyRange = new Vector2(1, 3);
    public Vector2 interval2SpawnRate = new Vector2(10f, 25f);

    public float interval3Time = 180f; // Troisième minute
    public Vector2 interval3EnemyRange = new Vector2(2, 3);
    public Vector2 interval3SpawnRate = new Vector2(10f, 25f);

    public float interval4Time = 240f; // Quatrième minute
    public Vector2 interval4EnemyRange = new Vector2(2, 4);
    public Vector2 interval4SpawnRate = new Vector2(10f, 25f);

    public float interval5Time = 300f; // Cinquième minute
    public Vector2 interval5EnemyRange = new Vector2(3, 4);
    public Vector2 interval5SpawnRate = new Vector2(10f, 25f);

    public float interval6Time = 360f; // Sixième minute
    public Vector2 interval6EnemyRange = new Vector2(3, 5);
    public Vector2 interval6SpawnRate = new Vector2(35f, 45f);

    public float interval7Time = 480f; // Huitième minute
    public Vector2 interval7EnemyRange = new Vector2(4, 5);
    public Vector2 interval7SpawnRate = new Vector2(20f, 35f);

    public float interval8Time = 600f; // Dixième minute
    public Vector2 interval8EnemyRange = new Vector2(2, 4);
    public Vector2 interval8SpawnRate = new Vector2(10f, 20f);

    // Nouveau champ pour le clip audio
    public AudioClip spawnSound; // Clip audio à jouer lors de l'apparition d'un ennemi
    private AudioSource audioSource; // Référence à l'AudioSource

    [Range(0f, 1f)]
    public float spawnSoundVolume = 1f; // Volume du son d'apparition

    private List<GameObject> activeEnemies = new List<GameObject>(); // Liste des ennemis actifs

    // Variables pour le nombre d'ennemis et l'intervalle de spawn actuel
    private int initialEnemyMinCount;
    private int initialEnemyMaxCount;
    private Vector2 spawnIntervalRange;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = spawnSound;

        StartCoroutine(SpawnEnemiesAfterDelay(initialSpawnDelay)); // Démarre le spawn après un délai
    }

    IEnumerator SpawnEnemiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        while (true)
        {
            UpdateSpawnParameters(); // Met à jour les paramètres en fonction du temps de jeu

            // Intervalle de spawn aléatoire selon la plage de temps actuelle
            float spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(spawnInterval);

            // Compte les ennemis actifs et nettoie la liste des nulls
            activeEnemies.RemoveAll(enemy => enemy == null);

            // Calcule un nombre aléatoire d'ennemis à spawn
            int enemiesToSpawn = Random.Range(initialEnemyMinCount, initialEnemyMaxCount + 1);
            SpawnEnemies(enemiesToSpawn);
        }
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);

            // Jouer le son pour le premier ennemi seulement
            if (i == 0)
            {
                audioSource.PlayOneShot(spawnSound, spawnSoundVolume);
            }
        }
        Debug.Log("Vague de " + count + " ennemis apparue.");
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        bool validPosition;

        do
        {
            // Génère une position aléatoire autour du joueur
            spawnPosition = GetRandomSpawnPositionAroundPlayer();
            validPosition = true;

            // Vérifie que la nouvelle position respecte la distance minimale
            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy != null && Vector3.Distance(spawnPosition, enemy.transform.position) < minDistanceBetweenEnemies)
                {
                    validPosition = false;
                    break;
                }
            }
        }
        while (!validPosition);

        return spawnPosition;
    }

    Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        return new Vector3(player.position.x + randomPos.x, 0, player.position.z + randomPos.y);
    }

    void UpdateSpawnParameters()
    {
        // Récupère le temps écoulé depuis le début du jeu
        float timeElapsed = Time.timeSinceLevelLoad;

        // Met à jour les paramètres de spawn en fonction du temps de jeu
        if (timeElapsed < interval1Time)
        {
            initialEnemyMinCount = (int)interval1EnemyRange.x;
            initialEnemyMaxCount = (int)interval1EnemyRange.y;
            spawnIntervalRange = interval1SpawnRate;
        }
        else if (timeElapsed < interval2Time)
        {
            initialEnemyMinCount = (int)interval2EnemyRange.x;
            initialEnemyMaxCount = (int)interval2EnemyRange.y;
            spawnIntervalRange = interval2SpawnRate;
        }
        else if (timeElapsed < interval3Time)
        {
            initialEnemyMinCount = (int)interval3EnemyRange.x;
            initialEnemyMaxCount = (int)interval3EnemyRange.y;
            spawnIntervalRange = interval3SpawnRate;
        }
        else if (timeElapsed < interval4Time)
        {
            initialEnemyMinCount = (int)interval4EnemyRange.x;
            initialEnemyMaxCount = (int)interval4EnemyRange.y;
            spawnIntervalRange = interval4SpawnRate;
        }
        else if (timeElapsed < interval5Time)
        {
            initialEnemyMinCount = (int)interval5EnemyRange.x;
            initialEnemyMaxCount = (int)interval5EnemyRange.y;
            spawnIntervalRange = interval5SpawnRate;
        }
        else if (timeElapsed < interval6Time)
        {
            initialEnemyMinCount = (int)interval6EnemyRange.x;
            initialEnemyMaxCount = (int)interval6EnemyRange.y;
            spawnIntervalRange = interval6SpawnRate;
        }
        else if (timeElapsed < interval7Time)
        {
            initialEnemyMinCount = (int)interval7EnemyRange.x;
            initialEnemyMaxCount = (int)interval7EnemyRange.y;
            spawnIntervalRange = interval7SpawnRate;
        }
        else
        {
            initialEnemyMinCount = (int)interval8EnemyRange.x;
            initialEnemyMaxCount = (int)interval8EnemyRange.y;
            spawnIntervalRange = interval8SpawnRate;
        }
    }
}
