using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab de l'ennemi � faire appara�tre
    public int numberOfEnemies = 5; // Nombre d'ennemis � faire appara�tre
    public float spawnRadius = 10f; // Rayon de spawn des ennemis autour de la torche
    public float attractionSpeed = 5f; // Vitesse � laquelle les ennemis sont attir�s vers la torche
    public float detectionDelay = 1f;  // D�lai avant que les ennemis ne d�tectent la torche
    public GameObject player;  // R�f�rence au joueur (Cube)
    public float distanceThreshold = 5f;  // Distance minimale pour poser la torche

    private bool isTorchPlaced = false;  // Pour savoir si la torche est "pos�e"
    private bool isTorchPickedUp = false;  // Pour savoir si la torche est r�cup�r�e par le joueur
    private GameObject[] spawnedEnemies;  // Tableau des ennemis g�n�r�s

    void Update()
    {
        // V�rifier la distance entre la torche (cylinder) et le joueur (cube)
        if (!isTorchPlaced && IsPlayerFarEnough())
        {
            PlaceTorch();  // Si la distance est suffisante, poser la torche
        }

        // V�rifier si la torche est r�cup�r�e et que les ennemis sont pr�sents
        if (isTorchPickedUp)
        {
            CheckEnemiesTouched();  // V�rifier si le joueur touche les ennemis
        }
    }

    // M�thode pour v�rifier si le joueur est � plus de 5 m�tres de la torche
    private bool IsPlayerFarEnough()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance > distanceThreshold;
    }

    // Cette m�thode place la torche au sol et fait appara�tre les ennemis
    public void PlaceTorch()
    {
        if (!isTorchPlaced)
        {
            isTorchPlaced = true;  // Marquer la torche comme plac�e
            StartCoroutine(SpawnEnemies());  // D�clencher le spawn des ennemis
        }
    }

    // M�thode pour r�cup�rer la torche
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && isTorchPlaced)
        {
            isTorchPickedUp = true;  // Le joueur a r�cup�r� la torche
            isTorchPlaced = false;   // La torche n'est plus pos�e
        }
    }

    // Coroutine pour g�rer l'apparition des ennemis apr�s un d�lai
    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(detectionDelay);  // Attendre un d�lai avant le spawn

        spawnedEnemies = new GameObject[numberOfEnemies];  // Initialiser le tableau des ennemis

        // Faire appara�tre plusieurs ennemis autour de la torche
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

    // V�rifier si le joueur touche un ennemi
    private void CheckEnemiesTouched()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (distanceToPlayer < 1.5f)  // Si l'ennemi est proche du joueur (1.5 m�tre ici)
                {
                    Destroy(enemy);  // D�truire l'ennemi quand le joueur le touche
                }
            }
        }
    }
}