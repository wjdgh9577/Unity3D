using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 780.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //치트키
        if (Input.GetMouseButtonDown(0)) transform.position = new Vector3(0.64f, 20f, 0);

        //점프한다.
        if (Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        //좌우 이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //스피드 제한
        if(speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        if (transform.position.x > 9.8f)
        {
            Vector3 playerPos = transform.position;
            transform.position = new Vector3(9.8f, playerPos.y, playerPos.z);
        }
        else if (transform.position.x < -9.8f)
        {
            Vector3 playerPos = transform.position;
            transform.position = new Vector3(-9.8f, playerPos.y, playerPos.z);
        }

        //움직이는 방향에 따라 이미지 반전
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        //플레이어의 속도에 맞춰 애니메이션 속도를 바꾼다.
        if (this.rigid2D.velocity.y == 0) this.animator.speed = speedx / 2.0f;
        else this.animator.speed = 1.0f;

        if (transform.position.y < -10) SceneManager.LoadScene("GameScene1");
    }
    //골 도착
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("ClearScene");
    }
}
