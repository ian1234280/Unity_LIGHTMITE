using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Rhino : MonoBehaviour
{
    int HP;
    int ATK;

    int AttackMode;

    bool isRun;
    float RunSpeed = 10;

    bool isCrash;
    float CrashSpeed = 15;

    bool isLazer;
    public GameObject Eyes;

    public GameObject DestructionExplosion;

    public GameObject LazerRed;

    public BoxCollider2D HitBox;

    public AudioClip 犀牛踢腳;
    public AudioClip 犀牛衝刺;
    public AudioClip 閃電;

    // Start is called before the first frame update
    void Start()
    {
        //Test!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HP = 600;
        ATK = 25;

        StartCoroutine(AttackModeChange(1f));
    }

    // Update is called once per frame
    void Update()
    {
        switch (AttackMode)
        {
            case 1:
                Run();
                break;
            case 2:
                Crash();
                break;
            case 3:
                Lazer();
                break;
            default:

                break;
        }

        Dead();
    }

    void Run()
    {
        if (isRun)
        {
            transform.Translate(new Vector2(-RunSpeed * Time.deltaTime, 0));

            if (transform.position.x < 8.2f && transform.rotation.y == 0)
            {
                isRun = false;
                AttackMode = 0;
                GetComponent<Animator>().Play("Idle", 0, 0f);

                GetComponent<AudioSource>().Stop();

                StartCoroutine(AttackModeChange(1f));
            }
            if (transform.position.x > 32.8f && transform.rotation.y != 0)
            {
                isRun = false;
                AttackMode = 0;
                GetComponent<Animator>().Play("Idle", 0, 0f);

                GetComponent<AudioSource>().Stop();

                StartCoroutine(AttackModeChange(1f));
            }

            //攝影機震動
            //CameraShake.Instance.EnergyCannon.GenerateImpulse();
        }
    }

    void Crash()
    {
        if (isCrash && !PlayerManager.RhinoDead)
        {
            transform.Translate(new Vector2(-CrashSpeed * Time.deltaTime, 0));

            //超出邊界往反方向跑
            if (transform.position.x < 8.2f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (transform.position.x > 32.8f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    IEnumerator StartCrash()
    {
        // 等待X秒
        yield return new WaitForSeconds(10);

        isCrash = false;
        AttackMode = 0;
        GetComponent<Animator>().Play("Idle", 0, 0f);

        GetComponent<AudioSource>().Stop();

        StartCoroutine(AttackModeChange(1f));
    }

    void Lazer()
    {
        if (isLazer)
        {
            StartCoroutine(FireLazer(2, 1.5f));

            isLazer = false;
        }
    }

    IEnumerator FireLazer(int shots, float time)
    {
        for (int i = 0; i < shots; i++)
        {
            //計算發射角度
            Vector2 direction = GameObject.Find("Player").transform.position - Eyes.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //延遲發射
            yield return new WaitForSeconds(0.5f);

            if (!PlayerManager.RhinoDead)
            {
                //生成雷射
                Instantiate(LazerRed, Eyes.transform.position, Quaternion.Euler(0, 0, angle + 90));

                // 攝影機震動
                CameraShake.Instance.EnergyCannon.GenerateImpulse();

                GetComponent<AudioSource>().PlayOneShot(閃電);

                // 等待time秒再發射下一個
                yield return new WaitForSeconds(time);
            }
        }

        //發射完後暫停X秒
        StartCoroutine(AttackModeChange(1));
    }

    IEnumerator AttackModeChange(float time)
    {

        //機率選擇招式
        int RandomAttackMode = UnityEngine.Random.Range(1, 101);

        if (HP <= 350)
        {
            switch (RandomAttackMode)
            {
                case <= 30:
                    AttackMode = 3;
                    break;
                case > 30:
                    AttackMode = 2;
                    break;
            }
        }
        else
        {
            switch (RandomAttackMode)
            {
                case <= 100:
                    AttackMode = 1;
                    break;
            }
        }

        //面向玩家
        if (GameObject.Find("Player").transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //招式間隔
        yield return new WaitForSeconds(time);

        switch (AttackMode)
        {
            case 1:
                GetComponent<AudioSource>().PlayOneShot(犀牛踢腳);
                GetComponent<Animator>().Play("Ready", 0, 0f);
                yield return new WaitForSeconds(1.74f);
                isRun = true;

                GetComponent<AudioSource>().Play();
                break;
            case 2:
                GetComponent<AudioSource>().PlayOneShot(犀牛踢腳);
                GetComponent<Animator>().Play("Ready", 0, 0f);
                yield return new WaitForSeconds(1.74f);
                isCrash = true;
                StartCoroutine(StartCrash());

                GetComponent<AudioSource>().Play();
                break;
            case 3:
                isLazer = true;

                break;
        }
    }

    void Dead()
    {
        if (HP <= 0 && !PlayerManager.RhinoDead)
        {
            PlayerManager.RhinoDead = true;
            AttackMode = 0;
            StopCoroutine("AttackModeChange");

            HitBox.isTrigger = true;

            StartCoroutine(FireDestructionExplosion(20, 0.2f));
        }

        if (PlayerManager.RhinoDead)
        {
            GetComponent<Animator>().Play("Idle", 0, 0f);
            GetComponent<AudioSource>().Stop();
        }
    }

    IEnumerator FireDestructionExplosion(int shots, float time)
    {
        for (int i = 0; i < shots; i++)
        {
            Instantiate(DestructionExplosion, GetRandomPositionWithinBoss(), transform.rotation);

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(time);
        }

        Destroy(gameObject);
    }

    private Vector3 GetRandomPositionWithinBoss()
    {
        // 假設 BOSS 使用的是 BoxCollider2D 作為碰撞體
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        if (collider != null)
        {
            // 計算碰撞體的範圍
            float randomX = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
            float randomY = Random.Range(collider.bounds.min.y, collider.bounds.max.y);

            return new Vector3(randomX, randomY, transform.position.z);
        }

        // 如果沒有 BoxCollider2D，則返回 BOSS 中心
        return transform.position;
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
