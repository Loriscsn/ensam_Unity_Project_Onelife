using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float firstPhaseDuration = 0.5f;
    public float secondPhaseDuration = 1f;
    private Renderer enemyRenderer;
    private Animator playerAnimator;
    private bool attack = false;

    public float interactionRange = 5f; // Distance maximale pour attaquer
    private PickUpDrop pickUpDrop;      // R�f�rence au script PickUpDrop

    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBar;

    private Color healthColor;

    public int clicksToKill = 3;        // Nombre de clics n�cessaires pour tuer
    private float healthReductionPerClick;

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        enemyRenderer = transform.GetChild(1).GetComponent<Renderer>();
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assign� !");
        }

        AssignRandomColor();
        currentHealth = maxHealth;
        UpdateHealthBar();
        healthReductionPerClick = maxHealth / clicksToKill;
    }

    void Update()
    {
        // Orienter la barre de vie vers la cam�ra
        healthBar.gameObject.transform.parent.transform.LookAt(Camera.main.transform.position);

        if (attack)
        {
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }

        // V�rifier si le joueur clique gauche
        if (Input.GetMouseButtonDown(0))
        {
            // Sortir si le joueur tient la torche
            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast pour v�rifier quel objet est sous le curseur
            if (Physics.Raycast(ray, out hit))
            {
                // V�rifier que c'est bien cet ennemi qui est cliqu�
                if (hit.collider.gameObject == gameObject)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    // V�rifier si le joueur est assez proche
                    if (player != null && Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
                    {
                        // R�duction de sant� de l'ennemi cibl�
                        attack = true;
                        ReduceHealth(healthReductionPerClick);
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
        UpdateHealthBar();
        StartCoroutine(ShakeHealthBar());
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;
        healthColor = Color.Lerp(Color.red, Color.green, healthPercentage);
        healthBar.color = healthColor;
    }

    private void DestroyEnemy()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        GetComponent<Collider>().enabled = false;
        Color enemyColor = enemyRenderer.material.color;

        float elapsedTime = 0f;
        while (elapsedTime < firstPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / firstPhaseDuration);
            enemyColor.a = Mathf.Lerp(1f, 0.5f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < secondPhaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / secondPhaseDuration);
            enemyColor.a = Mathf.Lerp(0.5f, 0f, normalizedTime);
            enemyRenderer.material.color = enemyColor;

            yield return null;
        }

        enemyColor.a = 0f;
        enemyRenderer.material.color = enemyColor;
        Destroy(gameObject);
    }

    void AssignRandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        enemyRenderer.material.color = randomColor;
    }

    IEnumerator ShakeHealthBar()
    {
        Vector3 originalPosition = healthBar.transform.localPosition;
        float shakeDuration = 0.1f;
        float shakeMagnitude = 5f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            healthBar.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        healthBar.transform.localPosition = originalPosition;
    }
}
