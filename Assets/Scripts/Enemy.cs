using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // N�cessaire pour utiliser les �l�ments UI

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;  // Dur�e pour atteindre 50% d'alpha
    public float secondPhaseDuration = 1f;   // Dur�e pour passer de 50% � 0% d'alpha
    private Renderer enemyRenderer;          // R�f�rence au Renderer de l'ennemi
    private Animator playerAnimator;
    private bool attack = false;

    public float interactionRange = 5f;  // Distance maximale pour cliquer sur l'ennemi
    private PickUpDrop pickUpDrop; // R�f�rence au script PickUpDrop

    public float maxHealth = 100f;  // Vie maximale de l'ennemi
    private float currentHealth;      // Vie actuelle de l'ennemi
    public Image healthBar;           // Image de la barre de vie (assign�e dans l'�diteur)

    private Color healthColor;        // Couleur actuelle de la barre de vie

    // Nouveau champ pour le nombre de clics n�cessaires pour tuer l'ennemi
    public int clicksToKill = 3;  // Nombre de clics n�cessaires pour tuer l'ennemi
    private float healthReductionPerClick; // R�duction de sant� par clic

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        enemyRenderer = GetComponent<Renderer>();
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assign� !");
        }

        AssignRandomColor();
        currentHealth = maxHealth; // Initialiser la vie actuelle
        UpdateHealthBar();
        healthReductionPerClick = maxHealth / clicksToKill; // Calculer la r�duction de sant� par clic
        Debug.Log("Ennemi instanci� avec succ�s.");
    }

    void Update()
    {
        if (attack)
        {
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }

        if (Input.GetMouseButtonDown(0))  // V�rifier si un clic gauche est effectu�
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
                            Debug.Log("Ennemi cliqu� : " + gameObject.name);
                            attack = true;

                            // Incr�menter les clics et r�duire la vie de l'ennemi
                            ReduceHealth(healthReductionPerClick);  // R�duction de sant� proportionnelle
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

    private void ReduceHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyEnemy();
        }
        UpdateHealthBar(); // Mettre � jour la barre de vie
        StartCoroutine(ShakeHealthBar()); // D�marrer l'effet de tremblement
    }

    private void UpdateHealthBar()
    {
        // Mettre � jour la couleur de la barre de vie
        float healthPercentage = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;

        // Changer la couleur de la barre de vie (vert � rouge)
        healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
        healthBar.color = healthColor;
    }

    private void DestroyEnemy()
    {
        // Logique pour d�truire l'ennemi
        Debug.Log("L'ennemi est d�truit !");
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

        elapsedTime = 0f;  // R�initialiser le temps �coul�
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

    // M�thode pour assigner une couleur al�atoire
    void AssignRandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        enemyRenderer.material.color = randomColor;
    }

    // Coroutine pour l'effet de tremblement de la barre de vie
    IEnumerator ShakeHealthBar()
    {
        Vector3 originalPosition = healthBar.transform.localPosition; // Position originale de la barre de vie
        float shakeDuration = 0.1f; // Dur�e de l'effet de tremblement
        float shakeMagnitude = 5f; // Intensit� du tremblement
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            healthBar.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // R�initialiser la position de la barre de vie
        healthBar.transform.localPosition = originalPosition;
    }
}
