using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    int ATK;

    float Speed = 10f;
    Vector3 Target;

    // Start is called before the first frame update
    void Start()
    {
        ATK = 20;

        Target = GameObject.Find("Player").transform.position;

        //旋轉面相目標
        Vector2 direction = Target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        StartCoroutine(Destroy(5f));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
        transform.Translate(0, Speed * Time.deltaTime, 0);
    }

    IEnumerator Destroy(float S)
    {
        yield return new WaitForSeconds(S);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.HP -= ATK;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //被玩家攻擊到
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
        //被玩家技能攻擊到
        if (collision.gameObject.tag == "Skill")
        {
            Destroy(gameObject);
        }
    }
}
