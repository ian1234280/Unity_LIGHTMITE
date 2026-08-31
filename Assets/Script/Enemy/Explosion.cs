using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    int ATK;

    // Start is called before the first frame update
    void Start()
    {
        ATK = 50;

        CameraShake.Instance.Explosion.GenerateImpulse();
        StartCoroutine(DisableCollision(0.2f));
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

    IEnumerator DisableCollision(float T)
    {
        yield return new WaitForSeconds(T);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
