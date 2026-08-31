using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWheel : MonoBehaviour
{
    int HP;
    int ATK;

    public bool moveRight;
    float MoveSpeed = 7.5f;

    bool startIdle;
    float IdleTime = 0.5f;
    float startIdleTime;

    public float LeftRange;
    public float RightRange;

    public bool Undead;

    public GameObject DestructionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        HP = 50;
        ATK = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        if (!Undead)
        {
            Dead();
        }
    }

    void Patrol()
    {
        if (startIdle)
        {
            if (startIdleTime > 0)
            {
                startIdleTime -= Time.deltaTime;
            }
            else
            {
                startIdle = false;
                //回復可旋轉
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
        }
        else
        {
            if (moveRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x >= RightRange)
                {
                    Idle();
                    moveRight = false;
                    //停止動作
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //凍結旋轉
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x <= LeftRange)
                {
                    Idle();
                    moveRight = true;
                    //停止動作
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //凍結旋轉
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }

    void Idle()
    {
        if (!startIdle)
        {
            startIdle = true;
            startIdleTime = IdleTime;
        }
        else
        {
            startIdleTime = IdleTime;
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            //回復玩家狀態
            PlayerManager.HP += 15;
            if (UnityEngine.Random.value < 0.5f)
            {
                PlayerManager.SkillPoint += 1;
            }

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

            GetComponent<Animator>().Play("Damage", 0, 0f);
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            HP -= PlayerManager.SkillATK;

            GetComponent<Animator>().Play("Damage", 0, 0f);
        }
    }
}
