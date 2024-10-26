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

    public float interactionRange = 5f;
    private PickUpDrop pickUpDrop;

    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBar;

    public int clicksToKill = 3;
    private float healthReductionPerClick;

    void Start()
    {
        playerAnimator = FindObjectOfType<Animator>();
        enemyRenderer = transform.GetChild(1).GetComponent<Renderer>();
        pickUpDrop = FindObjectOfType<PickUpDrop>();

        if (enemyRenderer == null)
        {
            Debug.LogError("Le Renderer n'est pas assigné !");
        }

        AssignRandomColor();
        currentHealth = maxHealth;
        UpdateHealthBar();
        healthReductionPerClick = maxHealth / clicksToKill;
        Debug.Log("Ennemi instancié avec succès.");
    }

    void Update()
    {
        healthBar.gameObject.transform.parent.transform.LookAt(Camera.main.transform.position);

        if (attack)
        {
            attack = false;
            playerAnimator.SetBool("IsAttacking", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch())
            {
                Debug.Log("Le joueur tient la torche, impossible de cliquer sur l'ennemi.");
                return;
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

                            ReduceHealth(healthReductionPerClick);
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
        UpdateHealthBar();
        StartCoroutine(ShakeHealthBar());
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    private void DestroyEnemy()
    {
        Debug.Log("L'ennemi est détruit !");
        StartCoroutine(FadeOutAndDestroy());
    }

    public void OnAttackTorch()
    {
        Debug.Log("L'ennemi attaque la torche et disparaît !");
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
