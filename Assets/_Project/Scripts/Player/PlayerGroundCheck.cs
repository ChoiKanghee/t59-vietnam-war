using UnityEngine;

namespace T59VietnamWar.Player
{
    public sealed class PlayerGroundCheck : MonoBehaviour
    {
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField, Min(0.01f)] private float groundCheckRadius = 0.15f;
        [SerializeField] private LayerMask groundLayers;

        public bool IsGrounded => groundCheckPoint != null &&
            Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayers) != null;

        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
