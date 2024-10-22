using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalMoveSpeed = 7f; // Vitesse normale de d�placement
    public float holdingTorchSpeed = 5f; // Vitesse r�duite quand on tient la torche
    public float rotationSpeed = 720f; // Vitesse de rotation du personnage (ajustable)

    private Vector3 movementDirection; // Direction de mouvement du joueur
    private Rigidbody rb; // Composant Rigidbody du personnage

    private PickUpDrop pickUpDrop; // R�f�rence au script PickUpDrop pour savoir si le personnage tient la torche

    private Animator animator;
    void Start()
    {
        // R�cup�rer le Rigidbody du personnage
        rb = GetComponent<Rigidbody>();
        // Trouver et stocker la r�f�rence au script PickUpDrop
        pickUpDrop = GetComponent<PickUpDrop>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Obtenir les inputs de d�placement sur les axes horizontaux et verticaux
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // A/D ou Left/Right
        float moveVertical = Input.GetAxisRaw("Vertical"); // W/S ou Up/Down

        // Calculer la direction de mouvement
        movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Si le joueur se d�place, faire tourner le personnage dans la direction du mouvement
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            // Utilise la vitesse de rotation d�finie pour tourner plus vite
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        // D�terminer la vitesse en fonction de si le personnage tient la torche ou non
        float currentSpeed = pickUpDrop.IsHoldingTorch() ? holdingTorchSpeed : normalMoveSpeed;

        // Appliquer le mouvement au personnage avec la vitesse actuelle
        rb.MovePosition(rb.position + movementDirection * currentSpeed * Time.fixedDeltaTime);
    }
}
