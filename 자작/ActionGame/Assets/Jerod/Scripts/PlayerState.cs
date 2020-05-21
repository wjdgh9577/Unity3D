using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State { Idle, Move, Attack, Jump , Fall};

    public State state { get; private set; }
    public bool armed = false;

    private Animator playerAnimator;
    private PlayerInput playerInput;

    private void OnEnable()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.tabButtonDown) armed = false;
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) state = State.Idle;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) state = State.Move;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkBack")) state = State.Move;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) state = State.Jump;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall")) state = State.Fall;
    }
}
