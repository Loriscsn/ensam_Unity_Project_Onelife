using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 5.0f;  // Vitesse de déplacement du cube

    void Update()
    {
        // Récupérer les touches de mouvement
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Flèches gauche/droite ou touches Q/D
        float moveVertical = Input.GetAxisRaw("Vertical");     // Flèches haut/bas ou touches Z/S

        // Gestion des touches ZQSD
        if (Input.GetKey(KeyCode.Z)) moveVertical = 1;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1;
        if (Input.GetKey(KeyCode.Q)) moveHorizontal = -1;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = 1;

        // Calcul du mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Appliquer le mouvement au cube
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
