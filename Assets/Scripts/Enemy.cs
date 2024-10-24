using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Dur�e pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Dur�e pour passer de 50% � 0% d'alpha
    private Renderer enemyRenderer;          // R�f�rence au Renderer de l'ennemi

    // Liste des couleurs possibles
    private Color[] possibleColors = {
        Color.white,                         // Blanc
        new Color(0.5f, 0f, 0.5f),           // Violet (valeur RGB pour un violet)
        new Color(1f, 0.41f, 0.71f),         // Rose (valeur RGB pour un rose)
        Color.black                          // Noir
    };

    void Start()
    {
        // R�cup�rer le Renderer pour manipuler la couleur
        enemyRenderer = GetComponent<Renderer>();

        // Assigner une couleur al�atoire � l'ennemi
        AssignRandomColor();
    }

    void OnTriggerEnter(Collider other)
    {
        // V�rifier si le joueur entre en collision avec l'ennemi
        if (other.CompareTag("Player"))
        {
            // D�marrer le fade-out
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    // M�thode pour assigner une couleur al�atoire
    void AssignRandomColor()
    {
        // Choisir une couleur au hasard parmi les couleurs possibles
        Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

        // Appliquer la couleur � l'ennemi
        enemyRenderer.material.color = randomColor;
    }

    IEnumerator FadeOutAndDestroy()
    {
        // D�sactiver le collider pour �viter d'autres collisions
        GetComponent<Collider>().enabled = false;

        Color enemyColor = enemyRenderer.material.color; // Couleur actuelle de l'ennemi

        // Premi�re phase : de 100% � 50% d'alpha en firstPhaseDuration secondes
        float elapsedTime = 0f;
        while (elapsedTime < firstPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / firstPhaseDuration);

            // Alpha passe de 1 (plein) � 0.5 (moiti�)
            enemyColor.a = Mathf.Lerp(1f, 0.5f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        // Deuxi�me phase : de 50% � 0% d'alpha en secondPhaseDuration secondes
        elapsedTime = 0f;  // R�initialiser le temps �coul�
        while (elapsedTime < secondPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / secondPhaseDuration);

            // Alpha passe de 0.5 � 0
            enemyColor.a = Mathf.Lerp(0.5f, 0f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        // Assurez-vous que l'ennemi est compl�tement transparent avant de le d�truire
        enemyColor.a = 0f;
        enemyRenderer.material.color = enemyColor; // S'assurer que l'alpha est 0

        // D�truire l'ennemi apr�s le fade-out
        Destroy(gameObject);
    }
}
