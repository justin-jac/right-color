using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PlayerController controller;
    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && !ColorChange.currentColor.Equals(ColorChange.grey))
        {
            SoundManager.PlaySound("jump");
            jump = true;
            animator.SetBool("isJumping", true);
            SoundManager.PlaySound("jump");
        }
    }

    public void onLnading()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {
        //Move Character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
