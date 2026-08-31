using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVacuum : MonoBehaviour
{
    int HP;
    int ATK;

    bool moveRight;
    float MoveSpeed = 1.5f;

    bool damage;
    float IdleTime = 0.75f;
    float startIdleTime;

    public float LeftRange;
    public float RightRange;

    public GameObject DestructionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        HP = 50;
        ATK = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(HP);
        Patrol();
        Flip();
        Dead();
    }

    void Patrol()
    {
        if (damage)
        {
            if (startIdleTime > 0)
            {
                startIdleTime -= Time.deltaTime;
            }
            else
            {
                damage = false;
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
                }
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x <= LeftRange)
                {
                    Idle();
                    moveRight = true;
                }
            }
        }
    }

    void Idle()
    {
        if (!damage)
        {
            damage = true;
            startIdleTime = IdleTime;
        }
        else
        {
            startIdleTime = IdleTime;
        }
    }

    void Flip()
    {
        bool HasXSpeed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        if (HasXSpeed)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (GetComponent<Rigidbody2D>().velocity.x < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            //回復玩家狀態
            PlayerManager.HP += 15;
            if (Random.value < 0.5f)
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

            GetComponent<Animator>().Play("Damage", 1, 0f);

            Idle();
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            HP -= PlayerManager.SkillATK;

            GetComponent<Animator>().Play("Damage", 1, 0f);

            Idle();
        }
    }
}