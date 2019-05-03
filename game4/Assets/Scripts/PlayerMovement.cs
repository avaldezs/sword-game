using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    float speed_f = 0.49f;
    Animator anim;
    Rigidbody playerRigidbody;
    public float rotationSpeed;
    public float translationSpeed;
    int floorMask;
    float camRayLength = 100f;
    public float runSpeed;
    public Text grabSword;
    public GameObject weapon;
    public Text instruction;
    private GameObject enemy;
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
    }
    void Awake()
    {
        weapon.SetActive(false);
        anim = GetComponent<Animator>();
        enemy = GameObject.FindWithTag("enemy");

        anim.SetBool("Static_b", false);
        anim.SetBool("Jump_b", false);

        floorMask = LayerMask.GetMask("Floor");

        playerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        setSpeed(translation, rotation);
        Move(translation, rotation);
       
        Turning();
        Animation();

        
       if (enemy)
       {
            Enemy EnemyScript = enemy.GetComponent<Enemy>();
            EnemyScript.target = transform;

            //Makes the enemy continuously face the player
            Vector3 targetRotation = new Vector3(transform.position.x, enemy.transform.position.y, transform.position.z);
            enemy.transform.LookAt(targetRotation);
       }

    }

    void setSpeed(float translation, float rotation)
    {
        bool walking = System.Math.Abs(translation) > Mathf.Epsilon ||
            System.Math.Abs(rotation) > Mathf.Epsilon;

        if(walking == true && Input.GetKey(KeyCode.LeftShift))
        {
            speed_f = runSpeed;
        }
        else if(walking == true)
        {
            speed_f = walkSpeed;
        }
        else if(walking == false)
        {
            speed_f = 0.0f;
        }
    }

    void Move(float translation, float rotation)
    {
        anim.SetFloat("Speed_f", speed_f);
    }

    void Animation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Jump_b", true);
        }
        else if (!Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Jump_b", false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetBool("Crouch_b", true);
        }
        else if (!Input.GetKeyDown(KeyCode.X))
        {
            anim.SetBool("Crouch_b", false);
        }
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = -7.0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRigidbody.MoveRotation(newRotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            grabSword.text = "Press E to grab your weapon";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            grabSword.text = "";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                grabSword.text = "";
                instruction.text = "";
                other.gameObject.SetActive(false);
                weapon.SetActive(true);
            }
        }
    }
}
