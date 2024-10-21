using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    // Propri�t�s de la lumi�re
    public float smallLightRange = 5f; // Port�e de la lumi�re lorsqu'elle est port�e
    public float largeLightRange = 15f; // Port�e de la lumi�re lorsqu'elle est pos�e
    public float smallSpotAngle = 30f; // Angle de la Spot Light lorsqu'elle est port�e
    public float largeSpotAngle = 60f; // Angle de la Spot Light lorsqu'elle est pos�e
    public float smallIntensity = 1f; // Intensit� de la lumi�re lorsqu'elle est port�e
    public float largeIntensity = 3f; // Intensit� de la lumi�re lorsqu'elle est pos�e

    private Light spotLight; // R�f�rence � la Spot Light

    void Start()
    {
        // R�cup�rer la Spot Light attach�e � cet objet
        spotLight = GetComponentInChildren<Light>(); // Trouver la Spot Light enfant

        if (spotLight == null)
        {
            Debug.LogError("Aucune Spot Light trouv�e dans les enfants de cet objet !");
            return; // Sortir si la Spot Light n'est pas trouv�e
        }

        // Initialiser la Spot Light � l'�tat "port�e"
        SetTorchHeld(true);
    }

    // Appel�e pour changer l'�tat de la torche (port�e ou pos�e)
    public void SetTorchHeld(bool isHeld)
    {
        if (isHeld)
        {
            // R�glages pour la torche port�e
            spotLight.range = smallLightRange;
            spotLight.spotAngle = smallSpotAngle;
            spotLight.intensity = smallIntensity; // Intensit� faible
        }
        else
        {
            // R�glages pour la torche pos�e
            spotLight.range = largeLightRange;
            spotLight.spotAngle = largeSpotAngle;
            spotLight.intensity = largeIntensity; // Intensit� plus �lev�e
        }
    }
}
