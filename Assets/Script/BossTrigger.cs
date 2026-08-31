using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    CinemachineVirtualCamera BossVirtualCamera;

    public GameObject Boss;
    public Vector3 BossPosition;
    public Quaternion BossRotation;
    
    public bool Rhino;
    public bool Octopus;
    public bool DestroyerSeries;
    public bool LenoBoss;
    bool BossDead;

    // Start is called before the first frame update
    void Start()
    {
        //設定Boss場景的攝影機
        BossVirtualCamera = GameObject.Find("Boss Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        BossVirtualCamera.Follow = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Return();
        Defeat();
    }

    void Return()
    {
        if (Rhino)
        {
            BossDead = PlayerManager.RhinoDead;
        }
        if (Octopus)
        {
            BossDead = PlayerManager.OctopusDead;
        }
        if (DestroyerSeries)
        {
            BossDead = PlayerManager.DestroyerSeriesDead;
        }
        if (LenoBoss)
        {
            BossDead = PlayerManager.LenoBossDead;
        }

        if (BossDead)
        {
            BossVirtualCamera.Priority = 0;
            GameObject.Find("Boss Wall").GetComponent<Collider2D>().enabled = false;
            GameObject.Find("Boss Wall1").GetComponent<Collider2D>().enabled = false;

            //停止BGM
            GetComponent<AudioSource>().Stop();
        }
    }

    void Defeat()
    {
        if (Rhino && PlayerManager.RhinoDead)
        {
            GameObject.Find("Boss Trigger").GetComponent<Collider2D>().enabled = false;
        }
        if (Octopus && PlayerManager.OctopusDead)
        {
            GameObject.Find("Boss Trigger").GetComponent<Collider2D>().enabled = false;
        }
        if (DestroyerSeries && PlayerManager.DestroyerSeriesDead)
        {
            GameObject.Find("Boss Trigger").GetComponent<Collider2D>().enabled = false;
        }
        if (LenoBoss && PlayerManager.LenoBossDead)
        {
            GameObject.Find("Boss Trigger").GetComponent<Collider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            //生成Boss
            Instantiate(Boss, BossPosition, BossRotation);

            //設定Boss場景的攝影機
            BossVirtualCamera.Priority = 20;

            //開啟隱形牆 關閉觸發器
            GameObject.Find("Boss Trigger").GetComponent<Collider2D>().enabled = false;
            GameObject.Find("Boss Wall").GetComponent<Collider2D>().enabled = true;
            GameObject.Find("Boss Wall1").GetComponent<Collider2D>().enabled = true;

            //播放BGM
            GetComponent<AudioSource>().Play();
        }
    }
}
