using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    CinemachineConfiner confiner;
    Animator Transition;

    public int sceneNumber;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        //設定新場景攝影機的邊界
        confiner = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = GameObject.Find("Camera Bounds").GetComponent<Collider2D>();

        //獲得場景過度的動畫器
        Transition = GameObject.Find("Transition").GetComponent<Animator>();
        Transition.Play("Transition_End", 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("Virtual Camera").transform.position = new Vector3(0,0,0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            Transition.Play("Transition_Start", 0, 0f);
            Invoke("Teleportation", 0.5f);
        }
    }

    void Teleportation()
    {
        SceneManager.LoadScene(sceneNumber);
        GameObject.Find("Player").transform.position = targetPosition;
        //GameObject.Find("Main Camera").transform.position = targetPosition;
    }
}
