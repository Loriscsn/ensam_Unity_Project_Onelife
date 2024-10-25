using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab; // R�f�rence au prefab de lucioles
    public int numberOfFireflies = 50; // Nombre de lucioles � g�n�rer
    public float spawnRadius = 10f; // Rayon de spawn

    void Start()
    {
        for (int i = 0; i < numberOfFireflies; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            Instantiate(fireflyPrefab, randomPosition, Quaternion.identity);
        }
    }
}
