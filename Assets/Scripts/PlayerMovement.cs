using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalMoveSpeed = 4f; // Vitesse normale de d�placement
    public float holdingTorchSpeed = 2f; // Vitesse r�duite quand on tient la torche
    public float rotationSpeed = 720f; // Vitesse de rotation du personnage (ajustable)

    private Vector3 movementDirection; // Direction de mouvement du joueur
    private Rigidbody rb; // Composant Rigidbody du personnage
    private PickUpDrop pickUpDrop; // R�f�rence au script PickUpDrop pour savoir si le personnage tient la torche
    private Animator animator; // Animation du personnage

    void Start()
    {
        // R�cup�rer le Rigidbody du personnage
        rb = GetComponent<Rigidbody>();

        // Trouver et stocker la r�f�rence au script PickUpDrop
        pickUpDrop = GetComponent<PickUpDrop>();

        // R�cup�rer l'Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Gestion des mouvements avec les touches ZQSD ou les fl�ches directionnelles
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // A/D ou Left/Right ou Q/D
        float moveVertical = Input.GetAxisRaw("Vertical"); // W/S ou Up/Down ou Z/S

        // Calculer la direction de mouvement en utilisant les axes X et Z
        movementDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Si le joueur se d�place, on fait tourner le personnage dans la direction du mouvement
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Gestion des animations en fonction du d�placement
        if (movementDirection != Vector3.zero)
        {
            // Si le joueur se d�place, on ajuste les animations (marche ou course)
            if (pickUpDrop.IsHoldingTorch())
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            // Si le joueur ne se d�place pas, on d�sactive les animations de mouvement
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
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
