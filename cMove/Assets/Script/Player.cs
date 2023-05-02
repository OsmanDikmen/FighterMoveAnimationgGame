using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float rGround;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask enem;

    private Vector3 moveDirection;
    private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        { 
            shot();
            Attack();
        }
    }
    
    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical");
        isGrounded = Physics.CheckSphere(transform.position, rGround, layer); //controller.isGrounded; 

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        if(isGrounded)
        {
            if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
            walk();

            }else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
            run();

            }else if(moveDirection == Vector3.zero)
            {
            idle();
            }
            moveDirection *= walkSpeed;
            if(Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

        velocity.y += gravity * Time.deltaTime;
        

        controller.Move(velocity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void idle()
    {
        anim.SetFloat("Blend", 0);
    }
    private void walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Blend", 0.5f);
    }
    private void run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Blend", 1);
    }
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
    }
    private void shot()
    {
        Ray middle = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if(Physics.Raycast(middle, out hit , 4f, enem))
        {
            Debug.Log("Vurdun");
        }
    }
}
