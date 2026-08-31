using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    int HP;
    public bool canRespawn;

    public GameObject explosion;

    public GameObject DestructionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        HP = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    void Dead()
    {
        if (HP <= 0)
        {
            Instantiate(DestructionExplosion, transform.position, transform.rotation);

            if (canRespawn)
            {
                //重設HP
                HP = 20;

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                StartCoroutine(Respawn());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            //觸碰到玩家就爆炸
            StartCoroutine(Explosion());
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
            HP -= PlayerManager.SkillATK;
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(explosion, transform.position, explosion.transform.rotation);

        if (canRespawn)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(Respawn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        //重新顯示地雷
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
