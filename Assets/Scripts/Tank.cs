using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    public int health;
    public float speed;
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
    int timeBeforeBullets;
    public GameObject bullet;

    // Use this for initialization
    void Start() {
        timeBeforeBullets = 0;

        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        statusCondition = (int)condition.Staying;
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        InvokeRepeating("randobMoving", 0, Random.Range(3f, 5f));
        InvokeRepeating("randomShooting", 0, Random.Range(3f, 6f));

    }

    // Update is called once per frame
    void Update() {

        updateMoving();
        checkBorders();
        CheckisDead();
    }


    void updateMoving()
    {
        transform.position = new Vector3(x, y);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);

        playerAnimator.SetInteger("statusCondition", statusCondition);

        if (moveDirection == 0)
        {
            moveX();
        }
        else if (moveDirection == 1) {
            moveY();
        }
        if ((speed == 0 || direction == 0) && health > 0)
        {
            statusCondition = (int)condition.Staying;
        }
    }


    void moveX()
    {
        x += direction * speed * Time.deltaTime;
        z = -90 * direction;
        statusCondition = (int)condition.Moving;

    }

    void moveY()
    {
        y += direction * speed * Time.deltaTime;
        if (direction < 0)
        {
            z = 180;
        }
        else
        {
            z = 0;
        }
        statusCondition = (int)condition.Moving;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        changeDirectionMirror();
        CheckHitByEnemy(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    void CheckHitByEnemy(Collision2D collision) {
        if (collision.rigidbody.CompareTag("Player")) {
            speed = 0;
            health -= health;
            
        }
        if (collision.rigidbody.CompareTag("Bullet") && !collision.gameObject.GetComponent<Bullet>().typeOfBullet.Equals("enemy"))
        {
            int hitPower = collision.gameObject.GetComponent<Bullet>().hitPower;
            health -= hitPower;
            
        }
    }

    void CheckisDead() {
        if (health == 0) {
            speed = 0;
            statusCondition = (int)condition.Badoom;
            playerAnimator.SetInteger("statusCondition", statusCondition);
            Destroy(gameObject, 1);
        }
    }

    void checkBorders()
    {
        
        if (x < minScreenBounds.x + transform.localScale.x)
        {
            
            x = minScreenBounds.x + transform.localScale.x;
            changeDirectionMirror();
            randobMoving();

        }
        else if (x > maxScreenBounds.x -transform.localScale.x)
        {
            x = maxScreenBounds.x - transform.localScale.x;
            changeDirectionMirror();
            randobMoving();

        }
        else if (y > maxScreenBounds.y - transform.localScale.y)
        {
            y = maxScreenBounds.y - transform.localScale.y;
            changeDirectionMirror();
            randobMoving();

        }
        else if (y < minScreenBounds.y + transform.localScale.y)
        {
            y = minScreenBounds.y + transform.localScale.y;
            changeDirectionMirror();
            randobMoving();
        }
    }


    void randobMoving() {
       
        int[] directionArr = {-1, 1};
        direction = directionArr[(int)System.Math.Round((double)Random.value)];
        moveDirection = (int)System.Math.Round((double)Random.value);
        

    }

    void randomShooting() {
        shotBullet();
    }

    void changeDirectionMirror() {
        direction = -direction;
        if (moveDirection == 0)
        {
            moveDirection = 1;
        }
        else if (moveDirection == 1)
        {
            moveDirection = 0;
        }
        randobMoving();
    }

    void shotBullet()
    {
        string typeOfBullet = "enemy";
        GameObject bul = Instantiate(bullet, transform.localPosition, transform.rotation);
        bul.GetComponent<Bullet>().typeOfBullet = typeOfBullet;
        bul.GetComponent<Bullet>().moveDirection = moveDirection;
        bul.GetComponent<Bullet>().direction = direction;
        timeBeforeBullets = 1;
        Invoke("RunTimerBullets", timeBeforeBullets);


    }

    void RunTimerBullets()
    {
        if (timeBeforeBullets > 0)
        {
            timeBeforeBullets -= 1;
        }

    }

}
