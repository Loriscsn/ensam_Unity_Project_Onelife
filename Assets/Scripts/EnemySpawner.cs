using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab de l'ennemi
    public float spawnInterval = 5f; // Intervalle entre chaque groupe d'ennemis
    private PickUpDrop pickUpDrop; // R�f�rence au gestionnaire de la torche
    private bool hasTorch = false; // V�rifie si la torche a �t� ramass�e
    private bool spawningEnemies = false; // Indique si nous sommes en train de faire appara�tre des ennemis

    void Start()
    {
        // Trouver le gestionnaire de la torche dans la sc�ne
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        // D�marrer la coroutine de v�rification de l'�tat de la torche
        StartCoroutine(CheckTorchState());
    }

    IEnumerator CheckTorchState()
    {
        while (true)
        {
            // V�rifier si la torche a �t� ramass�e
            if (pickUpDrop.IsHoldingTorch())
            {
                hasTorch = true; // La torche est dans la main
            }
            else if (hasTorch && !spawningEnemies)
            {
                // La torche a �t� l�ch�e, commence � faire appara�tre les ennemis
                StartCoroutine(SpawnEnemies());
                hasTorch = false; // R�initialiser pour �viter de red�marrer la g�n�ration
            }

            // Attendre avant de v�rifier � nouveau
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator SpawnEnemies()
    {
        spawningEnemies = true; // Indiquer que nous commen�ons � faire appara�tre des ennemis

        for (int i = 0; i < 5; i++) // Faire appara�tre 5 ennemis
        {
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(0.5f); // Pause entre chaque apparition d'ennemi
        }

        // Attendre avant de permettre de g�n�rer un autre groupe d'ennemis
        yield return new WaitForSeconds(spawnInterval);
        spawningEnemies = false; // R�initialiser l'indicateur pour permettre la prochaine g�n�ration
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Logique pour g�n�rer une position al�atoire pour l'ennemi
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z); // Position y = 0 pour la g�n�ration
    }
}
