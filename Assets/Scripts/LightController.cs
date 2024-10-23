using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumière attachée à l'objet
    public float minRange = 2.5f;  // La portée minimum de la lumière
    public float maxRange = 15f; // La portée maximum de la lumière
    public float transitionSpeed = 15f; // La vitesse de transition du changement de portée
    private bool isExpanding = true; // Indicateur si on agrandit ou rétrécit
    private bool isTransitioning = false; // Vérifie si la transition est en cours
    public bool IsHoldingTorch = false; // Indicateur si la torche est dans la main du joueur
    public float pickUpRange = 2f; // Distance pour ramasser la torche
    public Transform player; // Référence au joueur pour vérifier la distance

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
            // Si la torche n'est pas dans la main, on vérifie la distance pour la ramasser
            if (!IsHoldingTorch)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // Si le joueur est suffisamment proche, il peut ramasser la torche
                if (distanceToPlayer <= pickUpRange)
                {
                    IsHoldingTorch = true;
                    Debug.Log("Vous avez ramassé la torche.");
                }
            }
            else
            {
                // Si la torche est dans la main, contrôler la lumière
                if (isTransitioning && isExpanding)
                {
                    isExpanding = false;
                }
                else if (isTransitioning && !isExpanding)
                {
                    isExpanding = true;
                }
                else if (!isTransitioning)
                {
                    isExpanding = !isExpanding; // Inverser l'état d'expansion/réduction
                    isTransitioning = true; // Démarrer la transition
                }
            }
        }

        // Effectuer la transition si elle est en cours et si la torche est dans la main
        if (IsHoldingTorch && isTransitioning)
        {
            if (isExpanding)
            {
                // Rétrécir vers minRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, minRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == minRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
            else
            {
                // Agrandir vers maxRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, maxRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == maxRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
        }
    }
}
