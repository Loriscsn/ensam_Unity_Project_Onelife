using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    public Transform handPosition;    // Position de la main
    public GameObject initialObject;  // Objet que le personnage tiendra au d�but du jeu
    public float pickUpRange = 1.0f;  // Distance pour ramasser
    public Vector3 plantedRotation = new Vector3(0, 0, 0); // Rotation verticale
    public float plantHeightOffset = 0.0f;  // Hauteur du sceptre plant�
    private GameObject carriedObject = null; // Objet ramass�

    void Start()
    {
        if (initialObject != null)
        {
            // Si un objet initial est d�fini, le ramasser au d�but du jeu
            PickUpObject(initialObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (carriedObject != null)
            {
                // Si un objet est port�, on le plante
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
        // D�sactiver la physique pour ramasser l'objet
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Emp�cher la physique tant qu'il est port�
            rb.useGravity = false; // D�sactiver la gravit�
        }

        // Positionner et attacher l'objet � la main
        obj.transform.position = handPosition.position;
        obj.transform.rotation = handPosition.rotation;
        obj.transform.parent = handPosition;

        carriedObject = obj;
    }

    void PlantObject()
    {
        // D�tacher l'objet de la main
        carriedObject.transform.parent = null;

        // Positionner l'objet au sol avec une orientation verticale
        Vector3 plantPosition = new Vector3(transform.position.x, 0 + plantHeightOffset, transform.position.z);
        carriedObject.transform.position = plantPosition;
        carriedObject.transform.eulerAngles = plantedRotation;

        // Rendre l'objet immobile mais laisser les collisions actives
        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;   // Rendre l'objet immobile
            rb.detectCollisions = true;  // Garder les collisions actives pour pouvoir le reprendre
        }

        // Laisser le collider actif pour bloquer le personnage
        Collider collider = carriedObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;  // Le collider reste actif pour bloquer le joueur
        }

        carriedObject = null;  // R�initialiser la r�f�rence
    }

   


    // Nouvelle m�thode pour savoir si la torche est port�e
    public bool IsHoldingTorch()
    {
        return carriedObject != null; // Retourne true si le joueur porte la torche, sinon false
    }
}
