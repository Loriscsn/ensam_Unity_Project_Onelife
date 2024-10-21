using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public Vector3 offset; // Offset de la caméra par rapport au joueur

    void Start()
    {
        // Si aucun offset n'est défini dans l'inspecteur, on le définit comme la position actuelle
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Mise à jour de la position de la caméra en suivant le joueur sans changer la rotation
        transform.position = player.position + offset;
    }
}
