using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public bool GoLeft;

    float Speed = 150;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Ä²¸I¨ìª±®a
        if (collision.gameObject.tag == "Player")
        {
            if (GoLeft)
            {
                GameObject.Find("Player").GetComponent<Rigidbody2D>().AddForce(new Vector2(-Speed, 0), ForceMode2D.Force);
            }
            else
            {
                GameObject.Find("Player").GetComponent<Rigidbody2D>().AddForce(new Vector2(Speed, 0), ForceMode2D.Force);
            }
        }
    }
}
