using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientScript : MonoBehaviour
{
    public bool isServe = false; // Le client n'est pas servi
    public float fadeSpeed = 0.5f; // La vitesse à laquelle ont réduit le fondu

    private Renderer clientRenderer; // Renderer du gameObject Client
    private Color originalColor; // Couleur du client
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        clientRenderer = GetComponent<Renderer>();

        if (clientRenderer != null)
        {
            // Chemin vers la couleur du renderer du gameObject Client
            originalColor = clientRenderer.material.color;
            //StartCoroutine(FadeOut());
        }
    }

    public void Servir()
    {
        if (!isServe)
        {
            isServe = true;
            StartCoroutine(WaitBeforeGo());

        }
    }


    private IEnumerator WaitBeforeGo()
    {
        yield return new WaitForSeconds(2f); // Le client attend quelque second avant de partir
        LeaveResto();
    }

    // Fondu de l'alpha du joueur avant d'être destroye
    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");
        float alpha = originalColor.a;

        yield return new WaitForSeconds(10f); // Attend avant de commencer le fondu.

        // Tant que l'alpha du material est supérieur à 0
        while (originalColor.a > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 1);
            clientRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;


            Debug.Log($"Fondu en cours... Alpha = {alpha}");


        }

        Debug.Log("Destroy");
        Destroy(gameObject, 0.5f); // Détruire le Client
        





    }

    // Le client part
    public void LeaveResto()
    {
        GameObject sortie = GameObject.FindGameObjectWithTag("Sortie");

        if (sortie != null)
        {
            agent.SetDestination(sortie.transform.position);
            StartCoroutine(FadeOut());

        }
    }


}
