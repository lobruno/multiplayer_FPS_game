using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 10;
    public int damage = 25;
    public Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right * direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collide");
        if (collision.gameObject.GetComponent<PlayerController2D>())
        {
            print("zombie");
            collision.gameObject.GetComponent<PlayerController2D>().TakeDamage(25);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }


}

