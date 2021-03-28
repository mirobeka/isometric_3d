using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Svetielko svetielko;
    public GameObject[] itemList;
    public Vector3[] itemDestinationList;
    public GameObject destinationPrefab;
    private int currentItemIdx = 0;

    public void ItemInPosition(){
        currentItemIdx += 1;
        if (itemList.Length <= currentItemIdx){
            Debug.Log("Koniec veci!!!");
            // TODO spusti scénu
        }else{
            SpawnCurrentItem();
        }

    }

    public void SpawnCurrentItem(){
        // vytvor nový item, polož ho do výťahu
        GameObject newItem = Instantiate(itemList[currentItemIdx]);
        newItem.transform.position = spawnPoint.transform.position;

        // vytvor nový itemDestination a polož ho na svoju polohu
        GameObject newDestination = Instantiate(destinationPrefab);
        newDestination.transform.position = itemDestinationList[currentItemIdx];
        newDestination.GetComponent<ItemDestination>().itemManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCurrentItem();
        
    }

}
