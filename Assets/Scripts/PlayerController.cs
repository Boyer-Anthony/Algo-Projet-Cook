using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject Cursor;

    private Vector3 targetCursor;  
    private Stack<GameObject> pile = new Stack<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            

            if (Physics.Raycast(ray, out hit))
            {

                
                targetCursor = hit.point;            // Recuperer la position du Cursor
                agent.SetDestination(hit.point);    // Focus la position du Cursor

                // Supprime le dernier curseur s'il existe
                if (pile.Count > 0)
                {
                    GameObject lastCursor = pile.Pop();
                    Destroy(lastCursor);
                   
                }

                // Instanciation du Cursor à la position du hit
                GameObject newCursor = Instantiate(Cursor, targetCursor, Quaternion.identity);

                // Ajouter élément dans la stack
                pile.Push(newCursor);

            }
        }


    }
}
