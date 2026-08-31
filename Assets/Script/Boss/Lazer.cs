using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Lazer : MonoBehaviour
{
    int ATK;

    // Start is called before the first frame update
    void Start()
    {
        ATK = 50;
        StartCoroutine(Collision());
        StartCoroutine(Destroy(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Ä²¸I¨ìª±®a
        if (collider.gameObject.tag == "Player")
        {
            PlayerManager.HP -= ATK;
        }
    }

    IEnumerator Collision()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
