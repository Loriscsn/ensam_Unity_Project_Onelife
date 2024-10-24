using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumi�re attach�e � l'objet
    public float minRange = 2.5f;  // La port�e minimum de la lumi�re
    public float maxRange = 15f; // La port�e maximum de la lumi�re
    public float transitionSpeed = 15f; // La vitesse de transition du changement de port�e
    private bool isTransitioning = false; // V�rifie si la transition est en cours
    public bool isTorchInHand = true; // La torche commence dans la main du joueur
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
            if (isTorchInHand)
            {
                // Le joueur l�che la torche et la lumi�re s'agrandit
                isTorchInHand = false;
                isTransitioning = true; // Commence la transition pour agrandir la lumi�re
            }
            else
            {
                // Le joueur essaie de ramasser la torche
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= pickUpRange)
                {
                    // Le joueur r�cup�re la torche et la lumi�re diminue
                    isTorchInHand = true;
                    isTransitioning = true; // Commence la transition pour diminuer la lumi�re
                    Debug.Log("Vous avez ramass� la torche.");
                }
                else
                {
                    Debug.Log("Vous �tes trop loin pour ramasser la torche.");
                }
            }
        }

        // Effectuer la transition pour changer la port�e de la lumi�re
        if (isTransitioning)
        {
            if (isTorchInHand)
            {
                // Si la torche est dans la main, r�tr�cir la lumi�re vers minRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, minRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == minRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
            else
            {
                // Si la torche est au sol, agrandir la lumi�re vers maxRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, maxRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == maxRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
        }
    }
}
