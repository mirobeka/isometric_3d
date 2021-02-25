using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramManager : MonoBehaviour
{
    public GameObject tramPrefab;
    public Vector3 spawnPoint;

    GameObject tram;

    // Start is called before the first frame update
    void Start()
    {
        SpawnTram();
    }

    // Update is called once per frame
    void Update()
    {
        if (tram == null){
            SpawnTram();
        }
    }

    void SpawnTram(){
        tram = Instantiate(tramPrefab, spawnPoint, Quaternion.identity, this.transform);
    }
}
