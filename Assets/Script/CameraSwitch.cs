using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    CinemachineVirtualCamera VirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        //設定攝影機
        VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            //設定新的攝影機
            VirtualCamera.Priority = 20;
            VirtualCamera.Follow = GameObject.Find("Player").transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //復原攝影機的優先順序
            VirtualCamera.Priority = 10;
        }
    }
}
