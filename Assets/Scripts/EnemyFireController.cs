using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameObject attackPoint;
    public float shootForce = 7f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", 3f);
    }


    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.transform.position, attackPoint.transform.rotation);

        projectile.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * shootForce, ForceMode.Impulse);

        float delay = Random.Range(3, 6);
        Invoke("Shoot", delay);
    }
}
