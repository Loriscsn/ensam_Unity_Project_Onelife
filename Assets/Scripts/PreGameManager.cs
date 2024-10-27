using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameManager : MonoBehaviour
{
    public GameObject[] screens;  // Tableau pour stocker les écrans de présentation
    private int currentScreenIndex = 0;  // Index pour suivre l'écran actuel

    void Start()
    {
        // Activer uniquement l'écran titre au début
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(i == currentScreenIndex);
        }
    }

    // Fonction pour aller à l'écran suivant
    public void ShowNextScreen()
    {
        if (currentScreenIndex < screens.Length - 1)
        {
            screens[currentScreenIndex].SetActive(false);  // Désactiver l'écran actuel
            currentScreenIndex++;  // Passer à l'écran suivant
            screens[currentScreenIndex].SetActive(true);  // Activer l'écran suivant
        }
        else
        {
            // Si c'est le dernier écran, lancer la scène de jeu
            StartGame();
        }
    }

    // Fonction pour revenir à l'écran précédent
    public void ShowPreviousScreen()
    {
        if (currentScreenIndex > 1)  // On ne veut pas pouvoir revenir sur l'écran titre
        {
            screens[currentScreenIndex].SetActive(false);  // Désactiver l'écran actuel
            currentScreenIndex--;  // Revenir à l'écran précédent
            screens[currentScreenIndex].SetActive(true);  // Activer l'écran précédent
        }
    }

    // Fonction pour démarrer le jeu
    public void StartGame()
    {
        SceneManager.LoadScene("Build 1");  // Remplace "GameScene" par le nom exact de ta scène de jeu
    }
}
