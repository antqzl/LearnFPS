using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Health & Damage")]

    private int maxHealth = 100;

    public int currentHealth;

    public GameObject playerDamage;

    public HealthBar healthBar;

    public GunInventory gunInventory;


    void Start()
    {
        
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(maxHealth);
        gunInventory = GetComponent<GunInventory>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        StartCoroutine(PlayerDamage());

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            Debug.Log("Take damage can");
            gunInventory.DeadMethod();

        }

    }



    IEnumerator PlayerDamage()
    {
        playerDamage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        playerDamage.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Die()
    {
        Debug.Log("Player Died");
        //GetComponent<MouseMovement>().enabled = false;
        //GetComponent<PlayerMovement>().enabled = false;
        //GetComponent<Animator>().enabled = true;
        LoadScene("YouDied");


    }
}
