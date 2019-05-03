using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float navigationUpdate;
    private float navigationTime = 0;
    private NavMeshAgent agent;
    bool canMove;
    Animator anim;
    public float validDistance;
    private bool isDead;
    public int health;
    int swordDamage = 10;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        canMove = true;
        anim = GetComponent<Animator>();
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove == true)
        {
            navigationTime += Time.deltaTime;   //checks to see if a certain amount of time has passed, then updates the path
            if(navigationTime > navigationUpdate)
            {
                if (target != null)
                {
                    if (Vector3.Distance(this.transform.position, target.transform.position) >= validDistance)
                    {
                        agent.destination = target.position;
                        agent.isStopped = false;
                        canMove = true;
                        anim.SetFloat("Speed_f", 0.49f);
                    }
                    else if (Vector3.Distance(this.transform.position, target.transform.position) < validDistance)
                    {
                        agent.isStopped = true;
                        canMove = false;
                    }
                    navigationTime = 0;
                }
            }
        }
        else
        {
            anim.SetFloat("Speed_f", 0.0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = true;
            agent.isStopped = false;
            navigationTime = 0;
            agent.SetDestination(target.position);
            anim.SetFloat("Speed_f", 0.49f);
            TakeDamage(swordDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        health -= amount;

        if(health <= 0)
        {
            gameController.kills++;
            Destroy(this.gameObject.GetComponent<CapsuleCollider>());
            isDead = true;
            //add the animation for the enemy to die
            anim.SetBool("Death_b", true);
            anim.SetInteger("DeathType_int", 1);
            StartCoroutine("DestroyEnemy");
        }
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
             if (Input.GetKeyDown(KeyCode.Mouse0)) {
                TakeDamage(swordDamage);
                //Debug.Log("Hit with sword");
                //Destroy(gameObject);
            }
        }
    }
}
