using UnityEngine;

public class GhostFloat : MonoBehaviour
{
    public float oscillationAmplitude = 0.5f; // Amplitude de l'oscillation sur l'axe Y
    public float oscillationFrequency = 1.0f;  // Fréquence de l'oscillation sur l'axe Y
    private float initialY; // Pour stocker la position Y de départ
    private float time; // Pour calculer le mouvement sinusoïdal

    void Start()
    {
        // Enregistrer la position Y de départ de l'ennemi
        initialY = transform.position.y;
    }

    void Update()
    {
        // Incrémenter le temps en fonction de la fréquence
        time += Time.deltaTime * oscillationFrequency;

        // Calculer l'offset sinusoïdal
        float yOffset = Mathf.Sin(time) * oscillationAmplitude;

        // Mettre à jour la position Y de l'ennemi
        Vector3 newPosition = transform.position;
        newPosition.y = initialY + yOffset; // Garder la position Y oscillante

        // Appliquer la nouvelle position tout en gardant la position X et Z
        transform.position += new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
