using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    static public int HP;
    static public int ATK;

    int AttackMode;
    int ShotCount;

    public GameObject 突刺觸手;
    int TrackingThrustSpeed;

    public GameObject 橫掃觸手;

    public GameObject DestructionExplosion;

    public AudioClip 噴墨;

    // Start is called before the first frame update
    void Start()
    {
        //Test!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HP = 1600;
        ATK = 20;

        StartCoroutine(AttackModeChange(3));

        //視差功能
        gameObject.GetComponent<Parallax>().boundingCollider = GameObject.Find("Camera Bounds").GetComponent<Collider2D>();

        //設定噴墨
        GameObject.Find("噴墨").transform.SetParent(Camera.main.transform);
        GameObject.Find("噴墨").transform.position = new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y + 1.5f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Difficulty();
        Dead();
    }

    void Difficulty()
    {
        switch(HP)
        {
            case > 1200:
                ShotCount = 4;
                TrackingThrustSpeed = 2;
                break;
            case > 800:
                ShotCount = 5;
                TrackingThrustSpeed = 2;
                break;
            case > 400:
                ShotCount = 6;
                TrackingThrustSpeed = 1;
                break;
            case <= 400:
                ShotCount = 6;
                TrackingThrustSpeed = 1;
                break;
        }
    }

    IEnumerator FireTrackingThrust(int shots, float time)
    {
        for (int i = 0; i < shots; i++)
        {
            Instantiate(突刺觸手, new Vector3(GameObject.Find("Player").transform.position.x, 35f, 0), Quaternion.Euler(0, 0, 0));

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(time);
        }

        //發射完後暫停X秒
        StartCoroutine(AttackModeChange(1));
    }

    IEnumerator FireCrossThrust(int shots, float time)
    {
        for (int i = 0; i < shots; i++)
        {
            Instantiate(突刺觸手, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y + 30f, 0), Quaternion.Euler(0, 0, 0));
            if (GameObject.Find("Player").transform.position.x > -11f)
            {
                Instantiate(突刺觸手, new Vector3(GameObject.Find("Player").transform.position.x + 30f, GameObject.Find("Player").transform.position.y, 0), Quaternion.Euler(0, 0, 270));
            }
            else
            {
                Instantiate(突刺觸手, new Vector3(GameObject.Find("Player").transform.position.x - 30f, GameObject.Find("Player").transform.position.y, 0), Quaternion.Euler(0, 0, 90));
            }

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(time);
        }

        //發射完後暫停X秒
        StartCoroutine(AttackModeChange(1));
    }

    void WideSweep()
    {
        if (GameObject.Find("Player").transform.position.x > -11f)
        {
            Instantiate(橫掃觸手, new Vector3(5f, 25f, 0), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Instantiate(橫掃觸手, new Vector3(-25f, 25f, 0), Quaternion.Euler(0, 180, 0));
        }

        //發射完後暫停X秒
        StartCoroutine(AttackModeChange(3));
    }

    IEnumerator FireInfiniteTentacles()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(突刺觸手, new Vector3(Random.Range(GameObject.Find("Player").transform.position.x - 10f, GameObject.Find("Player").transform.position.x + 11f), 35f, 0), Quaternion.Euler(0, 0, 0));

            // 等待time秒再發射下一個
            yield return new WaitForSeconds(0.5f);
        }

        //發射完後暫停X秒
        StartCoroutine(AttackModeChange(1));
    }

    void Ink()
    {
        GameObject.Find("噴墨").GetComponent<Animator>().Play("噴墨", 0, 0f);

        GetComponent<AudioSource>().PlayOneShot(噴墨);
    }

    IEnumerator AttackModeChange(float time)
    {

        //機率選擇招式
        int RandomAttackMode = UnityEngine.Random.Range(1, 101);

        if (HP <= 800)
        {
            switch (RandomAttackMode)
            {
                case <= 40:
                    AttackMode = 4;
                    break;
                case <= 70:
                    AttackMode = 2;
                    break;
                case > 70:
                    AttackMode = 3;
                    break;
            }

            //半血噴墨
            int RandomInk = UnityEngine.Random.Range(1, 101);
            switch (RandomInk)
            {
                case <= 75:
                    Ink();
                    break;
                case > 75:

                    break;
            }
        }
        else
        {
            switch (RandomAttackMode)
            {
                case <= 40:
                    AttackMode = 1;
                    break;
                case <= 70:
                    AttackMode = 2;
                    break;
                case > 70:
                    AttackMode = 3;
                    break;
            }
        }

        //招式間隔
        yield return new WaitForSeconds(time);

        //攻擊前置
        switch (AttackMode)
        {
            case 1:
                StartCoroutine(FireTrackingThrust(ShotCount, TrackingThrustSpeed));
                break;
            case 2:
                StartCoroutine(FireCrossThrust(ShotCount, 3));
                break;
            case 3:
                WideSweep();
                break;
            case 4:
                StartCoroutine(FireInfiniteTentacles());
                break;
        }
    }

    void Dead()
    {
        if (HP <= 0 && !PlayerManager.OctopusDead)
        {
            PlayerManager.OctopusDead = true;
            AttackMode = 0;
            StopCoroutine("AttackModeChange");
            StartCoroutine(FireDestructionExplosion(30, 0.15f));
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
}
