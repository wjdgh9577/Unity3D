using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 플레이어 클래스
 * 플레이어의 행동이나 공격에 관한 입출력을 처리한다.
 * UI와 관련된 입출력은 GameController나 SceneManager에서 처리한다.
 */
public class Player_TPS : MonoBehaviour
{
    //스텟 관련 변수
    Status_Player status;
    private float runSpeedRatio = 1.4f;       //달리기 이동속도 비율
    private int jumpCount = 2;           //점프 가능 횟수

    //스킬 관련 변수
    public GameObject skill1;            //Jab
    public GameObject skill2;            //Hikick
    public GameObject skill3;            //Rising Punch
    public GameObject skill4;            //Spinkick
    public GameObject skill5;            //Screwkick
    HitDecision_Player jab;
    HitDecision_Player hikick;
    HitDecision_Player risingpunch;
    HitDecision_Player spinkick;
    HitDecision_Player screwkick;

    //카메라 관련 변수
    public GameObject mainCamera;
    public GameObject saveCamera;
    private Camera_TPS mainCam;

    //게임컨트롤러 관련 변수
    public GameObject gameController;
    GameController gc;
    PlayerAudio aud;

    //애니메이션 관련 변수
    private float animSpeed = 1.5f;      //애니메이션 속도

    private Animator anim;
    private AnimatorStateInfo currentBaseState;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Run");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int fallState = Animator.StringToHash("Base Layer.Fall");
    static int landState = Animator.StringToHash("Base Layer.Land");
    static int jabState = Animator.StringToHash("Base Layer.Jab");
    static int hikickState = Animator.StringToHash("Base Layer.Hikick");
    static int risingpunchState = Animator.StringToHash("Base Layer.Risingpunch");
    static int spinkickState = Animator.StringToHash("Base Layer.Spinkick");
    static int chargingState = Animator.StringToHash("Base Layer.Charging");
    static int screwkickState = Animator.StringToHash("Base Layer.Screwkick");

    //계산 변수
    private Vector3 movedir;
    private Rigidbody rb;

    private float nonFightTime = 5;      //비전투 시간
    private bool isJump = false;
    private bool isAttack = false;       //공격모션인지 판단
    private bool isAirborne = false;     //공중모션인지 판단
    private bool attack1 = false;        //마우스 좌클릭
    private bool attack2 = false;        //마우스 우클릭
    private bool attack3 = false;        //ALT키 필살기


    private void Start()
    {
        this.status = GetComponent<Status_Player>();
        
        this.jab = this.skill1.GetComponent<HitDecision_Player>();
        this.hikick = this.skill2.GetComponent<HitDecision_Player>();
        this.risingpunch = this.skill3.GetComponent<HitDecision_Player>();
        this.spinkick = this.skill4.GetComponent<HitDecision_Player>();
        this.screwkick = this.skill5.GetComponent<HitDecision_Player>();

        this.mainCam = this.mainCamera.GetComponent<Camera_TPS>();

        this.gc = this.gameController.GetComponent<GameController>();
        this.aud = GetComponent<PlayerAudio>();

        this.anim = GetComponent<Animator>();

        this.rb = GetComponent<Rigidbody>();

        DontDestroyOnLoad(gameObject);
    }

    //캐릭터 입력과 관련된 처리
    private void Update()
    {
        this.currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        this.isAttack = IsAttack();
        this.isAirborne = IsAirborne();
        this.movedir = Vector3.zero;

        //이동, 공격과 관련된 입력
        if (!this.mainCam.isLock && !this.isAttack && this.status.state == 0)
        {
            //키보드 입력
            bool w = Input.GetKey(KeyCode.W);
            bool a = Input.GetKey(KeyCode.A);
            bool s = Input.GetKey(KeyCode.S);
            bool d = Input.GetKey(KeyCode.D);
            bool alt = Input.GetKeyDown(KeyCode.LeftAlt);
            bool shift = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetButtonDown("Jump") && this.jumpCount > 0)
            {
                this.jumpCount--;
                this.isJump = true;
            }

            if (w) this.movedir += new Vector3(this.mainCamera.transform.forward.x, 0, this.mainCamera.transform.forward.z);
            else if (s) this.movedir -= new Vector3(this.mainCamera.transform.forward.x, 0, this.mainCamera.transform.forward.z);
            if (a) this.movedir -= new Vector3(this.mainCamera.transform.right.x, 0, this.mainCamera.transform.right.z);
            else if (d) this.movedir += new Vector3(this.mainCamera.transform.right.x, 0, this.mainCamera.transform.right.z);

            this.movedir = Vector3.Normalize(this.movedir);
            if (Vector3.SqrMagnitude(this.movedir) > 0) transform.forward = this.movedir;

            if (shift)
            {
                this.movedir *= this.runSpeedRatio;
                this.anim.SetFloat("RunSpeed", this.runSpeedRatio);
            }
            else this.anim.SetFloat("RunSpeed", 1);

            if (alt) this.attack3 = true;

            if (Setting.cursorLockOn)
            {
                //마우스 입력
                if (Input.GetMouseButtonDown(0)) this.attack1 = true;
                if (Input.GetMouseButtonDown(1)) this.attack2 = true;
            }

            //예외처리
            if (this.isAirborne || this.isJump)
            {
                this.attack1 = false;
                this.attack2 = false;
            }
        }

