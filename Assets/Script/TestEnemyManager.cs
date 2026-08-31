using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyManager : MonoBehaviour
{
    static public int HP;
    static public int ATK;

    // Start is called before the first frame update
    void Start()
    {
        HP = 50;
        ATK = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HP);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.HP -= ATK;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //被玩家攻擊到
        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP -= PlayerManager.ATK[PlayerManager.AtkLevel];
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            HP -= 60;
        }
    }
}
