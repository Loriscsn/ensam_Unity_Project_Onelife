using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    // Propriétés de la lumière
    public float smallLightRange = 5f; // Portée de la lumière lorsqu'elle est portée
    public float largeLightRange = 15f; // Portée de la lumière lorsqu'elle est posée
    public float smallSpotAngle = 30f; // Angle de la Spot Light lorsqu'elle est portée
    public float largeSpotAngle = 60f; // Angle de la Spot Light lorsqu'elle est posée
    public float smallIntensity = 1f; // Intensité de la lumière lorsqu'elle est portée
    public float largeIntensity = 3f; // Intensité de la lumière lorsqu'elle est posée

    private Light spotLight; // Référence à la Spot Light

    void Start()
    {
        // Récupérer la Spot Light attachée à cet objet
        spotLight = GetComponentInChildren<Light>(); // Trouver la Spot Light enfant

        if (spotLight == null)
        {
            Debug.LogError("Aucune Spot Light trouvée dans les enfants de cet objet !");
            return; // Sortir si la Spot Light n'est pas trouvée
        }

        // Initialiser la Spot Light à l'état "portée"
        SetTorchHeld(true);
    }

    // Appelée pour changer l'état de la torche (portée ou posée)
    public void SetTorchHeld(bool isHeld)
    {
        if (isHeld)
        {
            // Réglages pour la torche portée
            spotLight.range = smallLightRange;
            spotLight.spotAngle = smallSpotAngle;
            spotLight.intensity = smallIntensity; // Intensité faible
        }
        else
        {
            // Réglages pour la torche posée
            spotLight.range = largeLightRange;
            spotLight.spotAngle = largeSpotAngle;
            spotLight.intensity = largeIntensity; // Intensité plus élevée
        }
    }
}
