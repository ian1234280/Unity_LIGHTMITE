using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public Image HP;


    public Image skillBar1;
    public Image skillBar2;
    public Image skillPoint1;
    public Image skillPoint2;

    //地圖
    public GameObject MapUI;

    //暫停介面
    public GameObject PauseUI;
    public GameObject SettingsUI;

    //作弊
    public GameObject CheatUI;

    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HPManager();
        SkillPointManager();
        Map();
        MapUnlock();
        Pause();

        //作弊選單
        Cheat();
    }

    void HPManager()
    {
        HP.fillAmount = (float)PlayerManager.HP / (float)PlayerManager.MaxHP[PlayerManager.HPLevel];
    }

    void SkillPointManager()
    {
        skillBar1.enabled = (PlayerManager.MaxSkillPoint[PlayerManager.SkillPointLevel] >= 1);
        skillBar2.enabled = (PlayerManager.MaxSkillPoint[PlayerManager.SkillPointLevel] >= 2);
        skillPoint1.enabled = (PlayerManager.SkillPoint >= 1);
        skillPoint2.enabled = (PlayerManager.SkillPoint >= 2);
    }

    void Map()
    {
        if (PlayerManager.canMap)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(OpenCloseMap());
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                StartCoroutine(OpenCloseMap());
            }
        }
    }

    IEnumerator OpenCloseMap()
    {
        GameObject.Find("Transition").GetComponent<Animator>().Play("Map", 0, 0f);
        yield return new WaitForSeconds(0.166f);
        MapUI.SetActive(!MapUI.activeSelf);

        //顯示玩家位置
        if (MapUI.activeSelf)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-539, -227, 0);
                    break;
                case 2:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-373, -291, 0);
                    break;
                case 3:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-123, -291, 0);
                    break;
                case 4:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-354, -164, 0);
                    break;
                case 5:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(94, -255, 0);
                    break;
                case 6:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(275, -135, 0);
                    break;
                case 7:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-477, -77, 0);
                    break;
                case 8:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-326, -50, 0);
                    break;
                case 9:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-530, 32, 0);
                    break;
                case 10:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-32, -48, 0);
                    break;
                case 11:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(224, 76, 0);
                    break;
                case 12:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(-25, 90, 0);
                    break;
                case 13:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(470, -103, 0);
                    break;
                case 14:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(600, -23, 0);
                    break;
                case 15:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(442, 164, 0);
                    break;
                case 16:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(268, 244, 0);
                    break;
                case 17:
                    GameObject.Find("MapPlayer").GetComponent<RectTransform>().anchoredPosition = new Vector3(20, 273, 0);
                    break;
            }
        }
    }

    void MapUnlock()
    {
        if (PlayerManager.canMap)
        {
            Destroy(GameObject.Find("MapMask" + SceneManager.GetActiveScene().buildIndex));
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.Find("Flowchart") != null)
            {
                if (!FindObjectOfType<Flowchart>().HasExecutingBlocks())
                {
                    isPaused = !isPaused;

                    if (isPaused)
                    {
                        Time.timeScale = 0;
                        PauseUI.SetActive(true);
                    }
                    else
                    {
                        Time.timeScale = 1;
                        PauseUI.SetActive(false);
                    }
                }
            }
            else
            {
                isPaused = !isPaused;

                if (isPaused)
                {
                    Time.timeScale = 0;
                    PauseUI.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    PauseUI.SetActive(false);
                }
            }
        }
    }

    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CheatUI.SetActive(true);
        }
    }

    public void SettingsButton()
    {
        SettingsUI.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Virtual Camera"));
        Destroy(GameObject.Find("Canvas"));
        SettingsUI.SetActive(true);
        Destroy(GameObject.Find("SettingsCanvas"));
        CheatUI.SetActive(true);
        Destroy(GameObject.Find("CheatCanvas"));
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene(0);
    }
}
