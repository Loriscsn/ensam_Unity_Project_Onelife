using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StelesManager : MonoBehaviour
{
    public SteleActivation[] steles; // Tableau de toutes les stèles dans la scène
    private int activatedStelesCount = 0;  // Compteur de stèles activées

    void Start()
    {
        // Vérifier que toutes les stèles sont assignées
        if (steles.Length == 0)
        {
            Debug.LogError("Aucune stèle n'est assignée au manager !");
        }
    }

    // Fonction appelée par chaque stèle lorsqu'elle est activée
    public void NotifySteleActivated()
    {
        activatedStelesCount++; // Incrémenter le nombre de stèles activées

        Debug.Log("Stèle activée ! Total activé : " + activatedStelesCount);

        // Si toutes les stèles sont activées
        if (activatedStelesCount >= steles.Length)
        {
            AllStelesActivated();
        }
    }

    // Fonction déclenchée lorsque toutes les stèles sont activées
    private void AllStelesActivated()
    {
        Debug.Log("Toutes les stèles sont activées !");
        // Ici, tu peux déclencher une action (ouvrir une porte, jouer un son, etc.)
    }
}
