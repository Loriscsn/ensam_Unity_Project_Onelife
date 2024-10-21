using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    public Transform handPosition;           // Position de la main
    public GameObject initialObject;         // Objet que le personnage tiendra au début du jeu
    public float pickUpRange = 1.0f;         // Distance pour ramasser
    public Vector3 plantedRotation = new Vector3(0, 0, 0); // Rotation verticale
    public float plantHeightOffset = 0.1f;   // Hauteur du sceptre planté
    public TorchLightController torchLightController; // Référence au contrôleur de lumière
    private GameObject carriedObject = null; // Objet ramassé

    void Start()
    {
        if (initialObject != null)
        {
            PickUpObject(initialObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (carriedObject != null)
            {
                // Si un objet est porté, on le plante
                PlantObject();
            }
            else
            {
                // Sinon, essayer de ramasser un objet
                TryPickUpObject();
            }
        }
    }

    void TryPickUpObject()
    {
        // Chercher les objets autour du personnage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Pickup"))
            {
                PickUpObject(hitCollider.gameObject);
                break;
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        // Désactiver la physique pour ramasser l'objet
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Empêcher la physique tant qu'il est porté
            rb.useGravity = false; // Désactiver la gravité
        }

        // Positionner et attacher l'objet à la main
        obj.transform.position = handPosition.position;
        obj.transform.rotation = handPosition.rotation;
        obj.transform.parent = handPosition;

        carriedObject = obj;

        // Notifier que la torche est portée
        if (torchLightController != null)
        {
            torchLightController.SetTorchHeld(true);
        }
    }

    void PlantObject()
    {
        // Détacher l'objet de la main
        carriedObject.transform.parent = null;

        // Positionner l'objet au sol avec une orientation verticale
        Vector3 plantPosition = new Vector3(transform.position.x, 0 + plantHeightOffset, transform.position.z);
        carriedObject.transform.position = plantPosition;
        carriedObject.transform.eulerAngles = plantedRotation;

        // Rendre l'objet immobile mais laisser les collisions actives
        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Rendre l'objet immobile
            rb.detectCollisions = true; // Garder les collisions actives pour pouvoir le reprendre
        }

        // Notifier que la torche est posée
        if (torchLightController != null)
        {
            torchLightController.SetTorchHeld(false);
        }

        carriedObject = null; // Réinitialiser la référence
    }
}
