using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kara : MonoBehaviour
{
    //事件狀態
    static public int Event = 0;

    static public bool isRepeat;
    public GameObject HealTutorial;
    public GameObject MapTutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TalkTrigger();
        Talking();
    }

    void TalkTrigger()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (Event)
            {
                case 0:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("初見卡菈");
                    break;
                case 1:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("給地圖");
                    break;
                case 2:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("重複對話");
                    break;
            }
        }
        else if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //GameObject.Find("KaraUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void Talking()
    {
        if (FindObjectOfType<Flowchart>().HasExecutingBlocks())
        {
            GetComponent<Animator>().Play("Talking");
        }
        else
        {
            GetComponent<Animator>().Play("null");
        }
    }

    public void canSkill()
    {
        PlayerManager.canHeal = true;
        PlayerManager.SkillPointLevel += 1;
        PlayerManager.SkillPoint = PlayerManager.MaxSkillPoint[PlayerManager.SkillPointLevel];
        Event += 1;

        HealTutorial.SetActive(true);
    }

    public void GiveMap()
    {
        PlayerManager.canMap = true;
        Event += 1;

        MapTutorial.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("KaraUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //離開NPC停止對話
        if (collider.tag == "Player")
        {
            GameObject.Find("KaraUpButton").GetComponent<Animator>().Play("ButtonDisappears");
            GameObject.Find("Flowchart").GetComponent<Flowchart>().StopAllBlocks();
        }
    }
}
