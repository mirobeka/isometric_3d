using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramManager : MonoBehaviour
{
    public GameObject tramPrefab;
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;

    GameObject tram1;
    GameObject tram2;

    void Start(){
        StartCoroutine(spawnTramWithRandomDelay(1));
        StartCoroutine(spawnTramWithRandomDelay(2));
    }

    // Update is called once per frame
    void Update()
    {
        if (tram1 == null){
            spawnTramWithRandomDelay(1);
        }

        if (tram2 == null){
            spawnTramWithRandomDelay(1);
        }
    }


    IEnumerator spawnTramWithRandomDelay(int tramNumber){
        // počkaj pár sekund aby električky neboli symetricke
        int delay =  UnityEngine.Random.Range(0, 4);
        yield return new WaitForSeconds(delay);

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
