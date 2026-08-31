using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialization : MonoBehaviour
{
    //開頭動畫音效
    public AudioClip 硝煙聲;
    public AudioClip 鐵片;
    public AudioClip 拉桿;
    public AudioClip 程式運轉聲;
    public AudioClip 鍵盤打字聲;

    //遊戲物件
    public GameObject MainCamera;
    public GameObject VirtualCamera;
    public GameObject Player;
    public GameObject Canvas;
    public GameObject EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialization()
    {
        //初始化遊戲數值
        PlayerManager.HPLevel = 0;
        PlayerManager.AtkLevel = 0;
        PlayerManager.SkillPointLevel = 0;
        PlayerManager.RespawnScene = 7;
        PlayerManager.RespawnPoint = new Vector3(5.4f, 38.8f, 0);
        PlayerManager.canMap = false;
        PlayerManager.canHeal = false;
        PlayerManager.canSkill = false;
        PlayerManager.RhinoDead = false;
        PlayerManager.OctopusDead = false;
        PlayerManager.DestroyerSeriesDead = false;
        PlayerManager.LenoBossDead = false;
        PlayerManager.Component = false;
        Kara.Event = 0;
        Todd.isRepeat = false;
        Leno.Event = 0;
        KellyKarlo.Event = 0;

        MainCamera.SetActive(true);
        VirtualCamera.SetActive(true);
        Player.SetActive(true);
        Canvas.SetActive(true);
        EventSystem.SetActive(true);
        SceneManager.LoadScene(7);
    }

    public void BackToMenu()
    {
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Virtual Camera"));
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("SettingsCanvas"));
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene(0);
    }

    public void OpeningSE1()
    {
        GetComponent<AudioSource>().PlayOneShot(硝煙聲);
    }

    public void OpeningSE2()
    {
        GetComponent<AudioSource>().PlayOneShot(鐵片);
    }

    public void Ending1SE1()
    {
        GetComponent<AudioSource>().PlayOneShot(拉桿);
    }

    public void Ending1SE2()
    {
        GetComponent<AudioSource>().PlayOneShot(程式運轉聲);
    }

    public void Ending1SE3()
    {
        GetComponent<AudioSource>().PlayOneShot(鍵盤打字聲);
    }
}
