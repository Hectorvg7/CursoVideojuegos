using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;


    public void Launch(){
        float x = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float y = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
    
        rb.velocity = new Vector2(x * speed, y * speed);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = rb.velocity * 1.1f;
        }
        else if (collision.gameObject.CompareTag("Field"))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
