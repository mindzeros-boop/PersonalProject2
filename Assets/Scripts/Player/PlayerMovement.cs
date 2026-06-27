using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jump = 35f;
    private PhysicsMaterial2D _noFriction; // 마찰력
    private PhysicsMaterial2D _normalFriction; // 일반 마찰력
    private Rigidbody2D _rigid;
    public Vector2 _moveDir { get; private set; }
    public bool _isJumping { get; private set; } = false;
    public bool _isJumped = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        _noFriction = new PhysicsMaterial2D("NoFriction");
        _noFriction.friction = 0f; // 마찰력 없음

        _normalFriction = new PhysicsMaterial2D("NormalFriction");
        _normalFriction.friction = 0.4f; // 일반 마찰력 설정
    }

    private void FixedUpdate()
    {
        _rigid.linearVelocity = new Vector2(_moveDir.x * _speed, _rigid.linearVelocity.y);

        if (_isJumped && !_isJumping)
        {
            _rigid.AddForce(Vector2.up * _jump, ForceMode2D.Impulse);
            _isJumping = true;
        }
        _isJumped = false;
    }

    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed) { _isJumped = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            foreach (ContactPoint2D contact in collision.contacts) // 충돌 지점의 접촉점들을 확인
            {
                if (contact.normal.y >= 0.5f)
                {
                    _isJumping = false;
                    _rigid.sharedMaterial = _normalFriction; // 바닥에 닿았을 때 일반 마찰력 적용
                    break;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            bool isWall = true;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y >= 0.5f)
                {
                    isWall = false;
                    break;
                }
            }

            if (isWall)
                _rigid.sharedMaterial = _noFriction; // 벽에 닿았을 때 마찰력 없음 적용
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
            _rigid.sharedMaterial = _normalFriction; // 바닥에서 떨어졌을 때 일반 마찰력 적용
    }
}