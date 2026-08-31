using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCannon : MonoBehaviour
{
    float Speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
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
}
