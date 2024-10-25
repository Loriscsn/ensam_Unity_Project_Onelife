using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Assure-toi d'avoir cette ligne pour charger les scènes

public class MainMenu : MonoBehaviour
{
    // Fonction pour le bouton Start
    public void StartGame()
    {
        // Charge la première scène du jeu (remplace "GameScene" par le nom exact de ta scène)
        SceneManager.LoadScene("GameScene"); // Assure-toi que "GameScene" correspond bien à la scène de ton jeu
    }

    // Fonction pour le bouton Quit
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu"); // Juste pour tester dans l'éditeur
        Application.Quit(); // Quitte le jeu (ne fonctionne que dans un build)
    }
}
