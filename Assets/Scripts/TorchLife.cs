using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TorchLife : MonoBehaviour
{
    public Image torchLifeImage;
    public float torchDuration = 300f;
    public ParticleSystem torchParticleSystem;
    public float maxRateOverTime = 40f;
    public float enemyDamage = 5f;
    public float damageInterval = 1f; // Intervalle de temps entre chaque dégât
    public float enemyDetectionRange = 5.0f;

    private float remainingTime;
    private ParticleSystem.EmissionModule emissionModule;
    private List<Enemy> activeEnemies = new List<Enemy>();

    void Start()
    {
        remainingTime = torchDuration;
        torchLifeImage.fillAmount = 1f;
        emissionModule = torchParticleSystem.emission;
        StartCoroutine(PeriodicDamageCheck());
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        float lifePercentage = remainingTime / torchDuration;
        torchLifeImage.fillAmount = lifePercentage;
        emissionModule.rateOverTime = lifePercentage * maxRateOverTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TurnOffTorch();
        }
    }

    public void AddLife(float amount)
    {
        remainingTime = Mathf.Min(remainingTime + amount, torchDuration);
        Debug.Log("Ajout de " + amount + " points de vie à la torche. Vie restante : " + remainingTime);
    }

    public void AddLifeFromStele()
    {
        float currentLifePercentage = remainingTime / torchDuration;
        remainingTime = currentLifePercentage < 0.6f
            ? torchDuration * 0.6f
            : Mathf.Min(remainingTime + torchDuration * 0.2f, torchDuration);

        Debug.Log("Vie ajustée après activation de la stèle. Vie actuelle : " + remainingTime);
    }

    private IEnumerator PeriodicDamageCheck()
    {
        while (true)
        {
            DetectNearbyEnemies();
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void DetectNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);
        List<Enemy> detectedEnemies = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null && !activeEnemies.Contains(enemy))
                {
                    activeEnemies.Add(enemy);
                }
                detectedEnemies.Add(enemy);
                ApplyDamage();
            }
        }

        // Supprimer les ennemis qui sont sortis du rayon de détection
        activeEnemies.RemoveAll(enemy => !detectedEnemies.Contains(enemy));
    }

    private void ApplyDamage()
    {
        remainingTime = Mathf.Max(remainingTime - enemyDamage, 0);
        Debug.Log("Ennemi inflige des dégâts à la torche, vie restante : " + remainingTime);
    }

    void TurnOffTorch()
    {
        Debug.Log("La torche est éteinte !");
        torchParticleSystem.Stop();
        StopCoroutine(PeriodicDamageCheck());
    }
}
