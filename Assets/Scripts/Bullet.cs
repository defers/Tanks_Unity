using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int speed;
    public int direction;
    public int moveDirection;
    public int health;
    public int hitPower;
    public float x;
    public float y;
    public float z;
    public string typeOfBullet;
    public Animator bulletAnimator;
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;
    public Bullet(string typeOfBullet): base() {
        this.typeOfBullet = typeOfBullet;
    }
    // Use this for initialization
    void Start () {
        hitPower = 1;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    }
	
	// Update is called once per frame
	void Update () {
        updateMoving();
        checkBorders();
        CheckisDead();
    }

    void updateMoving()
    {
        transform.position = new Vector3(x, y);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);

      

        if (moveDirection == 0)
        {
            moveX();
        }
        else if (moveDirection == 1)
        {
            moveY();
        }

    }

    void moveX()
    {
        x += direction * speed * Time.deltaTime;
        z = -90 * direction;
  

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

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckHitByEnemy(collision);
    }

    void CheckHitByEnemy(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !typeOfBullet.Equals("player"))
        {
            speed = 0;
            health -= health;
        }
        else if (collision.gameObject.CompareTag("Enemy") && !typeOfBullet.Equals("enemy"))
        {
            speed = 0;
            health -= health;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            speed = 0;
            health -= health;
        }


    }

    void CheckisDead()
    {
        if (health == 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    void checkBorders()
    {

        if (x < minScreenBounds.x + transform.localScale.x)
        {

            x = minScreenBounds.x + transform.localScale.x;
            speed = 0;
            health -= health;

        }
        else if (x > maxScreenBounds.x - transform.localScale.x)
        {
            x = maxScreenBounds.x - transform.localScale.x;
            speed = 0;
            health -= health;

        }
        else if (y > maxScreenBounds.y - transform.localScale.y)
        {
            y = maxScreenBounds.y - transform.localScale.y;
            speed = 0;
            health -= health;

        }
        else if (y < minScreenBounds.y + transform.localScale.y)
        {
            y = minScreenBounds.y + transform.localScale.y;
            speed = 0;
            health -= health;
        }
    }

}

