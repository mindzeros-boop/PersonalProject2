using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (_anim == null) return;

        _anim.SetFloat("MoveX", Mathf.Abs(_playerMovement._moveDir.x));
        _anim.SetBool("IsGrounded", !_playerMovement._isJumping);

        if (_playerMovement._moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_playerMovement._moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
