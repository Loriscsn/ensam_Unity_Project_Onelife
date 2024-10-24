using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform torch;  // La position de la torche
    public float speed = 3.0f; // Vitesse de d�placement de l'ennemi
    public float detectionRange = 10.0f;  // Distance maximale pour d�tecter la torche
    public float fieldOfView = 45.0f;     // Angle de vue de l'ennemi
    public LayerMask obstructionMask;      // Masque de collision pour les obstacles (comme les murs)
    public float minDistanceBetweenEnemies = 1.0f; // Distance minimale entre les ennemis
    public float avoidanceRadius = 0.5f;   // Rayon d'�vitement pour les ennemis

    private bool canSeeTorch = false;  // Savoir si l'ennemi peut voir la torche
    private Rigidbody rb; // R�f�rence au Rigidbody de l'ennemi
    float initialYpos;
    public float amplitude = 0.01f;
    public float frequency = 5f;
    float time = 0f;
    void Start()
    {
        initialYpos = transform.position.y;
        // Assurez-vous que la torche a �t� assign�e
        if (torch == null)
        {
            torch = GameObject.FindGameObjectWithTag("Pickup").transform;
        }

        // Obtenir le Rigidbody de l'ennemi
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        time += Time.deltaTime;
        // Calcul de la distance entre l'ennemi et la torche
        float distanceToTorch = Vector3.Distance(transform.position, torch.position);

        if (distanceToTorch <= detectionRange)
        {
            // Calculer la direction vers la torche
            Vector3 directionToTorch = (torch.position - transform.position).normalized;

            // Calculer l'angle entre la direction actuelle de l'ennemi et la direction vers la torche
            float angleToTorch = Vector3.Angle(transform.forward, directionToTorch);

            if (angleToTorch <= fieldOfView / 2)
            {
                // Effectuer un Raycast pour v�rifier s'il y a des obstacles entre l'ennemi et la torche
                if (!Physics.Raycast(transform.position, directionToTorch, distanceToTorch, obstructionMask))
                {
                    canSeeTorch = true;
                    FollowTorch(directionToTorch);
                }
                else
                {
                    canSeeTorch = false;
                }
            }
            else
            {
                canSeeTorch = false;
            }
        }
        else
        {
            canSeeTorch = false;
        }
    }

    void FollowTorch(Vector3 directionToTorch)
    {
        // Faire tourner l'ennemi vers la torche
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTorch.x, 0, directionToTorch.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

        // V�rifier si l'ennemi doit �viter d'autres ennemis
        Vector3 avoidanceDirection = GetAvoidanceDirection();
        Vector3 moveDirection = (directionToTorch + avoidanceDirection).normalized;
        Vector3 floatOffset = Vector3.up * (Mathf.Sin(time * frequency) * amplitude + initialYpos);
        // D�placer l'ennemi vers la torche tout en �vitant d'autres ennemis
        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime + floatOffset);
    }

    Vector3 GetAvoidanceDirection()
    {
        // V�rifie les collisions avec les autres ennemis
        Collider[] colliders = Physics.OverlapSphere(transform.position, avoidanceRadius);
        Vector3 avoidance = Vector3.zero;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject) // Ignore soi-m�me
            {
                // Calcule la direction d'�vitement
                Vector3 directionToOther = transform.position - collider.transform.position;
                avoidance += directionToOther.normalized / directionToOther.magnitude; // Plus l'ennemi est proche, plus l'�vitement sera fort
            }
        }

        return avoidance; // Retourne la direction d'�vitement cumul�e
    }

    void OnDrawGizmos()
    {
        // Dessine la port�e de d�tection de l'ennemi et son champ de vision dans l'�diteur
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView / 2, transform.up) * transform.forward * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView / 2, transform.up) * transform.forward * detectionRange;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        if (canSeeTorch)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, torch.position);
        }
    }
}
