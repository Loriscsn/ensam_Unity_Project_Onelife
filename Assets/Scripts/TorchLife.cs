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
    public GameObject loseCanvas; // R�f�rence au Canvas de d�faite

    private float remainingTime;
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        remainingTime = torchDuration;
        torchLifeImage.fillAmount = 1f; // Remplissage complet au d�but
        emissionModule = torchParticleSystem.emission;

        if (loseCanvas != null)
        {
            loseCanvas.SetActive(false); // D�sactive le Canvas de d�faite au d�marrage
        }
        else
        {
            Debug.LogError("LoseCanvas n'est pas assign� dans l'Inspector !");
        }
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

    // M�thode pour ajouter la vie selon le niveau d�fini par la st�le
    public void AddLifeFromStele()
    {
        float currentLifePercentage = remainingTime / torchDuration;

        if (currentLifePercentage < 0.6f) // Si en dessous de 60%
        {
            remainingTime = torchDuration * 0.6f; // Ajuster � 60% de la dur�e de vie maximale
        }
        else // Si au-dessus de 60%
        {
            remainingTime += torchDuration * 0.2f; // Ajouter 20% de la dur�e de vie maximale
            if (remainingTime > torchDuration) // Limiter � la dur�e max
            {
                remainingTime = torchDuration;
            }
        }

        Debug.Log("Vie ajust�e apr�s activation de la st�le. Vie actuelle : " + remainingTime);
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
        torchParticleSystem.Stop();

        // Afficher l'�cran Lose
        if (loseCanvas != null)
        {
            loseCanvas.SetActive(true);
        }
    }
}
