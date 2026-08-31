using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : MonoBehaviour
{
    int HP;
    int ATK;

    bool moveRight;

    float AttackRange = 12;

    float Speed;
    float maxSpeed = 12f;

    public GameObject DestructionExplosion;

    public AudioClip 山羊衝刺;

    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
        ATK = 25;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StateDetection();
        Dead();
        Flip();
    }

    void StateDetection()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= AttackRange)
        {
            Attack();
            GetComponent<Animator>().Play("Attack");

            //動態控制奔跑動畫的速度
            GetComponent<Animator>().speed = (Mathf.Abs(Speed) / 12) + 0.25f;
        }
        else
        {
            Patrol();
            GetComponent<Animator>().Play("Idle");
        }
    }

    void Patrol()
    {

    }

    void Attack()
    {
        if (moveRight)
        {
            if (Speed >= maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Speed += 0.15f, GetComponent<Rigidbody2D>().velocity.y);
            }
        }
        else
        {
            if (Speed <= -maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Speed -= 0.15f, GetComponent<Rigidbody2D>().velocity.y);
            }
        }

        if (GetComponent<Rigidbody2D>().velocity.x == 0)
        {
            //GetComponent<AudioSource>().PlayOneShot(山羊衝刺);
        }
    }

    void Flip()
    {
        if (transform.position.x < GameObject.Find("Player").transform.position.x)
        {
            moveRight = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (transform.position.x > GameObject.Find("Player").transform.position.x)
        {
            moveRight = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            //回復玩家狀態
            PlayerManager.HP += 20;
            PlayerManager.SkillPoint += 1;

            Instantiate(DestructionExplosion, transform.position, transform.rotation);
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
            HP -= PlayerManager.ATK[PlayerManager.AtkLevel];

            GetComponent<Animator>().Play("Damage", 1, 0f);
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            HP -= PlayerManager.SkillATK;

            GetComponent<Animator>().Play("Damage", 1, 0f);
        }
    }
}
