using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StelesManager : MonoBehaviour
{
    public SteleActivation[] steles; // Tableau de toutes les stèles dans la scène
    private int activatedStelesCount = 0;  // Compteur de stèles activées
    public TorchLife torchLife; // Référence au script TorchLife
    public GameObject victoryCanvas; // Référence au Canvas de victoire

    void Start()
    {
        // Vérifier que toutes les stèles sont assignées
        if (steles.Length == 0)
        {
            Debug.LogError("Aucune stèle n'est assignée au manager !");
        }

        // Vérifier si la référence à la torche est assignée
        if (torchLife == null)
        {
            Debug.LogError("La référence à TorchLife n'est pas assignée !");
        }

        // Vérifier que le Canvas de victoire est assigné
        if (victoryCanvas == null)
        {
            Debug.LogError("La référence à VictoryCanvas n'est pas assignée !");
        }
        else
        {
            victoryCanvas.SetActive(false); // S'assurer que le Canvas de victoire est désactivé au début
        }
    }

    // Fonction appelée par chaque stèle lorsqu'elle est activée
    public void NotifySteleActivated()
    {
        activatedStelesCount++; // Incrémenter le nombre de stèles activées

        Debug.Log("Stèle activée ! Total activé : " + activatedStelesCount);

        // Ajouter des points de vie à la torche
        if (torchLife != null)
        {
            torchLife.AddLifeFromStele(); // Appel de la méthode d'ajustement de vie
        }

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
        // Activer le Canvas de victoire
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);
        }
    }
}
