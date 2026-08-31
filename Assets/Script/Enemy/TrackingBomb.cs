using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBomb : MonoBehaviour
{
    int HP;

    float MoveSpeed = 0f;
    float RotationSpeed = 3f;

    float AttackRange = 10f;
    bool isTracking;

    public GameObject Trail;
    public GameObject explosion;
    public GameObject DestructionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        HP = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StateDetection();
        Track();
        Dead();
    }

    void StateDetection()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= AttackRange)
        {
            isTracking = true;
        }
    }

    void Track()
    {
        if (isTracking)
        {
            //顯示火焰
            Trail.SetActive(true);

            //移動
            if (MoveSpeed < 15f)
            {
                transform.Translate(new Vector2(0, MoveSpeed * Time.deltaTime));
                //慢慢加速
                MoveSpeed += 0.15f;

                //箭頭永遠朝向玩家
                //獲取指向玩家的方向
                Vector3 direction = GameObject.Find("Player").transform.position - transform.position;
                //計算目標角度
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //目標旋轉
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
                //平滑旋轉
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector2(0, MoveSpeed * Time.deltaTime));
            }
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            Instantiate(DestructionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
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
        //觸碰到地板
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //觸碰到就爆炸
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
        Destroy(gameObject);
    }
}
