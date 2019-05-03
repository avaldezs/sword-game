using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    Animator anim;                                              // Reference to the Animator component.
    PlayerMovement playerMovement;                              // Reference to the player's movement.
    public bool isDead;                                         // Whether the player is dead.
    public GameObject deathcam;                                 // 3rd person camera to show the player's death.
    public Text YouDied;                                        // UI Text to tell the player that they have perished.
    public GameObject weapon;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        
        // Set the initial health of the player.
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        // Reduce the current health by the damage amount.
        currentHealth -= amount;
        //Debug.Log(currentHealth);

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            Debug.Log("Calling Death. Isdead = " + isDead);
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetBool("Death_b", true);
        anim.SetInteger("DeathType_int", 1);

        weapon.SetActive(false);

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;

        deathcam.SetActive(true);
        playerMovement.GetComponent<Camera>().gameObject.SetActive(false);
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            TakeDamage(25);
        }
    }
}