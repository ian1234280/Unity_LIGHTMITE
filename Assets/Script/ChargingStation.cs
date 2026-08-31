using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class ChargingStation : MonoBehaviour
{
    public BoxCollider2D Trigger;

    public int RespawnScene;
    public Vector3 RespawnPoint;

    public AudioClip 休息點;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Charging();
            }
        }
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            GameObject.Find("ChargingStationUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void Charging()
    {
        //閃白動畫
        GameObject.Find("Transition").GetComponent<Animator>().Play("Charging", 0, 0f);

        //回復血量
        PlayerManager.HP = PlayerManager.MaxHP[PlayerManager.HPLevel];

        //回復技能點
        PlayerManager.SkillPoint = PlayerManager.MaxSkillPoint[PlayerManager.SkillPointLevel];

        //設置重生點
        PlayerManager.RespawnScene = RespawnScene;
        PlayerManager.RespawnPoint = RespawnPoint;

        //移動到重生點
        GameObject.Find("Player").transform.position = RespawnPoint;

        //音效
        GetComponent<AudioSource>().PlayOneShot(休息點);
    }

    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("ChargingStationUpButton").GetComponent<Animator>().Play("ButtonDisappears");
        }
    }
}
