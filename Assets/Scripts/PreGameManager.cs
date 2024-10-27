using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameManager : MonoBehaviour
{
    public GameObject[] screens;  // Tableau pour stocker les �crans de pr�sentation
    private int currentScreenIndex = 0;  // Index pour suivre l'�cran actuel

    void Start()
    {
        // Activer uniquement l'�cran titre au d�but
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(i == currentScreenIndex);
        }
    }

    // Fonction pour aller � l'�cran suivant
    public void ShowNextScreen()
    {
        if (currentScreenIndex < screens.Length - 1)
        {
            screens[currentScreenIndex].SetActive(false);  // D�sactiver l'�cran actuel
            currentScreenIndex++;  // Passer � l'�cran suivant
            screens[currentScreenIndex].SetActive(true);  // Activer l'�cran suivant
        }
        else
        {
            // Si c'est le dernier �cran, lancer la sc�ne de jeu
            StartGame();
        }
    }

    // Fonction pour revenir � l'�cran pr�c�dent
    public void ShowPreviousScreen()
    {
        if (currentScreenIndex > 1)  // On ne veut pas pouvoir revenir sur l'�cran titre
        {
            screens[currentScreenIndex].SetActive(false);  // D�sactiver l'�cran actuel
            currentScreenIndex--;  // Revenir � l'�cran pr�c�dent
            screens[currentScreenIndex].SetActive(true);  // Activer l'�cran pr�c�dent
        }
    }

    // Fonction pour d�marrer le jeu
    public void StartGame()
    {
        SceneManager.LoadScene("Build 1");  // Remplace "GameScene" par le nom exact de ta sc�ne de jeu
    }
}
