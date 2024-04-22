using UnityEngine;
using Utils;

/// <summary>
/// HeroController should be responsible for hero's movement,
/// interaction, and attack, etc
/// </summary>
public class HeroController : SingletonMono<HeroController>
{
    private class HeroMoveController : Move2DController {
        public HeroMoveController(GameObject gameObject, float velocityFactor, Control control)
        : base(gameObject, velocityFactor) {
                m_control = control;
        }
        private readonly Control m_control;
        public override void SetVelocity()
        {
            m_velocity = m_control.gameplay.Move.ReadValue<Vector2>();
        }
    }
    private HeroMoveController heroMoveController;
    // Input System c# wrapper
    private Control m_control;
    // the speed factor of the hero, handle in inspector
    public float speedFactor = 1.5f;
    // reference to components
    private Rigidbody2D m_rigidbody2d;
    private Animator m_animator;
    private Collider2D m_collider2d;

    // state parameters
    /// <summary>
    /// the facing direction enum
    /// </summary>
    private enum LookDirectionEnum : int
    {
        Left = -1,
        Right = +1,
    }
    private LookDirectionEnum m_lookDirection = LookDirectionEnum.Right;
    public float LookDirection
    {
        get { return (float)m_lookDirection; }
        set { m_lookDirection = value > 0 ? LookDirectionEnum.Right : LookDirectionEnum.Left ; }
    }
    private Vector2 m_velocity = new();
    public Vector2 Velocity => m_velocity;

    private bool m_canMove = true;
    public bool CanMove {
        get { return m_canMove; }
        set { m_canMove = value; }
    }

    private bool m_climbed = false;
    public bool Climbed {
        get { return m_climbed; }
        set { m_climbed = value; }
    }

    protected override void Awake() {
        base.Awake();
        this.GetAndCheckComponent(out m_rigidbody2d);
        this.GetAndCheckComponent(out m_animator);
        this.GetAndCheckComponent(out m_collider2d);
    }

    private void Start() {
        m_control = GameManager.Instance.Control;
        if ( m_control==null )
            throw new LackingPropertyException("Control is not found on" + GameManager.Instance.gameObject);
        #if SCRIPT_TEST
        m_control.gameplay.Test.performed += OnTest;
        #endif
    }

    #if SCRIPT_TEST
    private void OnTest(InputAction.CallbackContext context)
    {
        Debug.Log("Test!");
    }
    #endif

    private void Update() {
        GetStatusParameters();
        SetAnimation();
    }

    private void FixedUpdate() {
        HeroMove();
    }

    // Get input and set status parameters
    private void GetStatusParameters() {
        if (!m_canMove) {
            return;
        }
        if (!Mathf.Approximately(m_velocity.x, 0.0f)) {
            m_lookDirection = m_velocity.x > 0 ? LookDirectionEnum.Right : LookDirectionEnum.Left;
        }
    }

    // Move hero by changing its rigidbody2d.position
    // Called in FixedUpdate to ensure the correct physical behavior
    private void HeroMove() {
        if (!m_canMove) return;
        m_velocity = m_control.gameplay.Move.ReadValue<Vector2>();
        m_rigidbody2d.MovePosition(m_rigidbody2d.position + speedFactor*Time.deltaTime*m_velocity);
    }

    private void SetAnimation() {
        m_animator.SetFloat("lookDirection", LookDirection);
        m_animator.SetFloat("speed", m_velocity.SqrMagnitude());
    }

    /// <summary>
    /// Climb up the climbable object
    /// </summary>
    /// <param name="offsetY">The height that Hero looks climbed</param>
    /// <param name="position">The position of the Climbable</param>
    public void ClimbUp(float offsetY, Vector3 position = default) {
        m_collider2d.offset += new Vector2(0, -offsetY);
        if (position == default) transform.position += new Vector3(0, offsetY, 0);
        else {
            transform.position = position + new Vector3(0, offsetY, 0);
        }
        m_climbed = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void ClimbDown(float offsetY) {
        m_collider2d.offset += new Vector2(0, offsetY);
        transform.position += new Vector3(0, -offsetY, 0);
        m_climbed = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
    
}
