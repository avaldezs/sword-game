using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int kills;
    public int maxKills;
    Text killCounter;
    PlayerMovement playerMovement;
    Text WinText;
    public bool win = false;
    public Camera DeathCam;
    PlayerHealth playerHealth;
    Animator anim;
    public GameObject weapon;
    Scene loadedScene = SceneManager.GetActiveScene();
    Text health;

    // Start is called before the first frame update
    void Start()
    {

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        killCounter = GameObject.Find("KillCount").GetComponent<Text>();
        WinText = GameObject.Find("WinText").GetComponent<Text>();
        //DeathCam = GameObject.Find("DeathCam").GetComponent<Camera>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        //weapon = GameObject.Find("weapon");
        health = GameObject.Find("healthText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = playerHealth.currentHealth.ToString();
        killCounter.text = "Kills: " + kills.ToString();

        if (playerHealth.isDead)
        {
            playerHealth.YouDied.text = "You Died, Press R to retry";
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(loadedScene.buildIndex);
            }
        }
        if (kills == maxKills)
        {
            win = true;
        }

        if (win)
        {
            anim.SetFloat("Speed_f", 0f);
            Debug.Log("Win is: " + win);
            weapon.SetActive(false);
            DeathCam.gameObject.SetActive(true);
            anim.SetInteger("Animation_int", 4);
            Destroy(playerMovement);
            WinText.text = "You Win!\n Press R to retry";
            
            
            
        }

        
    }
}
