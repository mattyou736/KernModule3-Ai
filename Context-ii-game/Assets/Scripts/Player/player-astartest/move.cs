using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    public float currentSpeed;
    public float normalSpeed;
    public float poweredUpSpeed;

    public string lvlToStart;
    public string gameoverScene;
    //rigidbody
    private Rigidbody RigidPlayer;


    //movement check
    private bool Moving, shooting;
    public Vector2 lastMove;

    private static bool playerExists;

    public bool canMove;

    public Camera mainCam;

    public GameObject stunPartic;

    public bool canShoot;
    public GameObject bulletPrefab;
    public Transform firePoint, firePoint2, firePoint3, firePoint4;
    [Header("Time in sec")]
    public float setCooldown;
    [HideInInspector] public bool fire;
    private float cooldown = 0;

    public Animator anim;
    public int bullets;
    public int locksHit;
    bool unlock;

    public int keyAmount;
    public GameObject finalWall;
    public bool keysUsed;

    public GameObject door1,door2,door3,door4;

    private void Awake()
    {
        cooldown = setCooldown;
    }

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator> ();
        RigidPlayer = GetComponent<Rigidbody>();
        canMove = true;

        

    }

    // Update is called once per frame
    void Update()
    {
        if(locksHit >= 5)
        {
            unlock = true;
            door1.SetActive(false);
            door2.SetActive(false);
            door3.SetActive(false);
            door4.SetActive(false);
            locksHit = 0;
            
        }

        if(keyAmount >= 5 && !keysUsed)
        {
            finalWall.SetActive(false);
            keysUsed = true;
        }

        if (!canMove)
        {
            RigidPlayer.velocity = Vector3.zero;
            stunPartic.SetActive(true);
            StartCoroutine(ResetMovement());
            return;
        }

        //aiming
        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(camRay, out rayLength))
        {
            Vector3 pointToLook = camRay.GetPoint(rayLength);
            Debug.DrawLine(camRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //movements

        //left right
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal")* moveSpeed * Time.deltaTime , 0, 0));
            RigidPlayer.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * currentSpeed, RigidPlayer.velocity.y, RigidPlayer.velocity.z);
            Moving = true;
            //lastMove = new Vector2(Input.GetAxisRaw ("Horizontal"),0f);
        }

        //up down
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.Translate(new Vector3(0, Input.GetAxisRaw("Vertical")* moveSpeed * Time.deltaTime , 0));
            RigidPlayer.velocity = new Vector3(RigidPlayer.velocity.x, 0, Input.GetAxisRaw("Vertical") * currentSpeed);
            Moving = true;
            //lastMove = new Vector2(0f,Input.GetAxisRaw ("Vertical"));
        }

        //stp moving
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            RigidPlayer.velocity = new Vector3(0f, RigidPlayer.velocity.y, RigidPlayer.velocity.z);

        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            RigidPlayer.velocity = new Vector3(RigidPlayer.velocity.x, RigidPlayer.velocity.y, 0f);

        }

        if (Input.GetMouseButton(0))
        {
            if(bullets > 0)
            {
                Shoot();
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        anim.SetBool("moving", Moving);
        anim.SetBool("shooting", shooting);
        Moving = false;
        shooting = false;
    }

    void Shoot()
    {
        cooldown -= Time.deltaTime;
        shooting = true;
        if (cooldown <= 0)
        {
            cooldown = setCooldown;
            bullets--;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
            
    }

    IEnumerator ResetMovement()
    {
        yield return new WaitForSeconds(3);
        canMove = true;
        stunPartic.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FinalGoal")
        {
            SceneManager.LoadScene(lvlToStart);
        }

        if (other.tag == "enemyTrigger")
        {
            SceneManager.LoadScene(gameoverScene);
            /*if (canMove == true)
            {
                canMove = false;
            }
            Destroy(other.GetComponentInParent<Unit>().gameObject);*/
        }

        if (other.tag == "Key")
        {
            keyAmount++;
            Destroy(other.gameObject);
            
        }

        if (other.tag == "Battery")
        {
            bullets = 5;
            Destroy(other.gameObject);
        }
    }
}
