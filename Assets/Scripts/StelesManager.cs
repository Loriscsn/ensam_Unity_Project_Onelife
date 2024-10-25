using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StelesManager : MonoBehaviour
{
    public SteleActivation[] steles; // Tableau de toutes les st�les dans la sc�ne
    private int activatedStelesCount = 0;  // Compteur de st�les activ�es
    public TorchLife torchLife; // R�f�rence au script TorchLife

    void Start()
    {
        // V�rifier que toutes les st�les sont assign�es
        if (steles.Length == 0)
        {
            Debug.LogError("Aucune st�le n'est assign�e au manager !");
        }

        // V�rifier si la r�f�rence � la torche est assign�e
        if (torchLife == null)
        {
            Debug.LogError("La r�f�rence � TorchLife n'est pas assign�e !");
        }
    }

    // Fonction appel�e par chaque st�le lorsqu'elle est activ�e
    public void NotifySteleActivated()
    {
        activatedStelesCount++; // Incr�menter le nombre de st�les activ�es

        Debug.Log("St�le activ�e ! Total activ� : " + activatedStelesCount);

        // Ajoute des points de vie � la torche
        if (torchLife != null)
        {
            torchLife.AddLifeFromStele(); // Appel de la m�thode d'ajustement de vie
        }

        // Si toutes les st�les sont activ�es
        if (activatedStelesCount >= steles.Length)
        {
            AllStelesActivated();
        }
    }

    // Fonction d�clench�e lorsque toutes les st�les sont activ�es
    private void AllStelesActivated()
    {
        Debug.Log("Toutes les st�les sont activ�es !");
        // Ici, tu peux d�clencher une action (ouvrir une porte, jouer un son, etc.)
    }
}
