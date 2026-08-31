using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leno : MonoBehaviour
{
    //飄浮的高度
    float floatAmplitude = 0.25f;
    //飄動的速度
    float floatSpeed = 2f;
    //初始位置
    Vector3 startPosition;

    //事件狀態
    static public int Event = 0;

    // Start is called before the first frame update
    void Start()
    {
        //記錄初始位置
        startPosition = transform.position;

        //偵測事件狀態
        switch (Event)
        {
            case 0:
                GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("初見雷諾");
                if (SceneManager.GetActiveScene().buildIndex == 13)
                {
                    gameObject.SetActive(false);
                }
                break;
            case 1:
                AddEvent();
                if (SceneManager.GetActiveScene().buildIndex == 7)
                {
                    gameObject.SetActive(false);
                }
                break;
            case 2:
                if (SceneManager.GetActiveScene().buildIndex == 7)
                {
                    gameObject.SetActive(false);
                }
                break;
            case 3:
                gameObject.SetActive(false);
                break;
            case 4:
                
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        TalkTrigger();
    }

    void Float()
    {
        // 使用 Mathf.Sin 來計算平滑的上下運動
        float floatY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 設定新位置
        transform.position = new Vector3(transform.position.x, floatY, transform.position.z);
    }

    void TalkTrigger()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (Event)
            {
                case 0:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("初見雷諾");
                    break;
                case 1:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("重複對話");
                    break;
                case 2:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("場景13雷諾");
                    break;
                case 3:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("場景13重複對話");
                    break;
            }
        }
        else if(GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //GameObject.Find("LenoUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }

    }

    public void AddEvent()
    {
        Event += 1;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("LenoUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //離開NPC停止對話
        if (collider.tag == "Player")
        {
            GameObject.Find("LenoUpButton").GetComponent<Animator>().Play("ButtonDisappears");
            GameObject.Find("Flowchart").GetComponent<Flowchart>().StopAllBlocks();
        }
    }
}
