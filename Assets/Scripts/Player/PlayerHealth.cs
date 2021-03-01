using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public HealthBar healthBar;

    public Renderer mesh;
    public Color collideColor;

    private Color normalColor;

    void Start()
    {
        normalColor = mesh.material.color;
        healthBar.SetMaxHealth(health);
    }


    public void TakeDamage(float amount)
    {
        health -= amount*0.1f;
        healthBar.SetHealth(health);

        // takes damage - flash the player
        StartCoroutine("Flasher");
        
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }

    IEnumerator Flasher() 
    {
        for (int i = 0; i < 3; i++)
        {
            mesh.material.color = collideColor;
            yield return new WaitForSeconds(.1f);
            mesh.material.color = normalColor; 
            yield return new WaitForSeconds(.1f);
        }
    }
}
