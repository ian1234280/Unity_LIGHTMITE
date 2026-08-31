using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSphere : MonoBehaviour
{
    float MoveSpeed = 0f;
    float RotationSpeed = 3f;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion(5));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Track();
    }

    void Track()
    {
        //移動
        transform.Translate(new Vector2(0, MoveSpeed * Time.deltaTime));

        //慢慢加速
        if (MoveSpeed < 10f)
        {
            MoveSpeed += 0.15f;
        }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        //觸碰到玩家
        if (collision.gameObject.tag == "Player")
        {
            //觸碰到玩家就爆炸
            StartCoroutine(Explosion(0));
        }
    }

    IEnumerator Explosion(float T)
    {
        yield return new WaitForSeconds(T);
        yield return new WaitForSeconds(0.05f);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
