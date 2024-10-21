using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumière attachée à l'objet
    public float minRange = 5f;  // La portée minimum de la lumière
    public float maxRange = 20f; // La portée maximum de la lumière
    public float transitionSpeed = 5f; // La vitesse de transition du changement de portée
    private bool isExpanding = true; // Indicateur si on agrandit ou rétrécit
    private bool isTransitioning = false; // Vérifie si la transition est en cours

    void Start()
    {
        // Si aucune lumière n'est assignée, essayer de récupérer la lumière attachée à l'objet
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        // Assurez-vous que la lumière est de type Point pour ajuster la portée.
        if (lightSource.type != LightType.Point)
        {
            Debug.LogError("La lumière doit être de type Point pour ajuster la portée (range).");
        }
    }

    void Update()
    {
        // Vérifier si l'utilisateur appuie sur la touche espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Si la lumière est en cours d'agrandissement, inverser pour la faire rétrécir
            if (isTransitioning && isExpanding)
            {
                isExpanding = false;
            }
            // Si la lumière est en cours de rétrécissement, inverser pour la faire agrandir
            else if (isTransitioning && !isExpanding)
            {
                isExpanding = true;
            }
            // Si aucune transition n'est en cours, démarrer une nouvelle transition
            else if (!isTransitioning)
            {
                isExpanding = !isExpanding; // Inverser l'état d'expansion/réduction
                isTransitioning = true; // Démarrer la transition
            }
        }

        // Effectuer la transition si elle est en cours
        if (isTransitioning)
        {
            if (isExpanding)
            {
                // Agrandir la portée vers maxRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, maxRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == maxRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
            else
            {
                // Rétrécir la portée vers minRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, minRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == minRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
        }
    }
}
