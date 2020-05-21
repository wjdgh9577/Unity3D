using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform followCam;

    private float runSpeedRate = 3f;  //달리기 속도 증가 비율
    private float moveSpeed = 2f;  //기본 이동속도
    private float jumpPower = 5f;  //점프력
    private bool landing = true;  //착지

    private Rigidbody playerRigidbody;
    private PlayerInput playerInput;
    private PlayerState playerState;
    private Animator playerAnimator;

    private Vector3 moveDirection;

    private void OnEnable()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float vertical = playerInput.vertical;
        float horizontal = playerInput.horizontal;
        float shift = playerInput.shift;
        bool scrollButton = playerInput.scrollButton;
        bool space = playerInput.space;

        //공격, 낙하중이 아닐 경우에만 움직임
        if (playerState.state != PlayerState.State.Attack && playerState.state != PlayerState.State.Fall)
        {
            Vector3 camForwardHorizon = new Vector3(followCam.forward.x, 0, followCam.forward.z).normalized;
            Vector3 camRightHorizon = followCam.right;

            if (playerState.state == PlayerState.State.Idle || playerState.state == PlayerState.State.Move) //이동
            {
                moveDirection = Vector3.zero;

                if (scrollButton)
                {
                    moveDirection += transform.forward * vertical + transform.right * horizontal;
                }
                else if (horizontal != 0 || vertical != 0)
                {
                    transform.forward = camForwardHorizon;
                    moveDirection += camForwardHorizon * vertical + camRightHorizon * horizontal;
                }

                if (space && landing)
                {
                    playerAnimator.SetTrigger("Jump");
                }

                moveDirection = moveDirection.normalized * moveSpeed;
            }
            else if (playerState.state == PlayerState.State.Jump)  //점프 도중에는 속도, 방향 고정
            {
                moveDirection = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            }

            if (vertical > 0 && shift > 0 && playerState.state != PlayerState.State.Jump)
            {
                moveDirection *= runSpeedRate;
                vertical += shift;
            }

            playerRigidbody.velocity = moveDirection + new Vector3(0, playerRigidbody.velocity.y, 0);
        }

        playerAnimator.SetFloat("Vertical", vertical);
        playerAnimator.SetFloat("Horizontal", horizontal);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Field" && collision.contacts[0].normal.y > 0.5f)
        {
            landing = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Field")
        {
            landing = false;
        }
    }

    //점프 애니메이션 이벤트
    public void Jump()
    {
        playerRigidbody.AddForce(transform.up * 5, ForceMode.VelocityChange);
    }
}