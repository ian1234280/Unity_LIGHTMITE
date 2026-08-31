using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KellyKarlo : MonoBehaviour
{
    public GameObject BossArea;

    //事件狀態
    static public int Event = 0;
    bool ConversationState;

    //音效
    public AudioClip 關電視聲;

    // Start is called before the first frame update
    void Start()
    {
        //擊敗所有BOSS和已交付零件
        //if (PlayerManager.RhinoDead && PlayerManager.OctopusDead && PlayerManager.DestroyerSeriesDead && Event == 2)
        if (PlayerManager.RhinoDead && PlayerManager.OctopusDead && PlayerManager.DestroyerSeriesDead && Event == 2)
        {
            gameObject.SetActive(false);
            BossArea.gameObject.SetActive(true);
        }
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
            switch (Event)
            {
                case 0:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("初見雙子");
                    break;
                case 1:
                    if (PlayerManager.Component)
                    {
                        GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("交付精密元件");
                    }
                    else if(!ConversationState)
                    {
                        GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("重複對話");
                        ConversationState = true;
                    }
                    else
                    {
                        GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("跟凱洛的對話");
                        ConversationState = false;
                    }
                    break;
                case 2:
                    GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("交付精密元件重複對話");
                    break;
            }
        }
        else if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //GameObject.Find("Kelly&KarloUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    public void AddEvent()
    {
        Event += 1;
    }

    public void StartFight()
    {
        LenoBoss.StartFightbool = true;
    }

    public void EndingCG()
    {
        GameObject.Find("Ending2CG").GetComponent<Animator>().Play("Ending2CGAnimation");

        GameObject.Find("雷諾腳色").GetComponent<AudioSource>().Play();
    }

    public void FadeOut()
    {
        GameObject.Find("Transition").GetComponent<Animator>().Play("Dead", 0, 0f);

        GameObject.Find("雷諾腳色").GetComponent<AudioSource>().PlayOneShot(關電視聲);
    }

    public void ED2()
    {
        GameObject.Find("ed2").GetComponent<Animator>().Play("ed2");
    }

    public void BackToMenu()
    {
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Virtual Camera"));
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("Kelly&KarloUpButton").GetComponent<Animator>().Play("ButtonAppears");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //離開NPC停止對話
        if (collider.tag == "Player")
        {
            GameObject.Find("Kelly&KarloUpButton").GetComponent<Animator>().Play("ButtonDisappears");
            GameObject.Find("Flowchart").GetComponent<Flowchart>().StopAllBlocks();
        }
    }
}
