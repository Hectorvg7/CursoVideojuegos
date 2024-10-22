using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    public bool isPlayer1;
    public float speed;
    public Rigidbody2D rb;
    private float movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            movement = Input.GetAxisRaw("Vertical");
        }else
        {
            movement = Input.GetAxisRaw("Vertical2");
        }
        rb.velocity = new Vector2(rb.velocity.x, movement*speed);
    }
}
