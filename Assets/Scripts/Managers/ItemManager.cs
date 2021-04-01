using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ItemManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Svetielko svetielko;
    public GameObject[] itemList;
    public Vector3[] itemDestinationList;
    public GameObject destinationPrefab;
    public int currentItemIdx = 0;
    public TimelineAsset scene;

    public void ItemInPosition(){
        currentItemIdx += 1;
        if (itemList.Length <= currentItemIdx){
            Debug.Log("Koniec veci!!!");
            if(scene != null){
                GameManager.Instance.SetTimelinePlayable(scene);
            }
        }else{
            SpawnCurrentItem();
        }

    }

    public virtual void SpawnCurrentItem(){
        // vytvor nový item, polož ho do výťahu
        GameObject newItem = Instantiate(itemList[currentItemIdx]);
        newItem.transform.position = spawnPoint.transform.position;

        // vytvor nový itemDestination a polož ho na svoju polohu
        GameObject newDestination = Instantiate(destinationPrefab);
        newDestination.transform.position = itemDestinationList[currentItemIdx];
        newDestination.GetComponent<ItemDestination>().itemManager = this;
        newDestination.GetComponent<ItemDestination>().SetMesh(itemList[currentItemIdx]);
    }
}
