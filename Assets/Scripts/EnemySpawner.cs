using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab de l'ennemi
    public float spawnInterval = 5f; // Intervalle entre chaque groupe d'ennemis
    private PickUpDrop pickUpDrop; // Référence au gestionnaire de la torche
    private bool hasTorch = false; // Vérifie si la torche a été ramassée
    private bool spawningEnemies = false; // Indique si nous sommes en train de faire apparaître des ennemis

    void Start()
    {
        // Trouver le gestionnaire de la torche dans la scène
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        // Démarrer la coroutine de vérification de l'état de la torche
        StartCoroutine(CheckTorchState());
    }

    IEnumerator CheckTorchState()
    {
        while (true)
        {
            // Vérifier si la torche a été ramassée
            if (pickUpDrop.IsHoldingTorch())
            {
                hasTorch = true; // La torche est dans la main
            }
            else if (hasTorch && !spawningEnemies)
            {
                // La torche a été lâchée, commence à faire apparaître les ennemis
                StartCoroutine(SpawnEnemies());
                hasTorch = false; // Réinitialiser pour éviter de redémarrer la génération
            }

            // Attendre avant de vérifier à nouveau
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnEnemies()
    {
        spawningEnemies = true; // Indiquer que nous commençons à faire apparaître des ennemis

        for (int i = 0; i < 5; i++) // Faire apparaître 5 ennemis
        {
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(0.5f); // Pause entre chaque apparition d'ennemi
        }

        // Attendre avant de permettre de générer un autre groupe d'ennemis
        yield return new WaitForSeconds(spawnInterval);
        spawningEnemies = false; // Réinitialiser l'indicateur pour permettre la prochaine génération
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Logique pour générer une position aléatoire pour l'ennemi
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z); // Position y = 0 pour la génération
    }
}
