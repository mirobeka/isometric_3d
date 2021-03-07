using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public GameObject[] carPrefabs;
    public float roadLength = 20f;
    public int currentTileNo = 0;
    public Transform playerTransform;

    private List<GameObject> tilesList = new List<GameObject>();

    void Start(){
        SpawnTileAt(1);
    }


    // Update is called once per frame
    void Update()
    {
        // na ktorom políčku sa akurát nachádza indexované od 0
        int tileNo = (int)(playerTransform.position.x / roadLength);

        if (tileNo > currentTileNo){
            // práve si prešiel na ďalší blok
            currentTileNo = tileNo;

            // treba doplniť
            SpawnTileAt(currentTileNo + 1);
            SpawnCarsAt(currentTileNo + 1);

            // treba ubrať
            RemoveTile(currentTileNo - 1);
        }
    }

    // spawns new car at given tile position
    void SpawnCarsAt(int tileNo){
        int noOfCars = Random.Range(0, 3);
        for (int i = 0; i <= noOfCars; i++){
            int randIdx = Random.Range(0, carPrefabs.Length);
            GameObject newCar = Instantiate(carPrefabs[randIdx], this.transform);

            float randomXPosition = Random.Range(0f, roadLength);
            float zPosition = 0f;
            int direction = Random.Range(0, 2);
            if (direction == 0){
                zPosition = -12f;
            }else{
                zPosition = -8f;
            }
            newCar.GetComponent<AutoController>().direction = direction;
            newCar.transform.position = new Vector3(tileNo * roadLength + randomXPosition, 0.5f, zPosition);
        }

    }

    void SpawnTileAt(int tileNo){
        int randIdx = Random.Range(0, roadPrefabs.Length);
        GameObject newTile = Instantiate(roadPrefabs[randIdx], this.transform);
        newTile.transform.position = new Vector3(tileNo * roadLength, 0f, 0f);
        // newTile.transform.position.x = tileNo * roadLength;
        tilesList.Add(newTile);
    }

    void RemoveTile(int tileNo){
        GameObject oldTile = tilesList[tileNo];
        // Rigidbody tileRb = oldTile.AddComponent<Rigidbody>();
        // tileRb.mass = 1;
        // AddRandomPush(tileRb);
        StartCoroutine(DeleteAfterSeconds(10, oldTile));
    }

    void AddRandomPush(Rigidbody rb){
        Vector3 direction = new Vector3(0f, 10f, 20f);
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private IEnumerator DeleteAfterSeconds(int delay, GameObject go)
    {
        // počkaj na zastavke
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }
}
