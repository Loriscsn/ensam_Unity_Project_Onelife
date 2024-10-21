using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur touche l'ennemi
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ennemi touché par le joueur, il disparaît.");
            Destroy(gameObject);  // Détruire cet ennemi
        }
    }
}
