using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float Speed = 15;
    public float speedLimit = 20;

    public float jumpForce = 5;
    public float jumpTime = 2;
    public float timer;

    public bool grounded;

    public Rigidbody2D rb2d;
    public Animator anim;
    public GameObject Shadow;

    private bool CanMove = true;             //Character is able to move
    private bool CanRun = true;              //Can the player Run
    private bool CanJump = true;             //Can the player Jump

    public bool running = false;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void FixedUpdate() {
        running = false;
        if (CanMove) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            //float vertical = Input.GetAxisRaw("Vertical");

            //Check to if the player is running or walking by holding down shift
            if (Input.GetKey(KeyCode.LeftShift) && CanRun) {
                Run(horizontal);
            }else{
                Walk(horizontal, speedLimit);
            }

            if ((Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) && CanJump) {
                Jump();
            }
        }

        if(Shadow != null)
            MoveShadow();

        //Animator
        CheckIfGrounded();
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat("Vertical Velocity", rb2d.velocity.y);
    }

    void MoveShadow() {
        Vector2 newPos = new Vector2(transform.position.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.1f), Vector2.down);
        newPos.y = transform.position.y - hit.distance;
        Shadow.transform.position = newPos;
        float modifier = hit.distance * 0.1f;
        if(modifier > 1 && modifier < 20)
            Shadow.transform.localScale = new Vector3(5/modifier, 1, 1);
        else 
            Shadow.transform.localScale = new Vector3(5, 1, 1);
    }

    void Jump() {
        //Initializing jump needs to be grounded
        if (grounded) {
                timer = Time.time + jumpTime;
                Vector2 vl = rb2d.velocity;
                vl.y += jumpForce;
                rb2d.velocity = vl;
        }

        //Need to hold for longer jump and can't run out of time.
        if (Time.time < timer) {
            Vector2 vl = rb2d.velocity;
            vl.y += jumpForce;
            rb2d.velocity = vl;
        }

        //If they let go of the jump button their time is reduced
        if (Input.GetKeyUp(KeyCode.Space)) {
            timer = Time.time - 5;
        }
    }

    void Walk(float hor, float sLimit) {
        if (Mathf.Abs(hor) > 0.1f) {

            //Gets rid of skidding

            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = (hor < 0);
            rb2d.AddForce(transform.right * Speed * hor, ForceMode2D.Impulse);

            //Applies equal force backwards to limit the character without affecting velocity
            if (Mathf.Abs(rb2d.velocity.x) >= sLimit)
                rb2d.AddForce(transform.right * Speed * -hor, ForceMode2D.Impulse);

        } else if(grounded){
            rb2d.velocity = new Vector2(rb2d.velocity.x / 5, rb2d.velocity.y);
        }
    }

    //Same thing as Walk but the speed limit is higher
    void Run(float hor) {
        running = true;
        if(grounded)
            Walk(hor, speedLimit * 3);
    }

    //Check grounded
    private void CheckIfGrounded() {
        Vector2 positionToCheck = new Vector2(transform.position.x, transform.position.y + 0.1f);
        int layerMask = ~LayerMask.GetMask("Player");
        if (rb2d.velocity.y > 2) {
            grounded = false;
            return;
        }
        grounded = Physics2D.Raycast(positionToCheck, Vector2.down, 0.3f, layerMask);
    }
}
