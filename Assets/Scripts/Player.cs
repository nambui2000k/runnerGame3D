using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 1;//0: left, 1:mid, 2:right
    public float laneDistance = 4;// the distance between two lanes 
    public float jumpForce;
    public float gravity = -20;
    public Animator anim;
    public CapsuleCollider capco;
    private bool isSliding = false;
    private bool isJumping = false;
    private bool isRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;
        if (!isRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            isRotation = true;
        }
        // increase Speed
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        anim.SetBool("isRun", true);
        direction.z = forwardSpeed;

        if (controller.isGrounded)
        {

            if (SwipeManager.swipeUp && !isJumping)
            {
                direction.y = jumpForce;
                StartCoroutine(AnimJump());
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }

        if (SwipeManager.swipeRight)
        {

            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }

        if (SwipeManager.swipeLeft)
        {

            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        if (SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(AnimSlide());
        }
        Vector3 targetPosition = transform.position.z
            * transform.forward + transform.position.y
            * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
        if (transform.position.y < -2f)
        {
            transform.position = new Vector3(transform.position.x, -1.23f, transform.position.z);
        }


    }
    private void FixedUpdate()
    {

        controller.Move(direction * Time.fixedDeltaTime);
    }
    private void Jump()
    {

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "obstacles")
        {
            PlayerManager.gameOver = true;
        }
    }
    private IEnumerator AnimJump()
    {
        isJumping = true;
        AfterSlide();
        Jump();
        anim.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        isJumping = false;
        anim.SetBool("isJump", false);


    }
    private IEnumerator AnimSlide()
    {
        BeforeSlide();
        yield return new WaitForSeconds(1.0f);
        AfterSlide();
    }
    void BeforeSlide()
    {
        isSliding = true;
        anim.SetBool("isSlide", true);
        controller.center = new Vector3(0, 0.0018f, 0);
        controller.height = 0.0015f;
    }
    void AfterSlide()
    {
        anim.SetBool("isSlide", false);
        controller.center = new Vector3(0, 0.0026f, 0);
        controller.height = 0.005f;
        isSliding = false;
    }
}