using UnityEngine;
using UnityEngine.UI;

public class TorchLife : MonoBehaviour
{
    public Image torchLifeImage; // Image de remplissage pour la barre de vie de la torche
    public float torchDuration = 300f; // Dur�e de vie en secondes (5 minutes)
    public ParticleSystem torchParticleSystem; // R�f�rence au Particle System de la torche
    public float maxRateOverTime = 40f; // Taux maximum des particules quand la vie est � 100%
    public float enemyDamage = 5f; // Points de vie perdus lorsqu'un ennemi est proche
    public float enemyDetectionRange = 5.0f; // Rayon de d�tection des ennemis autour de la torche
    public float lifePerStele = 10f; // Points de vie ajout�s par st�le

    private float remainingTime;
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        remainingTime = torchDuration;
        torchLifeImage.fillAmount = 1f; // Remplissage complet au d�but

        // Acc�der au module d'�mission du Particle System
        emissionModule = torchParticleSystem.emission;
    }

    void Update()
    {
        // R�duire le temps restant chaque seconde
        remainingTime -= Time.deltaTime;

        // Calculer le pourcentage de vie restant
        float lifePercentage = remainingTime / torchDuration;

        // Mettre � jour le remplissage de la barre de vie
        torchLifeImage.fillAmount = lifePercentage;

        // Ajuster le Rate over Time du Particle System en fonction de la vie restante
        emissionModule.rateOverTime = lifePercentage * maxRateOverTime;

        // V�rifier la pr�sence des ennemis dans le rayon de d�tection
        DetectAndDamageNearbyEnemies();

        // Si la torche est �puis�e, appelle une fonction pour l'�teindre
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TurnOffTorch();
        }
    }

    // M�thode pour ajouter de la vie � la torche
    public void AddLife(float amount)
    {
        remainingTime += amount;

        // S'assurer que la vie ne d�passe pas la dur�e de vie maximale
        if (remainingTime > torchDuration)
        {
            remainingTime = torchDuration;
        }

        Debug.Log("Ajout de " + amount + " points de vie � la torche. Vie restante : " + remainingTime);
    }

    void DetectAndDamageNearbyEnemies()
    {
        // Trouver tous les objets avec un collider dans le rayon de d�tection
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);

        foreach (var hitCollider in hitColliders)
        {
            // Si l'objet d�tect� est un ennemi, inflige des d�g�ts � la torche
            if (hitCollider.CompareTag("Enemy"))
            {
                remainingTime -= enemyDamage;

                // Emp�cher la vie de tomber en dessous de z�ro
                if (remainingTime < 0)
                {
                    remainingTime = 0;
                }

                Debug.Log("Ennemi d�tect� pr�s de la torche, perte de vie !");
                break; // Arr�te apr�s la premi�re d�tection pour �viter plusieurs r�ductions par frame
            }
        }
    }

    void TurnOffTorch()
    {
        // Logique pour �teindre la torche
        Debug.Log("La torche est �teinte !");

        // D�sactiver la lumi�re de la torche ou le Particle System, par exemple
        // GetComponent<Light>().enabled = false;
        torchParticleSystem.Stop();
    }
}
