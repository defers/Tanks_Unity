using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviour {

    public int health;
    private float speed;
    public float x;
    public float y;
    public float z;
    int direction;
    int moveDirection;
    public enum condition : int { Staying, Moving, Badoom };
    public int statusCondition;
    public Animator playerAnimator;
    public Rigidbody2D playerRigidbody;
    public SpriteRenderer rect1;
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;
    public GameObject bullet;
    int timeBeforeBullets;

    // Use this for initialization
    void Start () {
        timeBeforeBullets = 0;
        speed = 1.8f;
        direction = 0;
        health = 2;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        statusCondition = (int) condition.Staying;
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    }

    // Update is called once per frame
    void Update () {

        updateMoving();
        checkBorders();
        getInput();
        CheckisDead();

    }

    private void FixedUpdate()
    {
        
    }

    void getInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = 1;
            moveY();

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = -1;
            moveY();

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = -1;
            moveX();

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1;
            moveX();

        }

        if (Input.GetKey(KeyCode.Space)) {
            if (timeBeforeBullets != 0) {
                return;
            }
            shotBullet();
        }

    }

    void updateMoving() {
        transform.position = new Vector3(x, y);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);

        if ((speed == 0 || direction == 0) && health > 0)
        {
            statusCondition = (int)condition.Staying;
        }
        playerAnimator.SetInteger("statusCondition", statusCondition);

    }

    void moveX() {
        x += direction * speed * Time.deltaTime;
        z = -90 * direction;
        statusCondition = (int)condition.Moving;
        moveDirection = 0;


    }

    void moveY()
    {
        y += direction * speed * Time.deltaTime;
        if (direction < 0){
            z = 180;
        }
        else {
            z = 0;
        }
        statusCondition = (int)condition.Moving;
        moveDirection = 1;


    }

    void doOnCollision(Collision2D collision) {
        if (x >= collision.otherRigidbody.position.x)
        {
            x = collision.otherRigidbody.position.x;
        }
        else if (x <= collision.otherRigidbody.position.x)
        {
            x = collision.otherRigidbody.position.x;
        }
        else if (y >= collision.otherRigidbody.position.y)
        {
            y = collision.otherRigidbody.position.y;
        }
        else if (y <= collision.otherRigidbody.position.y)
        {
            y = collision.otherRigidbody.position.y;
        }

        direction = -direction;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        doOnCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        doOnCollision(collision);
        CheckHitByEnemy(collision);

    }

    void CheckHitByEnemy(Collision2D collision)
    {
        if (collision.rigidbody.CompareTag("Enemy"))
        {
            speed = 0;
            health -= health;
            statusCondition = (int)condition.Badoom;
        }
        if (collision.rigidbody.CompareTag("Bullet") && !collision.gameObject.GetComponent<Bullet>().typeOfBullet.Equals("player"))
        {
            int hitPower = collision.gameObject.GetComponent<Bullet>().hitPower;
            health -= hitPower;
            
        }

    }

    void CheckisDead()
    {
        if (health == 0)
        {
            statusCondition = (int)condition.Badoom;
            playerAnimator.SetInteger("statusCondition", statusCondition);
            Destroy(gameObject, 2);
        }
    }

    void checkBorders() {

        if (x < minScreenBounds.x + transform.localScale.x)
        {

            x = minScreenBounds.x + transform.localScale.x;

        }
        else if (x > maxScreenBounds.x - transform.localScale.x)
        {
            x = maxScreenBounds.x - transform.localScale.x;


        }
        else if (y > maxScreenBounds.y - transform.localScale.y)
        {
            y = maxScreenBounds.y - transform.localScale.y;


        }
        else if (y < minScreenBounds.y + transform.localScale.y)
        {
            y = minScreenBounds.y + transform.localScale.y;

        }

     
    }

    void shotBullet() {
        string typeOfBullet = "player";
        GameObject bul = Instantiate(bullet, transform.localPosition, transform.rotation);
        bul.GetComponent<Bullet>().typeOfBullet = typeOfBullet;
        bul.GetComponent<Bullet>().moveDirection = moveDirection;
        bul.GetComponent<Bullet>().direction = direction;
        timeBeforeBullets = 1;
        Invoke("RunTimerBullets", timeBeforeBullets);


    }

    void RunTimerBullets() {
        if (timeBeforeBullets > 0) {
            timeBeforeBullets -= 1;
        }
        
 
    }

}
