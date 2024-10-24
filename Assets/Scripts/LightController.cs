using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    public Light lightSource; // La lumière attachée à l'objet
    public float minRange = 2.5f;  // La portée minimum de la lumière
    public float maxRange = 15f; // La portée maximum de la lumière
    public float transitionSpeed = 15f; // La vitesse de transition du changement de portée
    private bool isTransitioning = false; // Vérifie si la transition est en cours
    public bool isTorchInHand = true; // La torche commence dans la main du joueur
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
            if (isTorchInHand)
            {
                // Le joueur lâche la torche et la lumière s'agrandit
                isTorchInHand = false;
                isTransitioning = true; // Commence la transition pour agrandir la lumière
            }
            else
            {
                // Le joueur essaie de ramasser la torche
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= pickUpRange)
                {
                    // Le joueur récupère la torche et la lumière diminue
                    isTorchInHand = true;
                    isTransitioning = true; // Commence la transition pour diminuer la lumière
                    Debug.Log("Vous avez ramassé la torche.");
                }
                else
                {
                    Debug.Log("Vous êtes trop loin pour ramasser la torche.");
                }
            }
        }

        // Effectuer la transition pour changer la portée de la lumière
        if (isTransitioning)
        {
            if (isTorchInHand)
            {
                // Si la torche est dans la main, rétrécir la lumière vers minRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, minRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == minRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
            else
            {
                // Si la torche est au sol, agrandir la lumière vers maxRange
                lightSource.range = Mathf.MoveTowards(lightSource.range, maxRange, transitionSpeed * Time.deltaTime);
                if (lightSource.range == maxRange)
                {
                    isTransitioning = false; // Fin de la transition
                }
            }
        }
    }
}