        //UI와 관련된 입력
        
    }

    //물리작용과 관련된 처리
    private void FixedUpdate()
    {
        this.nonFightTime += Time.fixedDeltaTime;

        MoveCharactor();
        Attack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Field" || collision.gameObject.tag == "Monster")
        {
            this.jumpCount = 2;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Field" || collision.gameObject.tag == "Monster")                             //필드 또는 몬스터에 착지하면 점프 가능
        {
            this.anim.SetBool("Land", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Field" || collision.gameObject.tag == "Monster") this.anim.SetBool("Land", false);   //필드 또는 몬스터에서 떨어지면 공중으로 판단
    }

    //캐릭터 이동과 관련된 처리
    private void MoveCharactor()
    {
        this.anim.SetFloat("Speed", Vector3.SqrMagnitude(this.movedir));
        this.anim.SetFloat("yVel", this.rb.velocity.y);
        this.anim.speed = this.animSpeed;

        //궁극기중 낙하 방지
        if (this.currentBaseState.fullPathHash == chargingState || this.currentBaseState.fullPathHash == screwkickState)
        {
            this.rb.velocity = Vector3.zero;
            this.rb.useGravity = false;
        }
        else this.rb.useGravity = true;

        //이동
        this.rb.velocity = new Vector3(0, this.rb.velocity.y, 0);
        this.rb.AddForce(this.movedir * this.status.speed, ForceMode.VelocityChange);

        //점프
        if (this.isJump)
        {
            this.anim.SetTrigger("Jump");
            this.aud.JumpSound();
            this.isJump = false;
            this.rb.velocity = new Vector3(this.rb.velocity.x, 0, this.rb.velocity.z);
            this.rb.AddForce(Vector3.up * this.status.jumpPower, ForceMode.VelocityChange);
        }
    }

    //캐릭터 공격과 관련된 처리
    private void Attack()
    {
        //공격
        if (this.attack1 && !this.isAttack)                         //Jab, Rising Punch
        {
            this.attack1 = false;
            if (this.movedir != Vector3.zero && this.status.stamina >= 30)
            {
                this.nonFightTime = 0;
                this.anim.SetTrigger("Risingpunch");
                this.aud.RisingpunchSound();
                this.status.SpendStamina(30);
                StartCoroutine(this.risingpunch.HitEnemy_Risingpunch());
            }
            else
            {
                this.nonFightTime = 0;
                this.anim.SetTrigger("Jab");
                this.aud.JabSound();
                StartCoroutine(this.jab.HitEnemy_Jab());
            }
        }
        else if (this.attack2 && !this.isAttack)                    //Hikick, Spinkick
        {
            this.attack2 = false;
            if (this.movedir != Vector3.zero && this.status.stamina >= 50)
            {
                this.nonFightTime = 0;
                this.anim.SetTrigger("Spinkick");
                this.aud.SpinkickSound();
                this.status.SpendStamina(50);
                StartCoroutine(this.spinkick.HitEnemy_Spinkick());
            }
            else if (this.status.stamina >= 5)
            {
                this.nonFightTime = 0;
                this.anim.SetTrigger("Hikick");
                this.aud.HikickSound();
                this.status.SpendStamina(5);
                StartCoroutine(this.hikick.HitEnemy_Hikick());
            }
        }
        else if (this.attack3 && !this.isAttack)                   //Screwkick
        {
            this.attack3 = false;
            if (this.status.stamina >= 100)
            {
                this.nonFightTime = 0;
                this.anim.SetTrigger("Charging");
                this.aud.ChargingSound();
                this.status.SpendStamina(100);
                StartCoroutine(this.screwkick.HitEnemy_Screwkick());
            }
        }
        this.anim.SetFloat("NonFightTime", this.nonFightTime);

        //예외처리
        if (this.isAttack)
        {
            this.anim.ResetTrigger("Jab");
            this.anim.ResetTrigger("Hikick");
            this.anim.ResetTrigger("Risingpunch");
            this.anim.ResetTrigger("Spinkick");
            this.anim.ResetTrigger("Charging");
            this.anim.ResetTrigger("Jump");
        }
        if (this.isAirborne)
        {
            this.anim.ResetTrigger("Jab");
            this.anim.ResetTrigger("Hikick");
            this.anim.ResetTrigger("Risingpunch");
            this.anim.ResetTrigger("Spinkick");
        }
    }

    //공중에 있으면 true 반환
    bool IsAirborne()
    {
        if (this.currentBaseState.fullPathHash == jumpState) return true;
        else if (this.currentBaseState.fullPathHash == fallState) return true;

        return false;
    }

    //공격 중이면 true 반환
    bool IsAttack()
    {
        if (this.currentBaseState.fullPathHash == jabState) return true;
        if (this.currentBaseState.fullPathHash == hikickState) return true;
        if (this.currentBaseState.fullPathHash == risingpunchState) return true;
        if (this.currentBaseState.fullPathHash == spinkickState) return true;
        if (this.currentBaseState.fullPathHash == chargingState) return true;
        if (this.currentBaseState.fullPathHash == screwkickState) return true;

        return false;
    }

    public IEnumerator CallCutScene()
    {
        this.mainCam.isLock = true;
        this.mainCam.SetCutScene();
        yield return new WaitForSeconds(1f);
        this.mainCam.isLock = false;
        this.mainCamera.transform.forward = this.saveCamera.transform.forward;
        this.anim.SetTrigger("Screwkick");
        this.aud.ScrewkickSound();
    }
}