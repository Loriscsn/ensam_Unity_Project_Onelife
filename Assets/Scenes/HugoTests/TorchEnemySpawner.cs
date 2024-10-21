using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab de l'ennemi à faire apparaître
    public int numberOfEnemies = 5; // Nombre d'ennemis à faire apparaître
    public float spawnRadius = 10f; // Rayon de spawn des ennemis autour de la torche
    public float attractionSpeed = 5f; // Vitesse à laquelle les ennemis sont attirés vers la torche
    public float detectionDelay = 1f;  // Délai avant que les ennemis ne détectent la torche
    public GameObject player;  // Référence au joueur (Cube)
    public float distanceThreshold = 5f;  // Distance minimale pour poser la torche

    private bool isTorchPlaced = false;  // Pour savoir si la torche est "posée"
    private bool isTorchPickedUp = false;  // Pour savoir si la torche est récupérée par le joueur
    private GameObject[] spawnedEnemies;  // Tableau des ennemis générés

    void Update()
    {
        // Vérifier la distance entre la torche (cylinder) et le joueur (cube)
        if (!isTorchPlaced && IsPlayerFarEnough())
        {
            PlaceTorch();  // Si la distance est suffisante, poser la torche
        }

        // Vérifier si la torche est récupérée et que les ennemis sont présents
        if (isTorchPickedUp)
        {
            CheckEnemiesTouched();  // Vérifier si le joueur touche les ennemis
        }
    }

    // Méthode pour vérifier si le joueur est à plus de 5 mètres de la torche
    private bool IsPlayerFarEnough()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance > distanceThreshold;
    }

    // Cette méthode place la torche au sol et fait apparaître les ennemis
    public void PlaceTorch()
    {
        if (!isTorchPlaced)
        {
            isTorchPlaced = true;  // Marquer la torche comme placée
            StartCoroutine(SpawnEnemies());  // Déclencher le spawn des ennemis
        }
    }

    // Méthode pour récupérer la torche
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && isTorchPlaced)
        {
            isTorchPickedUp = true;  // Le joueur a récupéré la torche
            isTorchPlaced = false;   // La torche n'est plus posée
        }
    }

    // Coroutine pour gérer l'apparition des ennemis après un délai
    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(detectionDelay);  // Attendre un délai avant le spawn

        spawnedEnemies = new GameObject[numberOfEnemies];  // Initialiser le tableau des ennemis

        // Faire apparaître plusieurs ennemis autour de la torche
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
            spawnPosition.y = 0;  // S'assurer que les ennemis apparaissent au niveau du sol

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies[i] = enemy;  // Stocker l'ennemi dans le tableau

            // Assigner un comportement pour attirer l'ennemi vers la torche
            EnemyAttraction enemyAttraction = enemy.AddComponent<EnemyAttraction>();
            enemyAttraction.SetTarget(transform, attractionSpeed);
        }
    }

    // Vérifier si le joueur touche un ennemi
    private void CheckEnemiesTouched()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (distanceToPlayer < 1.5f)  // Si l'ennemi est proche du joueur (1.5 mètre ici)
                {
                    Destroy(enemy);  // Détruire l'ennemi quand le joueur le touche
                }
            }
        }
    }
}