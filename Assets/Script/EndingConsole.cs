using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingConsole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.UpArrow) && PlayerManager.DestroyerSeriesDead)
        {
            GameObject.Find("Transition").GetComponent<Animator>().Play("Dead", 0, 0f);
            GameObject.Find("Ending1CGCanvas").GetComponent<Animator>().Play("Ending1", 0, 0f);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player" && PlayerManager.DestroyerSeriesDead)
        {
            GameObject.Find("EndingConsoleUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player" && PlayerManager.DestroyerSeriesDead)
        {
            GameObject.Find("EndingConsoleUpButton").GetComponent<Animator>().Play("ButtonDisappears");
        }
    }
}
