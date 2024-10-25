using UnityEngine;
using UnityEngine.UI;

public class TorchLife : MonoBehaviour
{
    public Image torchLifeImage; // Image de remplissage pour la barre de vie de la torche
    public float torchDuration = 300f; // Durée de vie en secondes (5 minutes)
    public ParticleSystem torchParticleSystem; // Référence au Particle System de la torche
    public float maxRateOverTime = 40f; // Taux maximum des particules quand la vie est à 100%
    public float enemyDamage = 5f; // Points de vie perdus lorsqu'un ennemi est proche
    public float enemyDetectionRange = 5.0f; // Rayon de détection des ennemis autour de la torche
    public float lifePerStele = 10f; // Points de vie ajoutés par stèle

    private float remainingTime;
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        remainingTime = torchDuration;
        torchLifeImage.fillAmount = 1f; // Remplissage complet au début

        // Accéder au module d'émission du Particle System
        emissionModule = torchParticleSystem.emission;
    }

    void Update()
    {
        // Réduire le temps restant chaque seconde
        remainingTime -= Time.deltaTime;

        // Calculer le pourcentage de vie restant
        float lifePercentage = remainingTime / torchDuration;

        // Mettre à jour le remplissage de la barre de vie
        torchLifeImage.fillAmount = lifePercentage;

        // Ajuster le Rate over Time du Particle System en fonction de la vie restante
        emissionModule.rateOverTime = lifePercentage * maxRateOverTime;

        // Vérifier la présence des ennemis dans le rayon de détection
        DetectAndDamageNearbyEnemies();

        // Si la torche est épuisée, appelle une fonction pour l'éteindre
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TurnOffTorch();
        }
    }

    // Méthode pour ajouter de la vie à la torche
    public void AddLife(float amount)
    {
        remainingTime += amount;

        // S'assurer que la vie ne dépasse pas la durée de vie maximale
        if (remainingTime > torchDuration)
        {
            remainingTime = torchDuration;
        }

        Debug.Log("Ajout de " + amount + " points de vie à la torche. Vie restante : " + remainingTime);
    }

    void DetectAndDamageNearbyEnemies()
    {
        // Trouver tous les objets avec un collider dans le rayon de détection
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);

        foreach (var hitCollider in hitColliders)
        {
            // Si l'objet détecté est un ennemi, inflige des dégâts à la torche
            if (hitCollider.CompareTag("Enemy"))
            {
                remainingTime -= enemyDamage;

                // Empêcher la vie de tomber en dessous de zéro
                if (remainingTime < 0)
                {
                    remainingTime = 0;
                }

                Debug.Log("Ennemi détecté près de la torche, perte de vie !");
                break; // Arrête après la première détection pour éviter plusieurs réductions par frame
            }
        }
    }

    void TurnOffTorch()
    {
        // Logique pour éteindre la torche
        Debug.Log("La torche est éteinte !");

        // Désactiver la lumière de la torche ou le Particle System, par exemple
        // GetComponent<Light>().enabled = false;
        torchParticleSystem.Stop();
    }
}
