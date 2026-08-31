using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    int ATK;

    // Start is called before the first frame update
    void Start()
    {
        ATK = Octopus.ATK;

        StartCoroutine(Destroy(3));
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    void Dead()
    {
        if (PlayerManager.OctopusDead)
        {
            Destroy(gameObject);
        }
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
            Octopus.HP -= PlayerManager.ATK[PlayerManager.AtkLevel];

            GetComponent<Animator>().Play("Damage", 1, 0f);
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            Octopus.HP -= PlayerManager.SkillATK;

            GetComponent<Animator>().Play("Damage", 1, 0f);
        }
    }

    IEnumerator Destroy(float T)
    {
        yield return new WaitForSeconds(T);
        Destroy(transform.parent.gameObject);
    }
}
