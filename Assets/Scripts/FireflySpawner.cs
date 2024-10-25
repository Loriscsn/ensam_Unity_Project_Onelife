using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab; // Référence au prefab de lucioles
    public int numberOfFireflies = 50; // Nombre de lucioles à générer
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
