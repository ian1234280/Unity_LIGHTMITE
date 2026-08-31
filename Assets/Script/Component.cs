using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.Component)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Trigger();
    }

    void Trigger()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerManager.Component = true;
            //閃白動畫
            GameObject.Find("Transition").GetComponent<Animator>().Play("Charging", 0, 0f);
            //移動到入口
            GameObject.Find("Player").transform.position = new Vector3(38, 0.39f, 0);
            gameObject.SetActive(false);
        }
        else if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            GameObject.Find("ComponentUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("ComponentUpButton").GetComponent<Animator>().Play("ButtonDisappears");
        }
    }
}
