using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    public int health;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckisDead();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        CheckHitByEnemy(collision);

    }

    void CheckHitByEnemy(Collision2D collision)
    {

        if (collision.rigidbody.CompareTag("Bullet"))
        {
            int hitPower = collision.gameObject.GetComponent<Bullet>().hitPower;
            health -= hitPower;

        }

    }

    void CheckisDead()
    {
        if (health == 0)
        {

            Destroy(gameObject, 1);
        }
    }
}
