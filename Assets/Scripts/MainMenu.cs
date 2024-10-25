using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fonction pour le bouton Start
    public void StartGame()
    {
        // Charge la première scène du jeu
        SceneManager.LoadScene("Loris"); // Assure-toi d'avoir ajouté la scène dans le build settings
    }

    // Fonction pour le bouton Quit
    public void QuitGame()
    {
        Debug.Log("Quit"); // Affiche dans la console pour les tests dans l'éditeur
        Application.Quit(); // Quitte l'application (ne fonctionne pas dans l'éditeur)
    }
}
