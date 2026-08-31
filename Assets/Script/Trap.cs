using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    int ATK;
    Vector3 RecordPosition;
    Vector3 backPosition;

    float idleRunTime = 0f; // 記錄 Idle/Run 的持續時間
    float recordDelay = 0.1f; // 需要持續多久才記錄

    Animator Transition;

    // Start is called before the first frame update
    void Start()
    {
        ATK = 30;

        //獲得場景過度的動畫器
        Transition = GameObject.Find("Transition").GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //偵測是否在邊緣
        bool isEdge;
        bool isLeftOnGround = Physics2D.Raycast(new Vector2(GameObject.Find("Player").transform.position.x - 0.8f, GameObject.Find("Player").transform.position.y - 3.3f), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        bool isRightOnGround = Physics2D.Raycast(new Vector2(GameObject.Find("Player").transform.position.x + 0.8f, GameObject.Find("Player").transform.position.y - 3.3f), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (isLeftOnGround || isRightOnGround)
        {
            isEdge = true;
            Debug.Log("邊緣戰士");
        }
        else
        {
            isEdge = false;
        }

        //在陷阱裡&邊緣不紀錄
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("isGroundHitBox")))
        {
            if (!isEdge)
            {
                RecordBackPosition();
            }
        }
    }

    void RecordBackPosition()
    {
        Animator animator = GameObject.Find("Player").GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 檢查是否在 "Idle" 或 "Run" 動畫狀態
        bool isIdle = stateInfo.IsName("Idle");
        bool isRunning = stateInfo.IsName("Run");

        if (isIdle || isRunning)
        {
            //Debug.Log("動作確認");
            idleRunTime += Time.deltaTime;

            if (idleRunTime >= recordDelay)
            {
                RecordPosition = GameObject.Find("Player").transform.position;
                Debug.Log("記錄玩家位置：" + RecordPosition);
                // 重置計時
                idleRunTime = 0f;
            }
        }
        else
        {
            // 如果離開 Idle/Run 狀態，重置計時
            idleRunTime = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.HP -= ATK;

            StartCoroutine(Back());
        }
    }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(0.05f);
        if (!PlayerManager.Dead)
        {
            backPosition = RecordPosition;
            Transition.Play("Transition_Start", 0, 0f);
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("Player").transform.position = backPosition;
            Transition.Play("Transition_End", 0, 0f);
        }
    }
}
