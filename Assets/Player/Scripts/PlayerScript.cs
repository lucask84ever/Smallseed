using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // GAMEOBJECT
    public CharacterController controller;

    // Vectors
    private float rotation;
    private float translation;
    private Vector3 velocity;

    // Public Forces
    public float speed = 1f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 1f;
    private bool isGrounded = false;

    // GROUND
    public LayerMask groundMask;
    public Transform flagGround;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(flagGround.position, groundDistance, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(z > 0) {
            animator.SetBool("isWalking", true);
        }else if(z == 0) {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingBackwards", false);
        } else {
            animator.SetBool("isWalkingBackwards", true);
        }

        if(x > 0) {
            animator.SetBool("isStrafingRight", true);
            animator.SetBool("isStrafingLeft", false);
        } else if (x == 0) {
            animator.SetBool("isStrafingRight", false);
            animator.SetBool("isStrafingLeft", false);
        } else {
            animator.SetBool("isStrafingRight", false);
            animator.SetBool("isStrafingLeft", true);
        }

        if(isGrounded && velocity.y < 1.1f) {
            velocity.y = -1f;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);

        }

        Vector3 movimento = transform.right * x + transform.forward * z;

        controller.Move(movimento * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded) {
            animator.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }

        if(!isGrounded && velocity.y < -2) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
