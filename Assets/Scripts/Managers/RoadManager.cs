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
    public GameObject lastCar;
    public float minDistanceToLastCar = 20f;

    private List<GameObject> tilesList = new List<GameObject>();

    void Start(){
        StartCoroutine(SpawnTileAt(1));
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
            StartCoroutine(SpawnTileAt(currentTileNo + 1));
            // SpawnRandomCarsAt(currentTileNo + 1);

            // treba ubrať
            RemoveTile(currentTileNo - 1);
        }
    }

    public void SpawnCarsAtRightLane(){
        // xPosition nastavujem podla toho kde sa nachádza hráč
        // ale ešte aj trošku mágie -> autá v pravo si udržujú minimálnu
        // vzdialenosť

        float xPosition = playerTransform.position.x % roadLength;
        xPosition += (currentTileNo + 1) * roadLength;

        if (lastCar != null){
            float positionOfLastCar = lastCar.transform.position.x;
            float distanceToLastCar = xPosition - positionOfLastCar;

            if (distanceToLastCar < minDistanceToLastCar){
                // auto by sa spawnlo príliš blízko k poslednému autu
                float randomOffset = Random.Range(0f, 5f); //some random offset
                xPosition = positionOfLastCar + minDistanceToLastCar + randomOffset;
            }
        }
        
        GameObject newCar = SpawnCarAt(xPosition, 0);
        lastCar = newCar;
    }

    public void SpawnCarsAtLeftLane(){
        // xPosition nastavujem podla toho kde sa nachádza hráč
        float xPosition = playerTransform.position.x % roadLength;
        xPosition += (currentTileNo + 1) * roadLength;
        SpawnCarAt(xPosition, 1);
    }

    GameObject SpawnCarAt(float xPos, int direction){
        int randIdx = Random.Range(0, carPrefabs.Length);
        float zPosition = direction == 0 ? -12f : -8f;

        GameObject newCar = Instantiate(carPrefabs[randIdx], this.transform);
        newCar.GetComponent<AutoController>().direction = direction;
        newCar.transform.position = new Vector3(xPos, 0.5f, zPosition);
        return newCar;
    }

    IEnumerator SpawnTileAt(int tileNo){
        int randIdx = Random.Range(0, roadPrefabs.Length);
        GameObject newTile = Instantiate(roadPrefabs[randIdx], this.transform);
        newTile.transform.position = new Vector3(tileNo * roadLength, 0f, 0f);
        // newTile.transform.position.x = tileNo * roadLength;
        tilesList.Add(newTile);
        yield return "";
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
