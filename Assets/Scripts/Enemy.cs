using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Durée pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Durée pour passer de 50% à 0% d'alpha
    private Renderer enemyRenderer;          // Référence au Renderer de l'ennemi
    Animator playerAnimator;
    bool attack = false;
    // Liste des couleurs possibles
    private Color[] possibleColors = {
        Color.white,                         // Blanc
        new Color(0.5f, 0f, 0.5f),           // Violet
        new Color(1f, 0.41f, 0.71f),         // Rose
        Color.black                          // Noir
    };

    public float interactionRange = 5f;  // Distance maximale pour cliquer sur l'ennemi
    private PickUpDrop pickUpDrop; // Référence au script PickUpDrop

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        // Récupérer le Renderer pour manipuler la couleur
        enemyRenderer = GetComponent<Renderer>();

        // Vérifiez si le Renderer est bien assigné
        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assigné !");
        }

        // Récupérer la référence au script PickUpDrop
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        // Assigner une couleur aléatoire à l'ennemi
        AssignRandomColor();
        Debug.Log("Ennemi instancié avec succès.");
    }

    void Update()
    {
        if (attack)
        { 
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }
        // Vérifier si un clic gauche de la souris est effectué
        if (Input.GetMouseButtonDown(0))  // 0 représente le clic gauche
        {
            // Vérifier si le joueur tient la torche
            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch())
            {
                Debug.Log("Le joueur tient la torche, impossible de cliquer sur l'ennemi.");
                return; // Sortir si le joueur tient la torche
            }

            // Lancer un raycast depuis la position de la caméra en direction de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Vérifier si le raycast touche quelque chose
            if (Physics.Raycast(ray, out hit))
            {
                // Vérifier si l'objet touché est cet ennemi
                if (hit.collider.gameObject == gameObject)
                {
                    // Trouver l'objet du joueur avec le tag "Player"
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    // Vérifier que le joueur existe
                    if (player != null)
                    {
                        // Calculer la distance entre le joueur et l'ennemi
                        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                        // Vérifier si le joueur est dans la portée d'interaction
                        if (distanceToPlayer <= interactionRange)
                        {
                            Debug.Log("Ennemi cliqué : " + gameObject.name);
                            // Démarrer le fade-out
                            playerAnimator.SetBool("IsAttacking",true);
                            attack = true;
                            StartCoroutine(FadeOutAndDestroy());
                        }
                        else
                        {
                            Debug.Log("Trop loin pour attaquer l'ennemi : " + gameObject.name);
                        }
                    }
                    else
                    {
                        Debug.LogError("Aucun objet avec le tag 'Player' trouvé !");
                    }
                }
            }
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
        // Désactiver le collider pour éviter d'autres interactions
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
