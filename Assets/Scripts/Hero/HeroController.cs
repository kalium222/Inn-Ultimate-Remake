using UnityEngine;
using Utils;

/// <summary>
/// HeroController should be responsible for hero's movement,
/// interaction, and attack, etc
/// </summary>
public class HeroController : SingletonMono<HeroController>
{
    // Move Controller
    private class HeroMoveController : Move2DController {
        public HeroMoveController(GameObject gameObject, float velocityFactor)
        : base(gameObject, velocityFactor) {}
        public Control control;
        public void SetControl(Control control) => this.control = control;
        protected override void SetVelocity()
        {
            m_velocity = control.gameplay.Move.ReadValue<Vector2>();
        }
    }
    private HeroMoveController m_heroMoveController;
    // the speed factor of the hero, handle in inspector
    [SerializeField]
    private float m_velocityFactor = 1.5f;

    // reference to components
    private Animator m_animator;
    private Collider2D m_collider2d;

    public float LookDirection => m_heroMoveController.LookDirection;
    public Vector2 Velocity => m_heroMoveController.Velocity;

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
        m_heroMoveController = new(gameObject, m_velocityFactor);
        this.GetAndCheckComponent(out m_animator);
        this.GetAndCheckComponent(out m_collider2d);
    }

    private void Start() {
        m_heroMoveController.SetControl(GameManager.Instance.Control);
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
        m_heroMoveController.Move();
    }

    // Get input and set status parameters
    private void GetStatusParameters() {
        if (!m_canMove) {
            return;
        }
        m_heroMoveController.SetState();
    }

    // Move hero by changing its rigidbody2d.position
    // Called in FixedUpdate to ensure the correct physical behavior
    // private void HeroMove() {
    //     if (!m_canMove) return;
    //     m_velocity = m_control.gameplay.Move.ReadValue<Vector2>();
    //     m_rigidbody2d.MovePosition(m_rigidbody2d.position + speedFactor*Time.deltaTime*m_velocity);
    // }

    private void SetAnimation() {
        m_animator.SetFloat("lookDirection", m_heroMoveController.LookDirection);
        m_animator.SetFloat("speed", m_heroMoveController.Velocity.SqrMagnitude());
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
