using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RestaurantManager : MonoBehaviour
{
    // D�clarations des variables 
    public GameObject clientPrefabs; // R�f�rence au prefab client
    public Transform spawnPosition; // Positions ou les clients spawneront
    public float spawnInterval = 3f;
    private GameObject client;

    // File d'attente des clients
    private Queue<GameObject> fileAttente = new Queue<GameObject>();

    // Liste des clients actuellement dans le restaurant
    public List<GameObject> clientResto = new List<GameObject>();

    // Tableau des tables disponibles
    private GameObject[] tables;

    // Dictionnaire pour suivre quelle table est occup�e et par qui 
    private Dictionary<GameObject, GameObject> tablesOccuper = new Dictionary<GameObject, GameObject>();

    
    void Start()
    {
        // Initialisation des tables sur la sc�ne
        tables = GameObject.FindGameObjectsWithTag("Table");

        Debug.Log("Table trouver : " + tables.Length);

        // Lancer la g�n�ration automatique des clients
        StartCoroutine(SpawnClient());
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            ClientComing();
        }
    }

    // Coroutine qui g�n�re le spawn des client tous les X temps
    private IEnumerator SpawnClient()
    {
        while (clientResto.Count <= 5)
        {
            yield return new  WaitForSeconds(spawnInterval);
            AddClientFile();
        }
    }

    // Ajout un client dans la file d'attente
    private void AddClientFile()
    {
        // Instanciation du client dans la zone spawnPosition
        int i = 1;
        GameObject newClient = Instantiate(clientPrefabs, spawnPosition.position, Quaternion.identity);
       
        newClient.name = "Client " + i++;

        // File indienne
        Vector3 positionFile = spawnPosition.position + new Vector3(fileAttente.Count * 2, 0, 0); 
        newClient.transform.position = positionFile;

        // Ajout du nouveau client dans la file
        fileAttente.Enqueue(newClient); 
        Debug.Log("Client ajout� dans la file");
    }

    // Faire entrer un client s'il y a une table de libre
    private void ClientComing()
    {
        
        if (fileAttente.Count > 0)
        {
            //Debug.Log("Tentative de faire entrer un client..");
            // Retire le client en t�te de file
            client = fileAttente.Dequeue();

            // Ajout le client dans la Liste des client dans le restaurant
            clientResto.Add(client);

            // Trouver une table libre
            foreach (GameObject table in tables)
            {
                // Si une table est libre
                if (!tablesOccuper.ContainsKey(table))
                {
                    // R�cup�re le NavMeshAgent pour le mettre sur le client qui rentre dans le resto
                    NavMeshAgent agent = client.GetComponent<NavMeshAgent>();

                    // Si agent diff�rent de null
                    if (agent != null)
                    {
                        // D�placer le client vers une table
                        agent.SetDestination(table.transform.position);
                        
                    }
                    else
                    {
                        Debug.LogError($"{client.name} n'a pas de NavMeshAgent !");
                    }

                    // Marquer la table occuper
                    tablesOccuper.Add(table, client);
                    Debug.Log($"{table.name} est occup�e par {client.name}");
                    return;

                }

            }


        }


    }

}
