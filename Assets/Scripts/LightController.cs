using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumi�re attach�e � l'objet
    public float minRange = 2.5f;  // La port�e minimum de la lumi�re
    public float maxRange = 15f; // La port�e maximum de la lumi�re
    public float transitionSpeed = 15f; // La vitesse de transition du changement de port�e
    private bool isExpanding = true; // Indicateur si on agrandit ou r�tr�cit
    private bool isTransitioning = false; // V�rifie si la transition est en cours
    public bool IsHoldingTorch = false; // Indicateur si la torche est dans la main du joueur
    public float pickUpRange = 2f; // Distance pour ramasser la torche
    public Transform player; // R�f�rence au joueur pour v�rifier la distance

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
            // Si la torche n'est pas dans la main, on v�rifie la distance pour la ramasser
            if (!IsHoldingTorch)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // Si le joueur est suffisamment proche, il peut ramasser la torche
                if (distanceToPlayer <= pickUpRange)
                {
                    IsHoldingTorch = true;
                    Debug.Log("Vous avez ramass� la torche.");
                }
            }
            else
            {
                // Si la torche est dans la main, contr�ler la lumi�re
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
                    isExpanding = !isExpanding; // Inverser l'�tat d'expansion/r�duction
                    isTransitioning = true; // D�marrer la transition
                }
            }
        }

        // Effectuer la transition si elle est en cours et si la torche est dans la main
        if (IsHoldingTorch && isTransitioning)
        {
            if (isExpanding)
            {
                // R�tr�cir vers minRange
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
