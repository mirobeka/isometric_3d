using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramManager : MonoBehaviour
{
    public GameObject tramPrefab;
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;

    // neviem to ani vysvetliť, proste to tu musí byť
    // lebo timing coroutines...
    private bool semaphore = false;

    GameObject tram1;
    GameObject tram2;

    void Start(){
        semaphore = true;
        StartCoroutine(spawnTramWithRandomDelay(1));
    }

    // Update is called once per frame
    void Update()
    {
        if (tram1 == null && !semaphore){
            semaphore = true;
            StartCoroutine(spawnTramWithRandomDelay(1));
        }

        if (tram2 == null && !semaphore){
            semaphore = true;
            StartCoroutine(spawnTramWithRandomDelay(2));
        }
    }


    IEnumerator spawnTramWithRandomDelay(int tramNumber){
        // počkaj pár sekund aby električky neboli symetricke
        int delay =  UnityEngine.Random.Range(5, 20);
        yield return new WaitForSeconds(delay);
        spawnTram(tramNumber);
        semaphore = false;
    }

    void spawnTram(int tramNumber){
        // vytvor električku
        if(tramNumber == 1){
            tram1 = Instantiate(tramPrefab, spawnPoint1, Quaternion.identity, this.transform);
        }else if(tramNumber == 2){
            Vector3 rotation = new Vector3(0f,-180f, 0f);
            tram2 = Instantiate(tramPrefab, spawnPoint2, Quaternion.Euler(rotation), this.transform);
            Elektricka tramComponent = tram2.GetComponent<Elektricka>();
            tramComponent.directionVector = tramComponent.directionVector * -1f;
        }

    }
}
