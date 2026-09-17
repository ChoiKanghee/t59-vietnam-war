using UnityEngine;
using UnityEngine.InputSystem;

namespace T59VietnamWar.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerGroundCheck))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 6f;
        [SerializeField, Min(0f)] private float jumpSpeed = 12f;

        [Header("Input")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;

        private Rigidbody2D body;
        private PlayerGroundCheck groundCheck;
        private float moveInput;
        private bool jumpRequested;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            groundCheck = GetComponent<PlayerGroundCheck>();
        }

        private void OnEnable()
        {
            if (moveAction != null) moveAction.action.Enable();
            if (jumpAction != null) jumpAction.action.Enable();
        }

        private void OnDisable()
        {
            if (moveAction != null) moveAction.action.Disable();
            if (jumpAction != null) jumpAction.action.Disable();
            moveInput = 0f;
            jumpRequested = false;
        }

        private void Update()
        {
            moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>().x : 0f;
            if (jumpAction != null && jumpAction.action.WasPressedThisFrame())
                jumpRequested = true;
        }

        private void FixedUpdate()
        {
            Vector2 velocity = body.linearVelocity;
            velocity.x = moveInput * moveSpeed;

            if (jumpRequested && groundCheck.IsGrounded)
                velocity.y = jumpSpeed;

            body.linearVelocity = velocity;
            jumpRequested = false;
        }
    }
}
