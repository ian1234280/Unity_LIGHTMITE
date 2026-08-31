using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ÂÂ
        //StartCoroutine(Destroy(2f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.Find("Player").transform.position;
    }

    IEnumerator Destroy(float S)
    {
        yield return new WaitForSeconds(S);
        Destroy(gameObject);
    }
}
