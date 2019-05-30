using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 7f;
    Rigidbody rb;
    Collider coll; //to keep the collider object

    public AudioSource coinAudioSource;
    public HudManager hud;

    bool pressedJump = false; //flag to keep track of whether a jump started

    void Awake() {
        coll = GetComponent<Collider>(); //get the player collider
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //get the rigid body component for later use
       
        hud.Refresh(); //refresh the HUD

    }

    // Update is called once per frame
    void Update()
    {        
        WalkHandler(); // Handle player walking        
        JumpHandler(); //Handle player jumping
    }

    /**
     * Make the player walk according to user input
     */
    void WalkHandler() 
    {        
        rb.velocity = new Vector3(0, rb.velocity.y, 0); // Set x and z velocities to zero        
        float distance = walkSpeed * Time.deltaTime; // Distance ( speed = distance / time --> distance = speed * time)        
        float hAxis = Input.GetAxis("Horizontal"); // Input on x ("Horizontal")        
        float vAxis = Input.GetAxis("Vertical"); // Input on z ("Vertical")        
        Vector3 movement = new Vector3(hAxis * distance, 0f, vAxis * distance); // Movement vector x,y,z        
        Vector3 currPosition = transform.position; // Current position        
        Vector3 newPosition = currPosition + movement; // New position        
        rb.MovePosition(newPosition); // Move the rigid body
    }
    /**
     * Check whether the player can jump and make it jump
     */
    void JumpHandler() 
    {        
        float jAxis = Input.GetAxis("Jump"); // Jump axis
        bool isGrounded = CheckGrounded();  // Is grounded

        if (jAxis > 0f) // Check if the player is pressing the jump key
        {            
            if (!pressedJump && isGrounded) {
                pressedJump = true; // We are jumping on the current key press
                Vector3 jumpVector = new Vector3(0f, jumpSpeed, 0f); // Jumping vector            
                rb.velocity = rb.velocity + jumpVector; // Make the player jump by adding velocity
            }
            else
            {
                pressedJump = false; // Update flag so it can jump again if we press the jump key
            }
        }
    }

    // Check if the object is grounded
    bool CheckGrounded()
    {
        // Object size in x
        float sizeX = coll.bounds.size.x;
        float sizeZ = coll.bounds.size.z;
        float sizeY = coll.bounds.size.y;

        // Position of the 4 bottom corners of the game object
        // We add 0.01 in Y so that there is some distance between the point and the floor
        Vector3 corner1 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner2 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner3 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);
        Vector3 corner4 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);

        // Send a short ray down the cube on all 4 corners to detect ground
        bool grounded1 = Physics.Raycast(corner1, new Vector3(0, -1, 0), 0.01f);
        bool grounded2 = Physics.Raycast(corner2, new Vector3(0, -1, 0), 0.01f);
        bool grounded3 = Physics.Raycast(corner3, new Vector3(0, -1, 0), 0.01f);
        bool grounded4 = Physics.Raycast(corner4, new Vector3(0, -1, 0), 0.01f);
        
        

        // If any corner is grounded, the object is grounded
        return (grounded1 || grounded2 || grounded3 || grounded4);
    }

    //Visualize cast ray limits
    /*void OnDrawGizmos()
    {

        // Object size in x
        float sizeX = coll.bounds.size.x;
        float sizeZ = coll.bounds.size.z;
        float sizeY = coll.bounds.size.y;

        Vector3 corner1 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner2 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner3 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);
        Vector3 corner4 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(corner1, new Vector3(1, 1, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(corner2, new Vector3(1, 1, 1));

        Gizmos.color = Color.green;
        Gizmos.DrawCube(corner3, new Vector3(1, 1, 1));

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(corner4, new Vector3(1, 1, 1));
    }*/

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "coin")  // Check if we ran into a coin
        {            
            print("Grabing coin...");
            GameManager.instance.IncreaseScore(1); // Increase score
            hud.Refresh(); //refresh the HUD
            coinAudioSource.Play(); // Play coin collection sound
            Destroy(collider.gameObject); // Destroy coin
        }
        else if (collider.gameObject.tag == "enemy")
        {
            // Game over
            print("game over");
            GameManager.instance.changeScene("GameOver");

            // Soon.. go to the game over scene
        }
        else if (collider.gameObject.tag == "goal")
        {
            // Next level
            print("next level");
            GameManager.instance.IncreaseLevel();
        }
    }

}
