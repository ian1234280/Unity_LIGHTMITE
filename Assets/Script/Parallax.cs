using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parallax : MonoBehaviour
{
    bool CanParallax;

    public bool repositionX;
    public bool repositionY;
    public float parallaxSpeed;
    private Transform cameraTransform;
    private Transform playerTransform;
    private Vector3 lastCameraPosition;

    public Collider2D boundingCollider;
    public bool upDownBoundStart;
    public bool DownBoundStart;
    public bool leftRightBoundStart;
    public bool upDownBoundContinued;
    public bool leftRightBoundContinued;

    public bool notMove;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        playerTransform = GameObject.Find("Player").transform;

        //開始場景時跟隨物件的父物件為攝影機 達到不會移動的效果
        if (notMove)
        {
            gameObject.transform.SetParent(Camera.main.transform);
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            //開始場景時移動(調整)視差背景位置
            if (repositionX)
            {
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
            }
            if (repositionY)
            {
                transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
            }

            // 取得圖片的邊界框
            Bounds bounds = GetComponent<SpriteRenderer>().bounds;

            // 檢查圖片的邊界框是否超出碰撞器範圍
            bool isOutside = !boundingCollider.bounds.Contains(bounds.min) || !boundingCollider.bounds.Contains(bounds.max);

            // 如果圖片的邊界框超出碰撞器範圍，則將圖片移回範圍內
            if (isOutside)
            {
                // 計算要移動的位移量
                Vector3 displacement = Vector3.zero;

                if (leftRightBoundStart)
                {
                    // 左邊界超出
                    if (bounds.min.x < boundingCollider.bounds.min.x)
                    {
                        displacement.x += boundingCollider.bounds.min.x - bounds.min.x;
                    }
                    // 右邊界超出
                    else if (bounds.max.x > boundingCollider.bounds.max.x)
                    {
                        displacement.x -= bounds.max.x - boundingCollider.bounds.max.x;
                    }
                }

                if (upDownBoundStart)
                {
                    // 下邊界超出
                    if (bounds.min.y < boundingCollider.bounds.min.y)
                    {
                        displacement.y += boundingCollider.bounds.min.y - bounds.min.y;
                    }
                    // 上邊界超出
                    else if (bounds.max.y > boundingCollider.bounds.max.y)
                    {
                        displacement.y -= bounds.max.y - boundingCollider.bounds.max.y;
                    }
                }
                if (DownBoundStart)
                {
                    // 下邊界超出
                    if (bounds.min.y < boundingCollider.bounds.min.y)
                    {
                        displacement.y += boundingCollider.bounds.min.y - bounds.min.y;
                    }
                }

                // 將圖片移回範圍內
                transform.position += displacement;
            }
        }

        Invoke("startParallax", 0.5f);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!notMove && CanParallax)
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += deltaMovement * parallaxSpeed;
            lastCameraPosition = cameraTransform.position;
        }
    }

    void startParallax()
    {
        lastCameraPosition = cameraTransform.position;
        CanParallax = true;
    }

    void Update()
    {
        if (boundingCollider == null || GetComponent<SpriteRenderer>() == null)
            return;

        // 取得圖片的邊界框
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;

        // 檢查圖片的邊界框是否超出碰撞器範圍
        bool isOutside = !boundingCollider.bounds.Contains(bounds.min) || !boundingCollider.bounds.Contains(bounds.max);

        // 如果圖片的邊界框超出碰撞器範圍，則將圖片移回範圍內
        if (isOutside)
        {
            // 計算要移動的位移量
            Vector3 displacement = Vector3.zero;

            if (leftRightBoundContinued)
            {
                // 左邊界超出
                if (bounds.min.x < boundingCollider.bounds.min.x)
                {
                    displacement.x += boundingCollider.bounds.min.x - bounds.min.x;
                }
                // 右邊界超出
                else if (bounds.max.x > boundingCollider.bounds.max.x)
                {
                    displacement.x -= bounds.max.x - boundingCollider.bounds.max.x;
                }
            }

            if (upDownBoundContinued)
            {
                // 下邊界超出
                if (bounds.min.y < boundingCollider.bounds.min.y)
                {
                    displacement.y += boundingCollider.bounds.min.y - bounds.min.y;
                }
                // 上邊界超出
                else if (bounds.max.y > boundingCollider.bounds.max.y)
                {
                    displacement.y -= bounds.max.y - boundingCollider.bounds.max.y;
                }
            }

            // 將圖片移回範圍內
            transform.position += displacement;
        }
    }
}
