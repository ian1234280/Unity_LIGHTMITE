using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerSeries : MonoBehaviour
{
    int HP;
    int ATK;

    int AttackMode;

    bool isDownAttack;
    float DownAttackSpeed = 35f;

    bool isEnergyCannon;
    bool isEnergyCannonRain;

    public GameObject energyCannon;

    public GameObject DestructionExplosion;

    public GameObject DestroyerSeriesDead;

    public AudioClip 瞬移;
    public AudioClip 能源砲;

    // Start is called before the first frame update
    void Start()
    {
        //Test!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HP = 1000;
        ATK = 25;

        //開始動畫1.292秒
        StartCoroutine(Disappear(1.292f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Animator>().Play("null", 1, 0f);
        }

        Debug.Log(HP);

        switch (AttackMode)
        {
            case 1:
                DownAttack();
                break;
            case 2:
                EnergyCannon();
                break;
            case 3:
                EnergyCannonRain();
                break;
            default:
                
                break;
        }

        Dead();
    }

    void DownAttack()
    {
        if (isDownAttack)
        {
            transform.Translate(new Vector3(0, -DownAttackSpeed, 0) * Time.deltaTime);
            GetComponent<Animator>().Play("DownAttack", 0, 0f);

            if (transform.position.y - DownAttackSpeed * Time.deltaTime <= -2.6f)
            {
                isDownAttack = false;
                StartCoroutine(Disappear(0));
            }
        }
    }

    void EnergyCannon()
    {
        if (isEnergyCannon)
        {
            GetComponent<Animator>().Play("UsingEnergyCannon", 0, 0f);

            if (transform.rotation.y == 0)
            {
                Instantiate(energyCannon, transform.position + new Vector3(2, 0, 0), transform.rotation);
            }
            else
            {
                Instantiate(energyCannon, transform.position + new Vector3(-2, 0, 0), transform.rotation);
            }
            //攝影機震動
            CameraShake.Instance.EnergyCannon.GenerateImpulse();

            //發射完後暫停X秒
            isEnergyCannon = false;
            StartCoroutine(Disappear(1f));

            GetComponent<AudioSource>().PlayOneShot(能源砲);
        }
    }

    void EnergyCannonRain()
    {
        if (isEnergyCannonRain)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            GetComponent<Animator>().Play("EnergyExplosion", 0, 0f);

            StartCoroutine(FireEnergyCannonRain(30, 0.25f));

            isEnergyCannonRain = false;
        }
    }

    IEnumerator FireEnergyCannonRain(int shots, float time)
    {
        for (int i = 0; i < shots; i++)
        {
            Instantiate(energyCannon, new Vector3(UnityEngine.Random.Range(-26, -3), 12, 0), Quaternion.Euler(0, 0, -90));

            // 攝影機震動
            CameraShake.Instance.EnergyCannon.GenerateImpulse();

            GetComponent<AudioSource>().PlayOneShot(能源砲);

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(time);
        }

        //場景換回藍色
        GameObject.Find("Background").GetComponent<Animator>().Play("控制室藍");

        //發射完後暫停X秒
        StartCoroutine(Disappear(1f));
    }

    void AttackModeChange()
    {
        int RandomAttackMode = UnityEngine.Random.Range(1, 101);

        if (HP <= 500)
        {
            switch (RandomAttackMode)
            {
                case <= 45:
                    AttackMode = 1;
                    StartCoroutine(Appear(0.292f));
                    break;
                case <= 85:
                    AttackMode = 2;
                    StartCoroutine(Appear(0.5f));
                    break;
                case > 85:
                    AttackMode = 3;
                    StartCoroutine(Appear(0.5f));
                    break;
            }
        }
        else
        {
            switch (RandomAttackMode)
            {
                case <= 60:
                    AttackMode = 1;
                    StartCoroutine(Appear(0.292f));
                    break;
                case > 60:
                    AttackMode = 2;
                    StartCoroutine(Appear(0.5f));
                    break;
            }
        }
    }

    //Disappear(攻擊結束後S秒後消失)
    IEnumerator Disappear(float S)
    {
        yield return new WaitForSeconds(S);
        GetComponent<Animator>().Play("Disappear", 0, 0f);

        GetComponent<AudioSource>().PlayOneShot(瞬移);
        //消失動畫0.292秒
        yield return new WaitForSeconds(0.292f);
        transform.position = new Vector3(transform.position.x, 15f, transform.position.z);
        //招式間隔 //invok
        yield return new WaitForSeconds(0.5f);
        AttackModeChange();
    }

    //Appear(出現之後S秒後攻擊)
    IEnumerator Appear(float S)
    {
        //各個攻擊模式的出現位置
        switch (AttackMode)
        {
            case 1:
                //追蹤玩家x座標
                transform.position = new Vector3(GameObject.Find("Player").transform.position.x, 5.7f, transform.position.z);
                break;
            case 2:
                //以Boss戰場景中間(x-15)為中心 玩家位置偏左偏右決定生成位置
                if (GameObject.Find("Player").transform.position.x > -15)
                {
                    transform.position = new Vector3(-5, -2.61f, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.position = new Vector3(-25, -2.61f, transform.position.z);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
            case 3:
                //到場景中央
                transform.position = new Vector3(-15f, 4f, transform.position.z);

                //場景變成紅色警告
                GameObject.Find("Background").GetComponent<Animator>().Play("控制室紅");

                break;
        }
        GetComponent<Animator>().Play("Appear", 0, 0f);

        GetComponent<AudioSource>().PlayOneShot(瞬移);
        yield return new WaitForSeconds(S);
        //開始攻擊
        switch (AttackMode)
        {
            case 1:
                isDownAttack = true;
                //面向玩家
                if (transform.position.x < GameObject.Find("Player").transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (transform.position.x > GameObject.Find("Player").transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                break;
            case 2:
                isEnergyCannon = true;
                break;
            case 3:
                isEnergyCannonRain = true;
                break;
        }
    }

    void Dead()
    {
        if (HP <= 0)
        {
            PlayerManager.DestroyerSeriesDead = true;

            //場景換回藍色
            GameObject.Find("Background").GetComponent<Animator>().Play("控制室藍");

            Instantiate(DestructionExplosion, transform.position, transform.rotation);
            Instantiate(DestroyerSeriesDead, transform.position, transform.rotation);

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
