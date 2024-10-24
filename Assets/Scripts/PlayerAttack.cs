using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // R�f�rence au prefab de votre syst�me de particules
    public Transform handTransform; // Transform de la main o� les particules partent
    private PickUpDrop pickUpDrop; // R�f�rence au script de prise/d�pose

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
        // Lancer le syst�me de particules si le joueur n'a pas de torche
        if (!pickUpDrop.IsHoldingTorch())
        {
           
        }
    }

    
}
