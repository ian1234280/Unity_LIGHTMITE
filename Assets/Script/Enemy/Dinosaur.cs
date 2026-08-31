using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class Dinosaur : MonoBehaviour
{
    int HP;
    int ATK;

    bool moveRight;

    float AttackRange = 10;
    bool canAttack;
    float AttackTime = 1.5f;
    float startAttackTime;

    Vector2 jumpForce = new Vector2(8, 15);

    public GameObject DestructionExplosion;

    public AudioClip 恐龍跳躍;

    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
        ATK = 25;
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
        StateDetection();
        Dead();
    }

    void Animation()
    {
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            GetComponent<Animator>().Play("Fall");
        }
        else if (GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            GetComponent<Animator>().Play("Jump");
        }
        else
        {
            GetComponent<Animator>().Play("landing");

            //攻擊的狀態下才做翻面
            if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= AttackRange)
            {
                Flip();
            }
        }
    }

    void StateDetection()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= AttackRange)
        {
            Attack();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {

    }

    void Attack()
    {
        if (!canAttack)
        {
            canAttack = true;
            startAttackTime = AttackTime;
        }
        if (startAttackTime > 0)
        {
            startAttackTime -= Time.deltaTime;
        }
        else
        {
            if (moveRight)
            {
                GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-jumpForce.x, jumpForce.y), ForceMode2D.Impulse);
            }
            canAttack = false;

            GetComponent<AudioSource>().PlayOneShot(恐龍跳躍);
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
