using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Assure-toi d'avoir cette ligne pour charger les sc�nes

public class MainMenu : MonoBehaviour
{
    public GameObject tuto;
    // Fonction pour le bouton Start
    public void StartGame()
    {
        // Charge la premi�re sc�ne du jeu (remplace "GameScene" par le nom exact de ta sc�ne)
        SceneManager.LoadScene("Build 1"); // Assure-toi que "GameScene" correspond bien � la sc�ne de ton jeu
        //tuto.SetActive(true);
    }

    // Fonction pour le bouton Quit
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu"); // Juste pour tester dans l'�diteur
        Application.Quit(); // Quitte le jeu (ne fonctionne que dans un build)
    }
}
