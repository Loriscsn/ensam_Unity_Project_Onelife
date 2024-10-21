using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumi�re attach�e � l'objet
    public float minRange = 5f;  // La port�e minimum de la lumi�re
    public float maxRange = 20f; // La port�e maximum de la lumi�re
    public float transitionSpeed = 5f; // La vitesse de transition du changement de port�e
    private bool isExpanding = true; // Indicateur si on agrandit ou r�tr�cit
    private bool isTransitioning = false; // V�rifie si la transition est en cours

    void Start()
    {
        // Si aucune lumi�re n'est assign�e, essayer de r�cup�rer la lumi�re attach�e � l'objet
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        // Assurez-vous que la lumi�re est de type Point pour ajuster la port�e.
        if (lightSource.type != LightType.Point)
        {
            Debug.LogError("La lumi�re doit �tre de type Point pour ajuster la port�e (range).");
        }
    }

    void Update()
    {
        // V�rifier si l'utilisateur appuie sur la touche espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Si la lumi�re est en cours d'agrandissement, inverser pour la faire r�tr�cir
            if (isTransitioning && isExpanding)
            {
                isExpanding = false;
            }
            // Si la lumi�re est en cours de r�tr�cissement, inverser pour la faire agrandir
            else if (isTransitioning && !isExpanding)
            {
                isExpanding = true;
            }
            // Si aucune transition n'est en cours, d�marrer une nouvelle transition
            else if (!isTransitioning)
            {
                isExpanding = !isExpanding; // Inverser l'�tat d'expansion/r�duction
                isTransitioning = true; // D�marrer la transition
            }
        }

        // Effectuer la transition si elle est en cours
        if (isTransitioning)
        {
            if (isExpanding)
            {
                // Agrandir la port�e vers maxRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, maxRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == maxRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
            else
            {
                // R�tr�cir la port�e vers minRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, minRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == minRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
        }
    }
}
