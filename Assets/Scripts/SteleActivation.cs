using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SteleActivation : MonoBehaviour
{
    public ParticleSystem activationEffect; // Le Particle System � activer
    public Light activationLight;           // La lumi�re � allumer
    public float lightFadeDuration = 2f;    // Dur�e de l'augmentation de la luminosit� (en secondes)
    public float targetIntensity = 1.5f;    // Intensit� maximale de la lumi�re
    public float flickerAmount = 0.05f;     // Amplitude du scintillement (variation autour de l'intensit� cible)
    public float flickerDuration = 2f;      // Temps pour passer d'une intensit� � une autre
    public bool isActivated = false;        // Variable pour v�rifier si la st�le est d�j� activ�e

    // Param�tres pour le Halo
    public Behaviour haloComponent;         // Le composant Halo
    public float haloMaxSize = 1.5f;        // Taille maximale du halo
    public float haloFadeDuration = 2f;     // Dur�e de l'apparition progressive du halo

    private void OnTriggerStay(Collider other)
    {
        // V�rifier si le joueur est dans la zone de la st�le
        if (other.CompareTag("Player"))
        {
            // Obtenir le script PickUpDrop du joueur pour v�rifier s'il tient la torche
            PickUpDrop pickUpDrop = other.GetComponent<PickUpDrop>();

            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch() && !isActivated)
            {
                // Si le joueur presse la touche F et que la st�le n'est pas activ�e
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ActivateStele();
                }
            }
        }
    }

    // Fonction pour activer la st�le
    private void ActivateStele()
    {
        isActivated = true; // Marquer la st�le comme activ�e

        // Activer le Particle System
        if (activationEffect != null)
        {
            activationEffect.Play();
        }

        // D�marrer l'augmentation progressive de la lumi�re
        if (activationLight != null)
        {
            activationLight.gameObject.SetActive(true); // Activer le GameObject de la lumi�re
            StartCoroutine(IncreaseLightIntensity());  // D�marrer la coroutine pour augmenter l'intensit� de la lumi�re
        }

        // Activer le Halo progressivement
        if (haloComponent != null)
        {
            StartCoroutine(FadeInHalo());
        }

        // Debug pour confirmation
        Debug.Log("St�le activ�e avec lumi�re et halo !");
    }

    // Coroutine pour augmenter progressivement l'intensit� de la lumi�re
    private IEnumerator IncreaseLightIntensity()
    {
        float currentTime = 0f;
        float initialIntensity = activationLight.intensity; // Intensit� initiale (0 ou une valeur faible)

        while (currentTime < lightFadeDuration)
        {
            currentTime += Time.deltaTime;
            // Lerp pour augmenter progressivement l'intensit� de la lumi�re
            activationLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, currentTime / lightFadeDuration);
            yield return null; // Attendre la frame suivante
        }

        // Assurer que l'intensit� atteint exactement la cible � la fin
        activationLight.intensity = targetIntensity;

        // Une fois la lumi�re allum�e, d�marrer le scintillement
        StartCoroutine(FlickerLight());
    }

    // Coroutine pour faire scintiller la lumi�re doucement
    private IEnumerator FlickerLight()
    {
        float currentIntensity = targetIntensity; // D�part � l'intensit� cible

        while (true) // Boucle infinie pour garder le scintillement
        {
            // Calculer une nouvelle intensit� l�g�rement diff�rente de mani�re plus douce
            float newIntensity = targetIntensity + Random.Range(-flickerAmount, flickerAmount);

            // Faire varier l'intensit� de mani�re progressive sur la dur�e d�finie
            float elapsedTime = 0f;
            while (elapsedTime < flickerDuration)
            {
                elapsedTime += Time.deltaTime;
                activationLight.intensity = Mathf.Lerp(currentIntensity, newIntensity, elapsedTime / flickerDuration);
                yield return null; // Attendre la prochaine frame
            }

            // Mettre � jour l'intensit� courante
            currentIntensity = newIntensity;

            // Attendre une courte pause (optionnel) avant de recommencer
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Coroutine pour faire appara�tre le halo progressivement
    private IEnumerator FadeInHalo()
    {
        float currentTime = 0f;
        float initialSize = 0f;  // Taille initiale du Halo

        // Acc�der � la propri�t� Halo via le syst�me Reflection (car Unity n'expose pas cette propri�t� directement)
        var halo = (Component)haloComponent;

        while (currentTime < haloFadeDuration)
        {
            currentTime += Time.deltaTime;
            // Calculer la taille du halo progressivement
            float newSize = Mathf.Lerp(initialSize, haloMaxSize, currentTime / haloFadeDuration);

            // Acc�der � la taille du Halo et l'ajuster
            halo.GetType().GetProperty("size").SetValue(halo, newSize, null);

            yield return null; // Attendre la frame suivante
        }

        // Assurer que la taille du halo atteint bien la taille maximale
        halo.GetType().GetProperty("size").SetValue(halo, haloMaxSize, null);
    }
}