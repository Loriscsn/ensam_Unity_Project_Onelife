using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // R�f�rence au prefab de votre syst�me de particules
    public Transform handTransform; // Transform de la main o� les particules partent
    private PickUpDrop pickUpDrop; // R�f�rence au script de prise/d�pose

    public int portee = 10;

    void Start()
    {
        // Initialiser le script PickUpDrop
        pickUpDrop = GetComponent<PickUpDrop>();
    }

    void Update()
    {
        // V�rifier si le joueur clique avec le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Lancer le syst�me de particules si le joueur n'a pas de torche
        if (!pickUpDrop.IsHoldingTorch())
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward); // Un raycast partant de la position du joueur

            // V�rifier si le raycast touche un ennemi
            if (Physics.Raycast(ray, out hit, portee)) // Ajustez la port�e si n�cessaire
            {
                if (hit.collider.CompareTag("Enemy")) // V�rifiez que l'objet touch� est un ennemi
                {
                    StartCoroutine(LaunchParticles(hit.point)); // Lancer la coroutine pour les particules
                    Debug.Log("Attaque lanc�e sur : " + hit.collider.gameObject.name);
                }
            }
        }
    }

    IEnumerator LaunchParticles(Vector3 targetPosition)
    {
        Debug.Log("Lancement des particules vers " + targetPosition);
        // Cr�ez une instance du syst�me de particules
        ParticleSystem particles = Instantiate(particleSystemPrefab, handTransform.position, Quaternion.identity);

        // Assurez-vous que les particules ne sont pas visibles au d�but
        particles.Stop();
        Debug.Log("Syst�me de particules instanci�.");

        // D�marrer les particules
        particles.Play();
        Debug.Log("Syst�me de particules jou�.");

        // Lancer les particules vers la position cible
        float duration = 1f; // Dur�e du mouvement
        float elapsedTime = 0f;
        Vector3 startPosition = handTransform.position;
        Vector3 direction = (targetPosition - startPosition).normalized;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Calculer la position avec une courbe (ici une simple courbe quadratique)
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t) + new Vector3(0, Mathf.Sin(t * Mathf.PI) * 1, 0); // Ajustez le 1 pour la hauteur de la courbe
            particles.transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurez-vous que le syst�me de particules est d�truit apr�s utilisation
        Destroy(particles.gameObject, 2f); // D�taille le temps apr�s lequel le syst�me de particules sera d�truit
    }
}
