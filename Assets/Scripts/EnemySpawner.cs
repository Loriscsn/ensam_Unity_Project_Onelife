using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab de l'ennemi
    public Transform player; // R�f�rence � la position du joueur
    public float initialSpawnDelay = 10f; // D�lai initial avant la premi�re apparition
    public Vector2 spawnIntervalRange = new Vector2(10f, 25f); // Intervalle entre chaque vague d'ennemis
    private int initialEnemyCount; // Nombre initial d'ennemis pour la premi�re vague
    private List<GameObject> activeEnemies = new List<GameObject>(); // Liste des ennemis actifs
    private int waveMultiplier = 1; // Multiplicateur de vague pour chaque nouvelle vague

    // Nouveau champ pour le clip audio
    public AudioClip spawnSound; // Clip audio � jouer lors de l'apparition d'un ennemi
    private AudioSource audioSource; // R�f�rence � l'AudioSource

    // Nouveau champ pour le volume du son
    [Range(0f, 1f)] // Limiter la valeur entre 0 et 1 dans l'inspecteur
    public float spawnSoundVolume = 1f; // Volume du son d'apparition

    public float spawnRadius = 15f; // Rayon autour du joueur pour la zone de spawn
    public float minDistanceBetweenEnemies = 5f; // Distance minimale entre les ennemis

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Ajouter un AudioSource � l'EnemySpawner
        audioSource.clip = spawnSound; // Assigner le clip audio

        initialEnemyCount = Random.Range(1, 5); // Nombre d'ennemis pour la premi�re vague
        StartCoroutine(SpawnEnemiesAfterDelay(initialSpawnDelay, initialEnemyCount)); // Premi�re apparition
    }

    IEnumerator SpawnEnemiesAfterDelay(float delay, int enemyCount)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemies(enemyCount);
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        while (true)
        {
            // Attendre un intervalle al�atoire entre les vagues
            float spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(spawnInterval);

            // Compter les ennemis actifs
            activeEnemies.RemoveAll(enemy => enemy == null); // Retirer les ennemis morts de la liste

            int enemiesToSpawn;
            if (activeEnemies.Count == 0)
            {
                enemiesToSpawn = initialEnemyCount * waveMultiplier * 2; // Double des ennemis initiaux
            }
            else
            {
                enemiesToSpawn = Mathf.CeilToInt(initialEnemyCount * waveMultiplier * 0.5f); // 50% des ennemis initiaux
            }

            // Incr�menter le multiplicateur de vague pour la prochaine vague
            waveMultiplier++;
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

            // Jouer le son pour le premier fant�me seulement
            if (i == 0) // V�rifie si c'est le premier ennemi de la vague
            {
                audioSource.PlayOneShot(spawnSound, spawnSoundVolume); // Joue le son d'apparition avec le volume sp�cifi�
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
            // G�n�re une position al�atoire autour du joueur
            spawnPosition = GetRandomSpawnPositionAroundPlayer();
            validPosition = true;

            // V�rifie que la nouvelle position respecte la distance minimale entre chaque ennemi
            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy != null && Vector3.Distance(spawnPosition, enemy.transform.position) < minDistanceBetweenEnemies)
                {
                    validPosition = false;
                    break;
                }
            }
        }
        while (!validPosition); // Continue jusqu'� trouver une position valide

        return spawnPosition;
    }

    Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        return new Vector3(player.position.x + randomPos.x, 0, player.position.z + randomPos.y);
    }
}
