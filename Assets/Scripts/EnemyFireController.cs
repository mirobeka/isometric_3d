using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameObject attackPoint;
    public float shootForce = 7f;
    public int rateOfFire = 15;

    // Start is called before the first frame update
    void Start()
    {
        float delay = 60/rateOfFire;
        Invoke("Shoot", delay);
    }


    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.transform.position, attackPoint.transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * shootForce, ForceMode.Impulse);

        float delay = 60/rateOfFire;
        delay = Random.Range(delay-1, delay+1);
        Invoke("Shoot", delay);
    }
}
