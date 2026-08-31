using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class LenoBoss : MonoBehaviour
{
    int HP;
    int ATK;

    int AttackMode;

    bool moveRight;

    bool isRush;
    //Rush的高度
    float RushAmplitude = 2.5f;
    //Rush的速度
    float RushSpeed = 5f;
    //Rush的時間
    float rushTime;

    bool isTracingAttack;
    bool hasTracingPlayer;
    Vector3 PlayerPosition;

    bool isMagicSphere;
    public GameObject magicSphere;

    bool isLazer;
    public GameObject Lazerblue;

    bool isfloat;
    //飄浮的高度
    float floatAmplitude = 0.25f;
    //飄動的速度
    float floatSpeed = 2f;
    //初始位置
    Vector3 startPosition;

    //打斷對話直接開始戰鬥
    bool XStartFightbool;
    static public bool StartFightbool;

    public GameObject DestructionExplosion;

    public AudioClip 雷諾狙擊;
    public AudioClip 閃電;

    // Start is called before the first frame update
    void Start()
    {
        //Test!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HP = 1000;
        ATK = 25;

        //記錄初始位置
        startPosition = transform.position;

        GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("雷諾BOSS開場");
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        StartFight();
        Dead();

        switch (AttackMode)
        {
            case 1:
                Rush();
                break;
            case 2:
                TracingAttack();
                break;
            case 3:
                MagicSphere();
                break;
            case 4:
                Lazer();
                break;
            default:

                break;
        }

        //受傷後停止對話
        if (HP < 1000 && !XStartFightbool)
        {
            GameObject.Find("Flowchart").GetComponent<Flowchart>().StopAllBlocks();
            StartFightbool = true;
            XStartFightbool = true;
        }
    }

    void Float()
    {
        if (isfloat)
        {
            // 使用 Mathf.Sin 來計算平滑的上下運動
            float floatY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

            // 設定新位置
            transform.position = new Vector3(transform.position.x, floatY, transform.position.z);
        }
    }

    void StartFight()
    {
        if (StartFightbool)
        {
            StartCoroutine(AttackEnd(1f));
            StartFightbool = false;
        }
    }

    void Rush()
    {
        if (!isRush)
        {
            GetComponent<Animator>().Play("生氣", 0, 0f);

            if (moveRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(29.5f, 1f), 5f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(47.5f, 1f), 5f * Time.deltaTime);
            }

            //歸零時間
            rushTime = 0;
        }
        else if (isRush)
        {
            rushTime += Time.deltaTime;
            // 使用 Mathf.Sin 來計算平滑的上下運動
            float floatY = 1 + Mathf.Sin(rushTime * RushSpeed) * RushAmplitude;

            // 設定新位置
            if (moveRight)
            {
                transform.position = new Vector3(transform.position.x + 10f * Time.deltaTime, floatY, transform.position.z);
                if (transform.position.x >= 47.5f)
                {
                    isRush = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    StartCoroutine(AttackEnd(0));
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 10f * Time.deltaTime, floatY, transform.position.z);
                if (transform.position.x <= 29.5f)
                {
                    isRush = false;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    StartCoroutine(AttackEnd(0));
                }
            }
        }
    }

    void TracingAttack()
    {
        if (isTracingAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerPosition, 20f * Time.deltaTime);
            //GetComponent<Animator>().Play("準備追擊", 0, 0f);

            //追擊到玩家位置後移動到
            if (Vector2.Distance(transform.position, PlayerPosition) <= 0.5f)
            {
                //GetComponent<Animator>().Play("追擊", 0, 0f);
                hasTracingPlayer = true;
                isTracingAttack = false;
                StartCoroutine(AttackEnd(2));
            }
        }
        if (hasTracingPlayer)
        {
            if (transform.position.x <= 38.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(29.5f, 1f), 5f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(47.5f, 1f), 5f * Time.deltaTime);
            }
        }
    }

    void MagicSphere()
    {
        if (isMagicSphere)
        {
            GetComponent<Animator>().Play("往前指", 0, 0f);
            if (transform.rotation.y == 0)
            {
                Instantiate(magicSphere, transform.position - new Vector3(3, 0, 0), magicSphere.transform.rotation);
            }
            else
            {
                Instantiate(magicSphere, transform.position - new Vector3(-3, 0, 0), magicSphere.transform.rotation);
            }
            isMagicSphere = false;
            AttackMode = 0;
            StartCoroutine(AttackEnd(4));
        }
    }

    void Lazer()
    {
        if (isLazer)
        {
            GetComponent<Animator>().Play("往上指", 0, 0f);

            StartCoroutine(FireLazer(8, 0.75f));

            isLazer = false;
        }
    }

    IEnumerator FireLazer(int shots, float time)
    {
        //往上指X秒後開始雷射
        yield return new WaitForSeconds(1);

        for (int i = 0; i < shots; i++)
        {
            Instantiate(Lazerblue, new Vector3(GameObject.Find("Player").transform.position.x, 1.7f, 0), Quaternion.Euler(0, 0, 0));

            // 攝影機震動
            CameraShake.Instance.EnergyCannon.GenerateImpulse();

            GetComponent<AudioSource>().PlayOneShot(閃電);

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(time);
        }

        //發射完後暫停X秒
        StartCoroutine(AttackEnd(1));
    }

    void AttackModeChange()
    {
        int RandomAttackMode = UnityEngine.Random.Range(1, 101);

        if (HP <= 500)
        {
            switch (RandomAttackMode)
            {
                case <= 20:
                    AttackMode = 1;
                    StartCoroutine(AttackStart(1f));
                    break;
                case <= 50:
                    AttackMode = 2;
                    StartCoroutine(AttackStart(1f));
                    break;
                case <= 75:
                    AttackMode = 3;
                    StartCoroutine(AttackStart(1f));
                    break;
                case > 75:
                    AttackMode = 4;
                    StartCoroutine(AttackStart(1f));
                    break;
            }
        }
        else
        {
            switch (RandomAttackMode)
            {
                case <= 35:
                    AttackMode = 1;
                    StartCoroutine(AttackStart(1f));
                    break;
                case <= 70:
                    AttackMode = 2;
                    StartCoroutine(AttackStart(1f));
                    break;
                case > 70:
                    AttackMode = 3;
                    StartCoroutine(AttackStart(1f));
                    break;
            }
        }
    }

    IEnumerator AttackEnd(float S)
    {
        //招式間隔
        yield return new WaitForSeconds(S);
        //取消以追蹤到玩家
        hasTracingPlayer = false;
        //回復動畫
        GetComponent<Animator>().Play("生氣", 0, 0f);

        AttackModeChange();
    }

    IEnumerator AttackStart(float S)
    {
        //各個攻擊模式的出現位置
        switch (AttackMode)
        {
            case 1:
                //判斷在場景的左邊還是右邊
                if (transform.position.x <= 38.5f)
                {
                    moveRight = true;
                }
                else
                {
                    moveRight = false;
                }
                break;
            case 2:

                break;
            case 3:
                
                break;
            case 4:
                //到場景中央
                
                break;
        }

        //等待時間
        yield return new WaitForSeconds(S);

        //開始攻擊
        switch (AttackMode)
        {
            case 1:
                isRush = true;
                if (moveRight)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
            case 2:
                isTracingAttack = true;
                PlayerPosition = GameObject.Find("Player").transform.position;
                //面向玩家
                if (transform.position.x < GameObject.Find("Player").transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (transform.position.x > GameObject.Find("Player").transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                GetComponent<AudioSource>().PlayOneShot(雷諾狙擊);
                break;
            case 3:
                isMagicSphere = true;
                if (transform.position.x <= 38.5f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
            case 4:
                isLazer = true;
                if (transform.position.x <= 38.5f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            GameObject.Find("Flowchart").GetComponent<Flowchart>().ExecuteBlock("結局2");
            PlayerManager.LenoBossDead = true;
            Instantiate(DestructionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
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
