using UnityEngine;

public class GhostFloat : MonoBehaviour
{
    public float oscillationAmplitude = 0.5f; // Amplitude de l'oscillation sur l'axe Y
    public float oscillationFrequency = 1.0f;  // Fr�quence de l'oscillation sur l'axe Y
    private float initialY; // Pour stocker la position Y de d�part
    private float time; // Pour calculer le mouvement sinuso�dal

    void Start()
    {
        // Enregistrer la position Y de d�part de l'ennemi
        initialY = transform.position.y;
    }

    void Update()
    {
        // Incr�menter le temps en fonction de la fr�quence
        time += Time.deltaTime * oscillationFrequency;

        // Calculer l'offset sinuso�dal
        float yOffset = Mathf.Sin(time) * oscillationAmplitude;

        // Mettre � jour la position Y de l'ennemi
        Vector3 newPosition = transform.position;
        newPosition.y = initialY + yOffset; // Garder la position Y oscillante

        // Appliquer la nouvelle position tout en gardant la position X et Z
        transform.position += new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
