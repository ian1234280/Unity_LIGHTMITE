using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerSeriesEnergyCannon : MonoBehaviour
{
    int ATK;

    float Speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        ATK = 25;

        StartCoroutine(Destroy(2f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    IEnumerator Destroy(float S)
    {
        yield return new WaitForSeconds(S);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Ä²¸I¨ìª±®a
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.HP -= ATK;
        }
    }
}
