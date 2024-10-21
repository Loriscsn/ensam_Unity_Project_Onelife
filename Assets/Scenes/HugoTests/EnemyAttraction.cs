using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttraction : MonoBehaviour
{
    private Transform target;
    private float speed;

    // Assigner la cible (ici la torche) et la vitesse d'attraction
    public void SetTarget(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
    }

    void Update()
    {
        if (target != null)
        {
            // Déplacer l'ennemi vers la position de la torche
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optionnel: ici tu peux ajouter des conditions pour quand l'ennemi arrive à la torche
        }
    }
}
