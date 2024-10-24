using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Dur�e pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Dur�e pour passer de 50% � 0% d'alpha
    private Renderer enemyRenderer;          // R�f�rence au Renderer de l'ennemi
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
    private PickUpDrop pickUpDrop; // R�f�rence au script PickUpDrop

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        // R�cup�rer le Renderer pour manipuler la couleur
        enemyRenderer = GetComponent<Renderer>();

        // V�rifiez si le Renderer est bien assign�
        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assign� !");
        }

        // R�cup�rer la r�f�rence au script PickUpDrop
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        // Assigner une couleur al�atoire � l'ennemi
        AssignRandomColor();
        Debug.Log("Ennemi instanci� avec succ�s.");
    }

    void Update()
    {
        if (attack)
        { 
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }
        // V�rifier si un clic gauche de la souris est effectu�
        if (Input.GetMouseButtonDown(0))  // 0 repr�sente le clic gauche
        {
            // V�rifier si le joueur tient la torche
            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch())
            {
                Debug.Log("Le joueur tient la torche, impossible de cliquer sur l'ennemi.");
                return; // Sortir si le joueur tient la torche
            }

            // Lancer un raycast depuis la position de la cam�ra en direction de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // V�rifier si le raycast touche quelque chose
            if (Physics.Raycast(ray, out hit))
            {
                // V�rifier si l'objet touch� est cet ennemi
                if (hit.collider.gameObject == gameObject)
                {
                    // Trouver l'objet du joueur avec le tag "Player"
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    // V�rifier que le joueur existe
                    if (player != null)
                    {
                        // Calculer la distance entre le joueur et l'ennemi
                        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                        // V�rifier si le joueur est dans la port�e d'interaction
                        if (distanceToPlayer <= interactionRange)
                        {
                            Debug.Log("Ennemi cliqu� : " + gameObject.name);
                            // D�marrer le fade-out
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
                        Debug.LogError("Aucun objet avec le tag 'Player' trouv� !");
                    }
                }
            }
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
        // D�sactiver le collider pour �viter d'autres interactions
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
