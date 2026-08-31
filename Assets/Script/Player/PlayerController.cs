using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerController : MonoBehaviour
{
    Animator playerAnimator;

    float MoveSpeed = 5f;
    bool playerHasXSpeed;

    float JumpSpeed = 10f;
    float SlideWallSpeed = 3f;
    float maxFallSpeed = -12.5f;
    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 10f;
    bool isGround;
    bool isLeftWall;
    bool isRightWall;
    bool isSlidingWall;
    bool isWallJumping;
    bool canDoubleJump = true;

    float dashSpeed = 7f;
    float dashTime = 0.2f;
    float startDashTime;
    bool isDashing;
    bool isDashAttack;
    bool canDoubleDash = true;
    //用於間隔閃避時間 無法連續無限閃避
    bool canDash = true;

    public BoxCollider2D isGroundHitBox;
    public PolygonCollider2D AttackHitBox;
    public PolygonCollider2D AttackHitBoxUp;
    public PolygonCollider2D AttackHitBoxDown;
    public BoxCollider2D DashAttackHitBox;
    bool canAttack = true;
    bool Attacking;
    bool UpAttacking;
    bool DownAttacking;

    bool recoil;
    bool recoilDown;
    float recoilSpeed = 4f;
    float recoillDownSpeed = 7.5f;
    float recoilTime = 0.15f;
    float startrecoilTime;

    bool canSkill = true;
    bool UsingEnergyCannon;
    public GameObject energyCannon;
    public GameObject energyExplosion;

    bool damage;
    bool canDamage = true;
    float damageRecoilSpeed = 6f;
    float damageRecoilTime = 0.2f;
    float startDamageRecoilTime;
    int beforeDamageHP;

    bool isInvincible;
    float startInvincibleTime;

    //玩家音效
    public AudioClip 揮刀;
    public AudioClip 擊中;
    public AudioClip 上下攻擊;
    public AudioClip 閃避;
    public AudioClip 二段跳;
    public AudioClip 能源砲;
    public AudioClip 能量屏障;
    public AudioClip 維修工具;
    public AudioClip 受傷;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
        if (!PlayerManager.Dead)
        {
            if (!damage)
            {
                Move();
                Flip();
                Jump();
                DoubleJump();
                WallJump();
                //Dash();
                DashAttack();
                Attack();
                AttackRecoil();
                if (PlayerManager.canSkill)
                {
                    EnergyCannon();
                    EnergyExplosion();
                }
                if (PlayerManager.canHeal)
                {
                    Heal();
                }
            }
            Damage();
            DisableInvincibility();
        }
        else
        {
            //死亡時速度移動歸零
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            //死亡時切換無敵圖層
            gameObject.layer = LayerMask.NameToLayer("PlayerInvincibility");
        }
    }

    void Animation()
    {
        if (PlayerManager.Dead)
        {
            playerAnimator.Play("Dead");
        }
        else if (damage)
        {
            playerAnimator.Play("Damage");
        }
        else if (isSlidingWall && Input.GetAxis("Horizontal") != 0)
        {
            playerAnimator.Play("SlideWall");
        }
        else if (isDashing || isDashAttack)
        {
            if (isDashing)
            {
                playerAnimator.Play("Dash");
            }
            if (isDashAttack)
            {
                playerAnimator.Play("DashAttack");
            }
        }
        else if (UsingEnergyCannon)
        {
            playerAnimator.Play("UsingEnergyCannon");
        }
        else if (DownAttacking)
        {
            playerAnimator.Play("DownAttack");
        }
        else if (UpAttacking)
        {
            playerAnimator.Play("UpAttack");
        }
        else if (Attacking)
        {
            playerAnimator.Play("Attack");
        }
        else if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            playerAnimator.Play("Fall");
        }
        else if (GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            if (!canDoubleJump)
            {
                playerAnimator.Play("DoubleJump");
            }
            else
            {
                playerAnimator.Play("Jump");
            }
        }
        else if (playerHasXSpeed)
        {
            playerAnimator.Play("Run");
        }
        else
        {
            playerAnimator.Play("Idle");
        }
    }

    void Move()
    {
        if (!recoil && !damage)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void Flip()
    {
        playerHasXSpeed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        if (playerHasXSpeed && !recoil && !damage)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (GetComponent<Rigidbody2D>().velocity.x < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Jump()
    {
        isGround = isGroundHitBox.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGround)
        {
            canDoubleJump = true;
            canDoubleDash = true;
            if (Input.GetButtonDown("Jump") && !isWallJumping)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, JumpSpeed);
            }
        }
        //判斷垂直速度是否小於零(下落中),如果正在下落,將增加物體的垂直速度,實現更自然的重力效果
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            GetComponent<Rigidbody2D>().velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //如果物體的垂直速度大於零(正在上升)且沒有按住跳躍按鈕,將增加物體的垂直速度,實現短跳
        else if (GetComponent<Rigidbody2D>().velocity.y > 0 && !Input.GetButton("Jump"))
        {
            GetComponent<Rigidbody2D>().velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        //設定最大落下速度
        if (GetComponent<Rigidbody2D>().velocity.y < maxFallSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxFallSpeed);
        }
    }

    void DoubleJump()
    {
        if (!isGround)
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, JumpSpeed * 0.9f);
                canDoubleJump = false;

                GetComponent<AudioSource>().PlayOneShot(二段跳);
            }
        }
    }

    void WallJump()
    {
        //確認抓住牆壁和牆壁位置
        isLeftWall = Physics2D.OverlapCircle(transform.position - new Vector3(0.4f, 0, 0), 0.1f, LayerMask.GetMask("Ground"));
        isRightWall = Physics2D.OverlapCircle(transform.position + new Vector3(0.4f, 0, 0), 0.1f, LayerMask.GetMask("Ground"));
        //抓住牆壁時會滑落,滑落時按跳躍會往反方向蹬牆
        if (isLeftWall && Input.GetAxis("Horizontal") < 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isWallJumping = true;
                Invoke("SetWallJumpingToFalse", 0.1f);
            }
            else
            {
                isSlidingWall = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -SlideWallSpeed);
            }
        }
        if (isRightWall && Input.GetAxis("Horizontal") > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isWallJumping = true;
                Invoke("SetWallJumpingToFalse", 0.1f);
            }
            else
            {
                isSlidingWall = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -SlideWallSpeed);
            }
        }
        if (isWallJumping)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10f * -Input.GetAxis("Horizontal"), JumpSpeed);
        }
        if (!isLeftWall && !isRightWall)
        {
            isSlidingWall = false;
        }
    }

    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    void Dash()
    {
        if (!isDashing)
        {
            if(Input.GetButtonDown("Dash") && canDoubleDash && canDash)
            {
                isDashing = true;
                startDashTime = dashTime;
            }
        }
        else
        {
            startDashTime -= Time.deltaTime;
            if (startDashTime > 0)
            {
                if(GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                    GetComponent<Rigidbody2D>().velocity += Vector2.right * dashSpeed;
                }
                else if(GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                    GetComponent<Rigidbody2D>().velocity += Vector2.left * dashSpeed;
                }
            }
            else
            {
                isDashing = false;
                canDoubleDash = false;
                canDash = false;
                StartCoroutine(DashTime());
            }
        }
    }

    void DashAttack()
    {
        if (!isDashAttack)
        {
            if (Input.GetButtonDown("Dash") && canDoubleDash && canDash)
            {
                isDashAttack = true;
                startDashTime = dashTime + 0.05f ;
                //切換成無敵圖層
                gameObject.layer = LayerMask.NameToLayer("PlayerInvincibility");
                if (isInvincible)
                {
                    startInvincibleTime += 0.25f;
                }
                else
                {
                    isInvincible = true;
                    startInvincibleTime = 0.25f;
                }

                //閃擊攻擊(!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!)
                //canAttack = false;
                //DashAttackHitBox.enabled = true;
                //StartCoroutine(DisableAttackHitBox());

                GetComponent<AudioSource>().PlayOneShot(閃避);
            }
        }
        else
        {
            startDashTime -= Time.deltaTime;
            if (startDashTime > 0)
            {
                if (GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                    GetComponent<Rigidbody2D>().velocity += Vector2.right * (dashSpeed + 2f);
                }
                else if (GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                    GetComponent<Rigidbody2D>().velocity += Vector2.left * (dashSpeed + 2f);
                }
            }
            else
            {
                isDashAttack = false;
                canDoubleDash = false;
                canDash = false;
                StartCoroutine(DashTime());
            }
        }
    }

    //Dash後間隔0.3秒才能再次Dash
    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(0.3f);
        canDash = true;
    }

    void Attack()
    {
        //跳躍時向下攻擊
        if (Input.GetButtonDown("Attack") && Input.GetAxis("Vertical") < 0 && canAttack && !isGround)
        {
            canAttack = false;
            DownAttacking = true;
            AttackHitBoxDown.enabled = true;
            StartCoroutine(DisableAttackHitBox());

            GetComponent<AudioSource>().PlayOneShot(上下攻擊);
        }
        //向上攻擊
        else if (Input.GetButtonDown("Attack") && Input.GetAxis("Vertical") > 0 && canAttack)
        {
            canAttack = false;
            UpAttacking = true;
            AttackHitBoxUp.enabled = true;
            StartCoroutine(DisableAttackHitBox());

            GetComponent<AudioSource>().PlayOneShot(上下攻擊);
        }
        //向前攻擊
        else if (Input.GetButtonDown("Attack") && canAttack)
        {
            canAttack = false;
            Attacking = true;
            AttackHitBox.enabled = true;
            StartCoroutine(DisableAttackHitBox());
            //攻擊時暫停移動
            //GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

            GetComponent<AudioSource>().PlayOneShot(揮刀);
        }
    }

    IEnumerator DisableAttackHitBox()
    {
        //結束攻擊
        //攻擊動畫0.2秒
        yield return new WaitForSeconds(0.2f);
        AttackHitBox.enabled = false;
        AttackHitBoxUp.enabled = false;
        AttackHitBoxDown.enabled = false;
        DashAttackHitBox.enabled = false;
        Attacking = false;
        UpAttacking = false;
        DownAttacking = false;
        //可以攻擊
        yield return new WaitForSeconds(0.15f);
        canAttack = true;
    }

    void AttackRecoil()
    {
        //攻擊後座力
        if (recoil)
        {
            startrecoilTime -= Time.deltaTime;
            if (startrecoilTime > 0)
            {
                if (transform.rotation.y == 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-recoilSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(recoilSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            else
            {
                recoil = false;
            }
        }
        //下攻擊後座力
        if (recoilDown)
        {
            startrecoilTime -= Time.deltaTime;
            if (startrecoilTime > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, recoillDownSpeed);
            }
            else
            {
                recoilDown = false;
            }
        }
    }

    void EnergyCannon()
    {
        if (Input.GetKeyDown(KeyCode.V) && canSkill && PlayerManager.SkillPoint > 0)
        {
            if (transform.rotation.y == 0)
            {
                Instantiate(energyCannon, transform.position + new Vector3(2, 0, 0), transform.rotation);
            }
            else
            {
                Instantiate(energyCannon, transform.position + new Vector3(-2, 0, 0), transform.rotation);
            }
            PlayerManager.SkillPoint--;
            canSkill = false;
            canAttack = false;
            UsingEnergyCannon = true;
            StartCoroutine(setCanSkill());
            //攝影機震動
            CameraShake.Instance.EnergyCannon.GenerateImpulse();

            GetComponent<AudioSource>().PlayOneShot(能源砲);
        }
    }

    void EnergyExplosion()
    {
        if (Input.GetKeyDown(KeyCode.F) && canSkill && PlayerManager.SkillPoint > 0)
        {
            Instantiate(energyExplosion, transform.position, transform.rotation);
            PlayerManager.SkillPoint--;
            canSkill = false;
            StartCoroutine(setCanSkill());

            GetComponent<AudioSource>().PlayOneShot(能量屏障);
        }
    }

    IEnumerator setCanSkill()
    {
        //可以攻擊和恢復動作
        yield return new WaitForSeconds(0.3f);
        UsingEnergyCannon = false;
        canAttack = true;
        //可以技能
        yield return new WaitForSeconds(1.7f);
        canSkill = true;
    }

    void Heal()
    {
        if (Input.GetKeyDown(KeyCode.D) && PlayerManager.SkillPoint > 0)
        {
            switch (PlayerManager.HPLevel)
            {
                case 0:
                    PlayerManager.HP += 50;
                    break;
                case 1:
                    PlayerManager.HP += 75;
                    break;
            }
            PlayerManager.SkillPoint--;

            GetComponent<AudioSource>().PlayOneShot(維修工具);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //攻擊到敵人
        if (AttackHitBox.IsTouching(collision) && collision.gameObject.tag == "Enemy")
        {
            if (!isDashAttack)
            {
                recoil = true;
                startrecoilTime = recoilTime;

                //攻擊到敵人後 攻擊的碰撞箱消失
                AttackHitBox.enabled = false;

                GetComponent<AudioSource>().PlayOneShot(擊中);
            }
        }
        //下攻擊到敵人
        if (AttackHitBoxDown.IsTouching(collision) && collision.gameObject.tag == "Enemy")
        {
            recoilDown = true;
            startrecoilTime = recoilTime;
            canDoubleJump = true;
            canDoubleDash = true;

            //攻擊到敵人後 攻擊的碰撞箱消失
            AttackHitBoxDown.enabled = false;

            GetComponent<AudioSource>().PlayOneShot(擊中);
        }
    }

    void Damage()
    {
        if (damage)
        {
            //受傷後退後
            startDamageRecoilTime -= Time.deltaTime;
            if (startDamageRecoilTime > 0)
            {
                if (transform.rotation.y == 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-damageRecoilSpeed, damageRecoilSpeed / 2);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(damageRecoilSpeed, damageRecoilSpeed / 2);
                }
            }
            else
            {
                damage = false;
            }
            //切換無敵圖層
            gameObject.layer = LayerMask.NameToLayer("PlayerInvincibility");
        }
        else
        {
            if (beforeDamageHP > PlayerManager.HP && beforeDamageHP !<= PlayerManager.MaxHP[PlayerManager.HPLevel])
            {
                if (canDamage)
                {
                    damage = true;
                    canDamage = false;
                    //受傷後無敵時間2秒
                    isInvincible = true;
                    startInvincibleTime = 2f;

                    //播放無敵閃爍動畫
                    if (!PlayerManager.Dead)
                    {
                        playerAnimator.Play("Invincibility", 1, 0f);
                    }
                    //攝影機震動
                    CameraShake.Instance.PlayerDamage.GenerateImpulse();

                    //開始受傷退後計時
                    startDamageRecoilTime = damageRecoilTime;

                    GetComponent<AudioSource>().PlayOneShot(受傷);
                }
            }
            beforeDamageHP = PlayerManager.HP;
        }
    }

    //從無敵狀態回復
    void DisableInvincibility()
    {
        if (isInvincible)
        {
            if (startInvincibleTime > 0)
            {
                startInvincibleTime -= Time.deltaTime;
            }
            else
            {
                isInvincible = false;
                canDamage = true;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
    }
}
