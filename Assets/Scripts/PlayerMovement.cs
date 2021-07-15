// ========== HMOVE ==========
//
//     PlayerMovement.cs
//
// Use keyboard to move player
//
// ===========================

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -10f;
    public float jumpHeight = 5f;
    public float runSpeed = 14f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    public bool running;
    public bool crouching;

    public float originalColliderHeight;

    public Vector3 move;

    public GameObject camera;

    public BoxCollider crouchUpCheck;

    void Start()
    {
        originalColliderHeight = controller.height;    
    }

    public void updateCameraPosition()
    {
        // updates camera position depending on height
        camera.transform.position = new Vector3(transform.position.x, transform.position.y + (controller.height / 2), transform.position.z);
    }

    public void crouching_updateYPosition()
    {
        // updates player when crouching, for example. stops falling
        transform.position = new Vector3(transform.position.x, transform.position.y - (originalColliderHeight / 2), transform.position.z);
    }

    public void updateCheckerPosition()
    {
        // moves the checker to keep its orignial position, also move it up a little bit. messy i know
        crouchUpCheck.transform.position = new Vector3(transform.position.x, transform.position.y + ((originalColliderHeight - controller.height) / 2)+0.2f, transform.position.z);
    }

    public Vector2 crouchSpherecastOrigin;

    public bool isCrouching()
    {
        // gonna check crouch
        bool crouch = Input.GetKey(KeyCode.LeftControl);

        if (crouch == false)
        {
            if (crouching == true)
            {
                // we're trying to uncrouch. we wanna check we actually can

                bool test = Physics.CheckBox(crouchUpCheck.transform.position, 
                    new Vector3(crouchUpCheck.size.x / 2, crouchUpCheck.size.y / 1.8f, crouchUpCheck.size.z / 2),
                    crouchUpCheck.transform.rotation, 
                    groundMask);

                if(test == true)
                {
                    Debug.Log("supposedly colldiing");
                    return true;
                }
                else
                {
                    // if we're uncrouching and theres nothing above us we're good to go
                    return false;
                }
            }
            else
            {
                // eh
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    void Update()
    {
        crouching = isCrouching(); // check left ctrl for crouch

        if (crouching == false)
        {
            // don't want to run if we're crouching
            running = Input.GetKey(KeyCode.LeftShift);
        }
        
        if(crouching == true)
        {
            // we'll move all that stuff
            controller.height = originalColliderHeight / 1.5f;
            crouching_updateYPosition();
            updateCameraPosition();
            updateCheckerPosition();
        }
        else
        {
            // we don't need to but we'll do it anyway
            controller.height = originalColliderHeight;
            updateCameraPosition();
            updateCheckerPosition();
        }

        isGrounded = controller.isGrounded; // the controller's isgrounded is better than anything we can do.
        Global.Grounded = controller.isGrounded; // do that as well

        if (isGrounded && velocity.y < 0)
        {
            // if we're not falling and we're not grounded then
            // gravity moment
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        move = transform.right * x + transform.forward * z; // move no shit

        if (Input.GetButton("Jump") && isGrounded)
        {
            // jumpy jumpy
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; // gravity again

        float finalSpeed = 0;

        if(running == true)
        {
            // if we're running then rnu
            finalSpeed = runSpeed;
        }
        else if(crouching == true)
        {
            // else crouch speed
            finalSpeed = speed / 2;
        }
        else
        {
            // else normal speed
            finalSpeed = speed;
        }

        controller.Move(((move * finalSpeed) + velocity) * Time.deltaTime);
    }
}