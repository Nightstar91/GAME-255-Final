using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Final
{
    public class Player : MonoBehaviour
    {
        // Start of declaring variable for my player entity
        // Specific action variable
        private InputAction move;
        private InputAction look;
        private InputAction jump;

        // Mapping for input variable
        PlayerControlMapping mapping;

        // Variable to handle the player's camera movement
        private float mouseDeltaX = 0f;
        private float mouseDeltaY = 0f;
        private float cameraRotX = 0f;
        private int rotDir = 0;

        // Attributes for player's stats
        public float speed = 8f;
        public int coinAmount;
        private float jumpForce = 250f;
        private float rotation = 300f;

        // Boolean to handle jumping and first time movement
        public bool grounded = false;
        public bool firstMovement = false;

        // Variable to handle the component of the player's rigid body
        private Rigidbody rb;


        private void OnEnable()
        {
            move.Enable();
            look.Enable();
            jump.Enable();


            jump.performed += Jump;
        }

        private void OnDisable()
        {
            move.Disable(); 
            look.Disable();
            jump.Disable();

            jump.performed -= Jump;
        }


        // Initializing player object for movement
        private void Awake()
        {
            mapping = new PlayerControlMapping();

            rb = GetComponent<Rigidbody>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            move = mapping.Player.Move;
            look = mapping.Player.Look;
            jump = mapping.Player.Jump;
        }

        private void Start()
        {

        }


        // Collecting coins
        public void GetCoin(int amt)
        {
            coinAmount += amt;
        }


        void Jump(InputAction.CallbackContext context)
        {
            if(grounded == true)
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }


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

        void HandleMovement()
        {
            Vector2 axis = move.ReadValue<Vector2>();

            Vector3 input = (axis.x * transform.right) + (transform.forward * axis.y);

            input *= speed;

            rb.velocity = new Vector3(input.x, rb.velocity.y, input.z);
        }

        void HandleHorizontalRotation()
        {

            mouseDeltaX = look.ReadValue<Vector2>().x;

            if (mouseDeltaX != 0)
            {
                rotDir = mouseDeltaX > 0 ? 1 : -1;

                transform.eulerAngles += new Vector3(0, (rotation * 1.5f) * Time.deltaTime * rotDir, 0);
            }
        }

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
    }
}
