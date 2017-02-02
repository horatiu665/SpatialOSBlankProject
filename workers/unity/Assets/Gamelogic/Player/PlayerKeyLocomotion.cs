namespace Assets.Gamelogic
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerController))]
    public sealed class PlayerKeyLocomotion : MonoBehaviour
    {
        [SerializeField]
        private bool _allowKeyInput = true;

        [SerializeField]
        private KeyCode _moveForward = KeyCode.W;

        [SerializeField]
        private KeyCode _moveLeft = KeyCode.A;

        [SerializeField]
        private KeyCode _moveRight = KeyCode.D;

        [SerializeField]
        private KeyCode _moveBack = KeyCode.S;

        private PlayerController _player;

        private void Awake()
        {
            _player = this.GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!_allowKeyInput || !_player.hasAuthority)
            {
                return;
            }

            Vector3 velocity = Vector3.zero;
            if (Input.GetKey(_moveForward))
            {
                velocity = this.transform.forward;
            }
            else if (Input.GetKey(_moveBack))
            {
                velocity = -this.transform.forward;
            }

            if (Input.GetKey(_moveRight))
            {
                velocity += this.transform.right;
            }
            else if (Input.GetKey(_moveLeft))
            {
                velocity += -this.transform.right;
            }

            this.transform.position += velocity.normalized * _player.moveSpeed * Time.deltaTime;
        }
    }
}