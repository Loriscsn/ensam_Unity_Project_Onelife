using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Nécessaire pour utiliser les éléments UI

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Durée pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Durée pour passer de 50% à 0% d'alpha
    private Renderer enemyRenderer;          // Référence au Renderer de l'ennemi
    private Animator playerAnimator;
    private bool attack = false;

    public float interactionRange = 5f;  // Distance maximale pour cliquer sur l'ennemi
    private PickUpDrop pickUpDrop; // Référence au script PickUpDrop

    public float maxHealth = 100f;  // Vie maximale de l'ennemi
    private float currentHealth;      // Vie actuelle de l'ennemi
    public Image healthBar;           // Image de la barre de vie (assignée dans l'éditeur)

    private Color healthColor;        // Couleur actuelle de la barre de vie

    // Nouveau champ pour le nombre de clics nécessaires pour tuer l'ennemi
    public int clicksToKill = 3;  // Nombre de clics nécessaires pour tuer l'ennemi
    private float healthReductionPerClick; // Réduction de santé par clic

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        enemyRenderer = GetComponent<Renderer>();
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assigné !");
        }

        AssignRandomColor();
        currentHealth = maxHealth; // Initialiser la vie actuelle
        UpdateHealthBar();
        healthReductionPerClick = maxHealth / clicksToKill; // Calculer la réduction de santé par clic
        Debug.Log("Ennemi instancié avec succès.");
    }

    void Update()
    {
        if (attack)
        {
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }

        if (Input.GetMouseButtonDown(0))  // Vérifier si un clic gauche est effectué
        {
            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch())
            {
                Debug.Log("Le joueur tient la torche, impossible de cliquer sur l'ennemi.");
                return; // Sortir si le joueur tient la torche
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    if (player != null)
                    {
                        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                        if (distanceToPlayer <= interactionRange)
                        {
                            Debug.Log("Ennemi cliqué : " + gameObject.name);
                            attack = true;

                            // Incrémenter les clics et réduire la vie de l'ennemi
                            ReduceHealth(healthReductionPerClick);  // Réduction de santé proportionnelle
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

    private void ReduceHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyEnemy();
        }
        UpdateHealthBar(); // Mettre à jour la barre de vie
        StartCoroutine(ShakeHealthBar()); // Démarrer l'effet de tremblement
    }

    private void UpdateHealthBar()
    {
        // Mettre à jour la couleur de la barre de vie
        float healthPercentage = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;

        // Changer la couleur de la barre de vie (vert à rouge)
        healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
        healthBar.color = healthColor;
    }

    private void DestroyEnemy()
    {
        // Logique pour détruire l'ennemi
        Debug.Log("L'ennemi est détruit !");
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        GetComponent<Collider>().enabled = false;

        Color enemyColor = enemyRenderer.material.color; // Couleur actuelle de l'ennemi

        float elapsedTime = 0f;
        while (elapsedTime < firstPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / firstPhaseDuration);
            enemyColor.a = Mathf.Lerp(1f, 0.5f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        elapsedTime = 0f;  // Réinitialiser le temps écoulé
        while (elapsedTime < secondPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / secondPhaseDuration);
            enemyColor.a = Mathf.Lerp(0.5f, 0f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        enemyColor.a = 0f;
        enemyRenderer.material.color = enemyColor; // Assurer que l'alpha est 0
        Destroy(gameObject);
    }

    // Méthode pour assigner une couleur aléatoire
    void AssignRandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        enemyRenderer.material.color = randomColor;
    }

    // Coroutine pour l'effet de tremblement de la barre de vie
    IEnumerator ShakeHealthBar()
    {
        Vector3 originalPosition = healthBar.transform.localPosition; // Position originale de la barre de vie
        float shakeDuration = 0.1f; // Durée de l'effet de tremblement
        float shakeMagnitude = 5f; // Intensité du tremblement
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            healthBar.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Réinitialiser la position de la barre de vie
        healthBar.transform.localPosition = originalPosition;
    }
}
