using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // V�rifier si le joueur touche l'ennemi
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ennemi touch� par le joueur, il dispara�t.");
            Destroy(gameObject);  // D�truire cet ennemi
        }
    }
}
