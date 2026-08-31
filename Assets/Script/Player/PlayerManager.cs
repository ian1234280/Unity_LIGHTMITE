using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //玩家狀態
    static public int[] MaxHP = { 125, 150 };
    static public int[] ATK = { 20, 25 };//測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    static public int[] MaxSkillPoint = { 0, 1, 2 };
    static public int SkillATK = 100;

    static public int HPLevel = 0;
    static public int AtkLevel = 0;
    static public int SkillPointLevel = 0;//測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    static public int HP;
    static public int SkillPoint;

    static public bool Dead;
    bool RespawnBool;

    static public int RespawnScene = 7;
    static public Vector3 RespawnPoint = new Vector3(5.4f, 38.8f, 0);

    //作弊
    static public bool GodMode;

    //技能解鎖
    //測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    static public bool canMap;
    static public bool canHeal;
    static public bool canSkill;

    //遊戲進度
    //BOSS是否被擊敗
    static public bool RhinoDead;
    static public bool OctopusDead;
    static public bool DestroyerSeriesDead;
    static public bool LenoBossDead;

    //精密元件
    static public bool Component;

    //玩家音效
    public AudioClip 死亡;

    // Start is called before the first frame update
    void Start()
    {
        //測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //HP = 100;
        HP = MaxHP[HPLevel];

        //測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SkillPoint = 0;

        //測試用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HPManager();
        PlayerDead();
    }

    void HPManager()
    {
        if (GodMode)
        {
            HP = MaxHP[HPLevel];
            SkillPoint = 2;
        }
        else if (HP > MaxHP[HPLevel])
        {
            HP = MaxHP[HPLevel];
        }
        else if (SkillPoint > SkillPointLevel)
        {
            SkillPoint = SkillPointLevel;
        }
    }

    void PlayerDead()
    {
        if (HP <= 0 && !RespawnBool)
        {
            Dead = true;
            RespawnBool = true;
            GameObject.Find("Transition").GetComponent<Animator>().Play("Dead", 0, 0f);
            Invoke("Respawn", 4f);

            GetComponent<AudioSource>().PlayOneShot(死亡);
        }
    }

    void Respawn()
    {
        //回復血量
        HP = MaxHP[HPLevel];

        //回復技能點
        SkillPoint = MaxSkillPoint[SkillPointLevel];

        //移動到重生點
        SceneManager.LoadScene(RespawnScene);
        GameObject.Find("Player").transform.position = RespawnPoint;

        //播放場景過度動畫
        GameObject.Find("Transition").GetComponent<Animator>().Play("Transition_End", 0, 0f);

        //回復Bool
        Dead = false;
        RespawnBool = false;
    }
}
