using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager2 : ItemManager
{
    public Vector3[] itemRotationList;

    public override void SpawnCurrentItem(){
        // vytvor nový itemDestination a polož ho na svoju polohu
        GameObject newDestination = Instantiate(destinationPrefab);
        newDestination.transform.position = itemDestinationList[currentItemIdx];
        newDestination.transform.rotation = Quaternion.Euler(itemRotationList[currentItemIdx]);
        newDestination.GetComponent<ItemDestination>().itemManager = this;
        newDestination.GetComponent<ItemDestination>().SetMesh(itemList[currentItemIdx]);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCurrentItem();
        
    }

}
