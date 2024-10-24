using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SteleActivation : MonoBehaviour
{
    public ParticleSystem activationEffect; // Le Particle System à activer
    public Light activationLight;           // La lumière à allumer
    public float lightFadeDuration = 2f;    // Durée de l'augmentation de la luminosité (en secondes)
    public float targetIntensity = 1.5f;    // Intensité maximale de la lumière
    public float flickerAmount = 0.05f;     // Amplitude du scintillement (variation autour de l'intensité cible)
    public float flickerDuration = 2f;      // Temps pour passer d'une intensité à une autre
    public bool isActivated = false;        // Variable pour vérifier si la stèle est déjà activée

    private void OnTriggerStay(Collider other)
    {
        // Vérifier si le joueur est dans la zone de la stèle
        if (other.CompareTag("Player"))
        {
            // Obtenir le script PickUpDrop du joueur pour vérifier s'il tient la torche
            PickUpDrop pickUpDrop = other.GetComponent<PickUpDrop>();

            if (pickUpDrop != null && pickUpDrop.IsHoldingTorch() && !isActivated)
            {
                // Si le joueur presse la touche F et que la stèle n'est pas activée
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ActivateStele();
                }
            }
        }
    }

    // Fonction pour activer la stèle
    private void ActivateStele()
    {
        isActivated = true; // Marquer la stèle comme activée

        // Activer le Particle System
        if (activationEffect != null)
        {
            activationEffect.Play();
        }

        // Démarrer l'augmentation progressive de la lumière
        if (activationLight != null)
        {
            activationLight.gameObject.SetActive(true); // Activer le GameObject de la lumière
            StartCoroutine(IncreaseLightIntensity());  // Démarrer la coroutine pour augmenter l'intensité de la lumière
        }

        // Debug pour confirmation
        Debug.Log("Stèle activée avec lumière !");
    }

    // Coroutine pour augmenter progressivement l'intensité de la lumière
    private IEnumerator IncreaseLightIntensity()
    {
        float currentTime = 0f;
        float initialIntensity = activationLight.intensity; // Intensité initiale (0 ou une valeur faible)

        while (currentTime < lightFadeDuration)
        {
            currentTime += Time.deltaTime;
            // Lerp pour augmenter progressivement l'intensité de la lumière
            activationLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, currentTime / lightFadeDuration);
            yield return null; // Attendre la frame suivante
        }

        // Assurer que l'intensité atteint exactement la cible à la fin
        activationLight.intensity = targetIntensity;

        // Une fois la lumière allumée, démarrer le scintillement
        StartCoroutine(FlickerLight());
    }

    // Coroutine pour faire scintiller la lumière doucement
    private IEnumerator FlickerLight()
    {
        float currentIntensity = targetIntensity; // Départ à l'intensité cible

        while (true) // Boucle infinie pour garder le scintillement
        {
            // Calculer une nouvelle intensité légèrement différente de manière plus douce
            float newIntensity = targetIntensity + Random.Range(-flickerAmount, flickerAmount);

            // Faire varier l'intensité de manière progressive sur la durée définie
            float elapsedTime = 0f;
            while (elapsedTime < flickerDuration)
            {
                elapsedTime += Time.deltaTime;
                activationLight.intensity = Mathf.Lerp(currentIntensity, newIntensity, elapsedTime / flickerDuration);
                yield return null; // Attendre la prochaine frame
            }

            // Mettre à jour l'intensité courante
            currentIntensity = newIntensity;

            // Attendre une courte pause (optionnel) avant de recommencer
            yield return new WaitForSeconds(0.1f);
        }
    }
}
