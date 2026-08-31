using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Todd : MonoBehaviour
{
    static public bool isRepeat;
    public GameObject SkillTutorial_1;
    public GameObject SkillTutorial_2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TalkTrigger();
    }

    void TalkTrigger()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isRepeat)
            {
                GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("初見陶德");
            }
            else
            {
                GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("重複對話");
            }
        }
        else if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //GameObject.Find("ToddUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    public void Repeat()
    {
        //閃白動畫
        GameObject.Find("Transition").GetComponent<Animator>().Play("Charging", 0, 0f);

        PlayerManager.canSkill = true;
        PlayerManager.HPLevel += 1;
        PlayerManager.HP = PlayerManager.MaxHP[PlayerManager.HPLevel];
        PlayerManager.AtkLevel += 1;
        PlayerManager.SkillPointLevel += 1;
        PlayerManager.SkillPoint = PlayerManager.MaxSkillPoint[PlayerManager.SkillPointLevel];
        isRepeat = true;

        SkillTutorial_1.SetActive(true);
        SkillTutorial_2.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("ToddUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //離開NPC停止對話
        if (collider.tag == "Player")
        {
            GameObject.Find("ToddUpButton").GetComponent<Animator>().Play("ButtonDisappears");
            GameObject.Find("Flowchart").GetComponent<Flowchart>().StopAllBlocks();
        }
    }
}
