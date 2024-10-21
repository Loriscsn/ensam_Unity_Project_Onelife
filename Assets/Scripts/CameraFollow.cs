using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public Vector3 offset; // Offset de la cam�ra par rapport au joueur

    void Start()
    {
        // Si aucun offset n'est d�fini dans l'inspecteur, on le d�finit comme la position actuelle
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Mise � jour de la position de la cam�ra en suivant le joueur sans changer la rotation
        transform.position = player.position + offset;
    }
}
