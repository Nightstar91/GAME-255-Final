using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace Final
{
    public class Player : MonoBehaviour
    {
        // Declaring variable
        // Specific action variable
        private InputAction move;
        private InputAction look;
        private InputAction jump;
        private InputAction fire;

        // Mapping for input variable
        PlayerControlMapping mapping;

        // Getting GameManager class for the timer
        public GameManager manager;

        // Variable to handle the player's camera movement
        private float mouseDeltaX = 0f;
        private float mouseDeltaY = 0f;
        private float cameraRotX = 0f;
        private int rotDir = 0;

        // Attributes for player's stats
        public float totalScore;
        public float levelScore = 0f;
        private float score = 0f;
        public float speed = 8f;
        public int coinAmount;
        private float jumpForce = 250f;
        public float rotation;

        // Boolean to handle game logic
        public bool grounded = false;
        public bool beginLevel;
        public bool levelIsFinished;

        // Variable to handle the component of the player's rigid body
        private Rigidbody rb;

        // Variable to be use for event
        public static Player instance;
        public UnityEvent beginCountDownEvent;

        // Initializing player object for movement
        private void Awake()
        {
            // Related to unityevent syntax
            instance = this;

            // Default value for beginLevel
            beginLevel = false;
            levelIsFinished = false;

            // For player control
            mapping = new PlayerControlMapping();

            // Loading in the sensivity saved inside the mainmenu and totalscore
            rotation = PlayerPrefs.GetInt("Sensitivity", 500);
            totalScore += PlayerPrefs.GetFloat("Total Score", 0);

            // initializing the rigidbody and cursor mode
            rb = GetComponent<Rigidbody>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Setting up the player control
            move = mapping.Player.Move;
            look = mapping.Player.Look;
            jump = mapping.Player.Jump;
            fire = mapping.Player.Fire;

            // Finding the gameManager in the scene and then caching it inside a variable
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }


        private void OnEnable()
        {
            move.Enable();
            look.Enable();
            jump.Enable();
            fire.Enable();


            jump.performed += Jump;
            fire.performed += Fire;
        }


        private void OnDisable()
        {
            move.Disable(); 
            look.Disable();
            jump.Disable();
            fire.Disable();

            jump.performed -= Jump;
            fire.performed -= Fire;
        }


        // When the game starts
        private void Start()
        {
            // Freeze the player's movement
            FreezePlayer();

            // Display to the player that they are frozen
            manager.DisplayLevelFrozen();
        }


        // To be use for unityEvent
        public static UnityEvent GetBeginCountDownEvent()
        {
            return instance.beginCountDownEvent;
        }


        private void FixedUpdate()
        {
            HandleMovement();
        }


        // Update is called once per frame
        void Update()
        {
            grounded = IsGrounded();

            HandleHorizontalRotation();
            HandleVerticalRotation();
        }


        // Collecting coins
        public void GetCoin(int amt)
        {
            coinAmount += amt;
        }


        // Gaining speed whenever a coin is collected
        public void GainSpeed(float amt)
        {
            speed += amt;
        }


        // Gaining points whenever a coin is collected
        public void GainPoint(float amt)
        {
            score += amt * manager.timer;
            levelScore = (float)Math.Round(score, 2);
        }


        // Method to be use for whenever the player fail to collect all the coin, punishment is to set point to 0 for that level
        public void SetPointZero()
        {
            levelScore = 0;
        }


        // For Jumping...
        void Jump(InputAction.CallbackContext context)
        {
            //Debug.Log("Jump");
            if(grounded == true)
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }


        // The input that will start the level once the player loads in
        void Fire(InputAction.CallbackContext context)
        {
            // When the player loads in the level, once they left click...
            if (beginLevel == false)
            {
                // Unfreeze the player to give them back control
                UnfreezePlayer();

                // Start the countdown event
                beginCountDownEvent.Invoke();

                // Set the level status to be true
                beginLevel = true;
            }

            // Once the level is finished...
            if (beginLevel == true && levelIsFinished == true)
            {
                // Add to the total score
                totalScore += levelScore;

                // Saving the player totalscore
                PlayerPrefs.SetFloat("Total Score", totalScore);

                // Load to the next level when the player leftclick when prompted
                SceneManager.LoadScene(manager.TransitionToNextLevel());
            }
        }


        // Strip the player control (typically used when instructing the player)
        public void FreezePlayer()
        {
            move.Disable();
            look.Disable();
        }


        // Bring control back to the player
        public void UnfreezePlayer()
        {
            move.Enable();
            look.Enable();
        }


        // Handle logic for when the player is in the air
        bool IsGrounded()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 3;

            RaycastHit hit;

            // Does the ray intersect any object beside the player layer
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out hit, 1.1f, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up * -1) * hit.distance, Color.yellow);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                return false;
            }

        }
        

        // Logic for player movement
        void HandleMovement()
        {
            Vector2 axis = move.ReadValue<Vector2>();

            Vector3 input = (axis.x * transform.right) + (transform.forward * axis.y);

            input *= speed;

            rb.velocity = new Vector3(input.x, rb.velocity.y, input.z);
        }


        // Handling of the camera's horizontal rotation
        void HandleHorizontalRotation()
        {

            mouseDeltaX = look.ReadValue<Vector2>().x;

            if (mouseDeltaX != 0)
            {
                rotDir = mouseDeltaX > 0 ? 1 : -1;

                transform.eulerAngles += new Vector3(0, (rotation * 1.5f) * Time.deltaTime * rotDir, 0);
            }
        }


        // Handling of the camera's vertical rotation
        void HandleVerticalRotation()
        {
            mouseDeltaY = look.ReadValue<Vector2>().y;

            if (mouseDeltaY != 0)
            {
                rotDir = mouseDeltaY > 0 ? -1 : 1;

                cameraRotX += rotation * Time.deltaTime * rotDir;
                cameraRotX = Mathf.Clamp(cameraRotX, -45f, 45f);

                var targetRotation = Quaternion.Euler(Vector3.right * cameraRotX);


                //Vector3 angle = new Vector3(rotation * Time.deltaTime * rotDir, 0, 0);

                //Debug.Log(Camera.main.transform.localRotation.x);

                Camera.main.transform.localRotation = targetRotation;
                //Camera.main.transform.Rotate(angle, Space.Self);

            }
        }
    }
}
