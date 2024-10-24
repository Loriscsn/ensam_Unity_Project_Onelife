using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Référence au prefab de votre système de particules
    public Transform handTransform; // Transform de la main où les particules partent
    private PickUpDrop pickUpDrop; // Référence au script de prise/dépose

    public int portee = 10;

    void Start()
    {
        // Initialiser le script PickUpDrop
        pickUpDrop = GetComponent<PickUpDrop>();
    }

    void Update()
    {
        
    }

    void Attack()
    {
        // Lancer le système de particules si le joueur n'a pas de torche
        if (!pickUpDrop.IsHoldingTorch())
        {
           
        }
    }

    
}
