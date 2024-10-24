using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Durée pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Durée pour passer de 50% à 0% d'alpha
    private Renderer enemyRenderer;          // Référence au Renderer de l'ennemi

    // Liste des couleurs possibles
    private Color[] possibleColors = {
        Color.white,                         // Blanc
        new Color(0.5f, 0f, 0.5f),           // Violet (valeur RGB pour un violet)
        new Color(1f, 0.41f, 0.71f),         // Rose (valeur RGB pour un rose)
        Color.black                          // Noir
    };

    void Start()
    {
        // Récupérer le Renderer pour manipuler la couleur
        enemyRenderer = GetComponent<Renderer>();

        // Assigner une couleur aléatoire à l'ennemi
        AssignRandomColor();
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur entre en collision avec l'ennemi
        if (other.CompareTag("Player"))
        {
            // Démarrer le fade-out
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    // Méthode pour assigner une couleur aléatoire
    void AssignRandomColor()
    {
        // Choisir une couleur au hasard parmi les couleurs possibles
        Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

        // Appliquer la couleur à l'ennemi
        enemyRenderer.material.color = randomColor;
    }

    IEnumerator FadeOutAndDestroy()
    {
        // Désactiver le collider pour éviter d'autres collisions
        GetComponent<Collider>().enabled = false;

        Color enemyColor = enemyRenderer.material.color; // Couleur actuelle de l'ennemi

        // Première phase : de 100% à 50% d'alpha en firstPhaseDuration secondes
        float elapsedTime = 0f;
        while (elapsedTime < firstPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / firstPhaseDuration);

            // Alpha passe de 1 (plein) à 0.5 (moitié)
            enemyColor.a = Mathf.Lerp(1f, 0.5f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        // Deuxième phase : de 50% à 0% d'alpha en secondPhaseDuration secondes
        elapsedTime = 0f;  // Réinitialiser le temps écoulé
        while (elapsedTime < secondPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / secondPhaseDuration);

            // Alpha passe de 0.5 à 0
            enemyColor.a = Mathf.Lerp(0.5f, 0f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        // Assurez-vous que l'ennemi est complètement transparent avant de le détruire
        enemyColor.a = 0f;
        enemyRenderer.material.color = enemyColor; // S'assurer que l'alpha est 0

        // Détruire l'ennemi après le fade-out
        Destroy(gameObject);
    }
}
