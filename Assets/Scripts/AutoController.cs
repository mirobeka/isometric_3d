using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoController : MonoBehaviour
{
    public float speed = 4f;

    // direction:
    //   0 -> forward
    //   1 -> backward
    public int direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        // správne otoč vozidlo podla smeru
        if (direction == 0){
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }else{
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + -transform.forward * speed * Time.deltaTime;
    }
}
