using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpAmount = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathKick = new Vector2 (0f,15f);
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bullet2;
    [SerializeField] Transform gun;
    [SerializeField] float downAmount = 1f;
    Vector2 moveInput;
    Rigidbody2D rgb;
    Animator myAnimation;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;
    float startGravityAmount;
    bool playerHasHorizontalSpeed;
    public bool isAlive = true;
    bool isShoot = false;
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        myAnimation = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        startGravityAmount = rgb.gravityScale;
    }
    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipCharacter();
        ClimbLadder();
        Die();
        Swim();
    }
    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if(!isAlive){return;}
        if(!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground","Water"))){return;}
        if(value.isPressed)
        {
            rgb.velocity += new Vector2 (0f,jumpAmount);
        }
    }
    void OnFire(InputValue value)
    {
        if(value.isPressed && !isShoot)
        {
            if(!isAlive){return;}
            isShoot = true;
            myAnimation.SetBool("IsFire",isShoot);
            if(transform.localScale.x < 0){
                Instantiate(bullet, gun.position, transform.rotation);
            }
            else{
                Instantiate(bullet2, gun.position, transform.rotation);
            }
            Invoke("StopFire",0.7f);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rgb.velocity.y);
        rgb.velocity = playerVelocity; 
        playerHasHorizontalSpeed = Mathf.Abs(rgb.velocity.x) > Mathf.Epsilon;
        myAnimation.SetBool("IsRun", playerHasHorizontalSpeed);
        if(Input.GetKey(KeyCode.S))
            {
                rgb.velocity += new Vector2 (0f,-downAmount);
            }
    }
    void FlipCharacter()
    {
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rgb.velocity.x), 1f);
        }
    }
    void ClimbLadder()
    {
        if(!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rgb.gravityScale = startGravityAmount;
            myAnimation.SetBool("IsClimbing",false);
            return;
        }

        Vector2 climbVelocity = new Vector2 (rgb.velocity.x, moveInput.y * climbSpeed);
        bool playerHasVerticalSpeed = Mathf.Abs(rgb.velocity.y) > Mathf.Epsilon;
        rgb.velocity = climbVelocity; 
        rgb.gravityScale = 0f;
        myAnimation.SetBool("IsClimbing",playerHasVerticalSpeed);
    }
    void Die(){
        if(capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spikes")))
        {
            isAlive = false;
            myAnimation.SetTrigger("Dying");
            rgb.velocity = deathKick;
            StopMove();
            FindObjectOfType<GameSession>().ProccesPlayerDeath();
        }
    }
    void StopFire(){
        isShoot = false;
        myAnimation.SetBool("IsFire",isShoot);
    }
    void Swim()
    {
        if(!isAlive){return;}
        if(capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Water")) && playerHasHorizontalSpeed)
        {
            myAnimation.SetBool("IsSwim",true);
            myAnimation.SetBool("IsRun", false);
        }
        else{
            myAnimation.SetBool("IsSwim",false);
        }
    }
    void StopMove(){
        if(boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground","Water")))
        {
            Destroy(capsuleCollider,0.2f);
            Destroy(boxCollider,0.2f);
        }
    }
}
