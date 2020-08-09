using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{

    // Is Armed
    private bool isUseGun = false;
    // Controller
    public CharacterController controller;
    
    // GROUND
    public LayerMask groundMask;
    public Transform flagGround;
    private bool isGrounded = true;

    // Animator
    public Animator animator;
    public RuntimeAnimatorController  unarmed;
    public RuntimeAnimatorController  armed;

    // Positions
    private float z;
    private float x;

    // Physics
    private Vector3 movimento;
    private Vector3 velocity;
    public float speed = 1f;
    public float runningSpeed = 5f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 1f;

    private CharacterStateEnum state;

    void Start()
    {
        animator.runtimeAnimatorController = unarmed;
        state = CharacterStateEnum.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Mouse2)) {
            isUseGun = !isUseGun;
            animator.runtimeAnimatorController = isUseGun ? armed : unarmed;
        }

        if(Input.GetButtonDown("Jump") && isGrounded) {
            state = CharacterStateEnum.JUMPING;
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            animator.SetInteger("state", (int) state);
            return;
        }

        if(!isGrounded && velocity.y < -0.4) {
            state = CharacterStateEnum.FALLING;
            velocity.y = -1f;
        }

        isGrounded = Physics.CheckSphere(flagGround.position, groundDistance, groundMask);

        movimento = transform.right * x + transform.forward * z;

        if(state == CharacterStateEnum.JUMPING){
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        ChangeStateUpdate();
        
        controller.Move(movimento * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void ChangeStateUpdate() 
    {
        if(isGrounded) {
            if(z > 0 && x == 0) {
                if(Input.GetKey(KeyCode.LeftShift)) {
                    state = CharacterStateEnum.RUNNING;
                    speed = 5f;
                } else {
                    state = CharacterStateEnum.WALKING;
                    speed = 1f;
                }
            } else if(x == 0 && z == 0) {
                state = CharacterStateEnum.IDLE;
                speed = 1f;
            } else if(z < 0 && x == 0){
                state = CharacterStateEnum.WALKING_BACKWARDS;
                speed = 1f;
            } else if (z > 0 && x > 0) {
                state = CharacterStateEnum.DIAGONAL_WALKING_FRONT_RIGHT;
                speed = 1f;
            } else if (z < 0 && x > 0) {
                state = CharacterStateEnum.DIAGONAL_WALKING_BACK_RIGHT;
                speed = 1f;
            } else if (z == 0 && x > 0) {
                state = CharacterStateEnum.STRAFING_RIGHT;
                speed = 1f;
            } else if (z > 0 && x < 0) {
                state = CharacterStateEnum.DIAGONAL_WALKING_FRONT_LEFT;
                speed = 1f;
            } else if (z < 0 && x < 0) {
                state = CharacterStateEnum.DIAGONAL_WALKING_BACK_LEFT;
                speed = 1f;
            } else if (z == 0 && x < 0) {
                state = CharacterStateEnum.STRAFING_LEFT;
                speed = 1f;
            }
        }
        animator.SetInteger("state", (int) state);
    }
}

public enum CharacterStateEnum
{
    IDLE                         = 0,
    WALKING                      = 1,
    RUNNING                      = 2,
    STRAFING_RIGHT               = 3,
    STRAFING_LEFT                = 4,
    WALKING_BACKWARDS            = 5,
    JUMPING                      = 6,
    FALLING                      = 7,
    DIAGONAL_WALKING_FRONT_LEFT  = 8,
    DIAGONAL_WALKING_FRONT_RIGHT = 9,
    DIAGONAL_WALKING_BACK_LEFT   = 10,
    DIAGONAL_WALKING_BACK_RIGHT  = 11,
}