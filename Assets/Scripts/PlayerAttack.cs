using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab; // Référence au prefab de votre système de particules
    public Transform handTransform; // Transform de la main où les particules partent
    private PickUpDrop pickUpDrop; // Référence au script de prise/dépose

    public int portee = 10;

    void Start()
    {
        // Initialiser le script PickUpDrop
        pickUpDrop = GetComponent<PickUpDrop>();
    }

    void Update()
    {
        // Vérifier si le joueur clique avec le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Lancer le système de particules si le joueur n'a pas de torche
        if (!pickUpDrop.IsHoldingTorch())
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward); // Un raycast partant de la position du joueur

            // Vérifier si le raycast touche un ennemi
            if (Physics.Raycast(ray, out hit, portee)) // Ajustez la portée si nécessaire
            {
                if (hit.collider.CompareTag("Enemy")) // Vérifiez que l'objet touché est un ennemi
                {
                    StartCoroutine(LaunchParticles(hit.point)); // Lancer la coroutine pour les particules
                    Debug.Log("Attaque lancée sur : " + hit.collider.gameObject.name);
                }
            }
        }
    }

    IEnumerator LaunchParticles(Vector3 targetPosition)
    {
        Debug.Log("Lancement des particules vers " + targetPosition);
        // Créez une instance du système de particules
        ParticleSystem particles = Instantiate(particleSystemPrefab, handTransform.position, Quaternion.identity);

        // Assurez-vous que les particules ne sont pas visibles au début
        particles.Stop();
        Debug.Log("Système de particules instancié.");

        // Démarrer les particules
        particles.Play();
        Debug.Log("Système de particules joué.");

        // Lancer les particules vers la position cible
        float duration = 1f; // Durée du mouvement
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

        // Assurez-vous que le système de particules est détruit après utilisation
        Destroy(particles.gameObject, 2f); // Détaille le temps après lequel le système de particules sera détruit
    }
}
