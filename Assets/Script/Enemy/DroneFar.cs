using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneFar : MonoBehaviour
{
    int HP;
    int ATK;

    bool moveRight;

    float MoveSpeed = 2.5f;
    float damageRecoilSpeed = 2.5f;
    float AirResistanceSpeed = 0.03f;

    bool damage;
    float damageRecoilTime = 0.1f;
    float startDamageRecoilTime;

    bool InstantiateBullet;
    float InstantiateBulletTime = 2f;
    float startInstantiateBulletTime;

    bool StopMove;
    float IdleTime = 2f;
    float startIdleTime;

    public Vector2 LeftDownRange;
    public Vector2 RightTopRange;

    float TrackRange = 12f;
    float AttackRange = 9f;

    public GameObject Bullet;

    public GameObject DestructionExplosion;

    public AudioClip 無人機發射子彈;

    // Start is called before the first frame update
    void Start()
    {
        HP = 50;
        ATK = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AirResistance();
        StateDetection();
        Flip();
        damageRecoil();
        Dead();
    }

    void StateDetection()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= AttackRange)
        {
            Attack();
        }
        else if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= TrackRange)
        {
            Track();
        }
        else
        {
            Patrol();

        }
    }

    void Patrol()
    {
        if (StopMove)
        {
            if (startIdleTime > 0)
            {
                startIdleTime -= Time.deltaTime;
            }
            else
            {
                StopMove = false;
            }
        }
        else
        {
            if (moveRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x >= RightTopRange.x)
                {
                    Idle();
                    moveRight = false;
                }
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x <= LeftDownRange.x)
                {
                    Idle();
                    moveRight = true;
                }
            }
            //回到LeftDownRange.y RightTopRange.y兩數中間的高度
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, (LeftDownRange.y + RightTopRange.y) / 2), MoveSpeed * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= TrackRange)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Track()
    {
        transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, MoveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) >= AttackRange - 1f)
        {
            
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -MoveSpeed / 2 * Time.deltaTime);
        }
        //生成子彈
        if (!InstantiateBullet)
        {
            InstantiateBullet = true;
            startInstantiateBulletTime = InstantiateBulletTime;
        }
        if (startInstantiateBulletTime > 0)
        {
            startInstantiateBulletTime -= Time.deltaTime;
        }
        else
        {
            Instantiate(Bullet, transform.position, transform.rotation);
            GetComponent<Animator>().Play("Thorn", 1, 0f);
            InstantiateBullet = false;

            GetComponent<AudioSource>().PlayOneShot(無人機發射子彈);
        }
    }

    void Idle()
    {
        //停止動作數秒
        if (!StopMove)
        {
            StopMove = true;
            startIdleTime = IdleTime;
        }
        else
        {
            startIdleTime = IdleTime;
        }
    }

    void AirResistance()
    {
        //模擬空氣阻力減速效果
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon || Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > Mathf.Epsilon)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x - AirResistanceSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + AirResistanceSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y - AirResistanceSpeed);
            }
            if (GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y + AirResistanceSpeed);
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Flip()
    {
        bool HasXSpeed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;

        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= TrackRange)
        {
            if (transform.position.x < GameObject.Find("Player").transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (transform.position.x > GameObject.Find("Player").transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (HasXSpeed && !damage)
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
    }

    void damageRecoil()
    {
        if (damage)
        {
            if (startDamageRecoilTime > 0)
            {
                startDamageRecoilTime -= Time.deltaTime;
                if (transform.rotation.y == 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(damageRecoilSpeed, damageRecoilSpeed / 4);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-damageRecoilSpeed, damageRecoilSpeed / 4);
                }
            }
            else
            {
                damage = false;
            }
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

            GetComponent<Animator>().Play("Damage", 2, 0f);

            damage = true;
            startDamageRecoilTime = damageRecoilTime;
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            HP -= PlayerManager.SkillATK;

            GetComponent<Animator>().Play("Damage", 2, 0f);

            damage = true;
            startDamageRecoilTime = damageRecoilTime;
        }
    }
}
