using UnityEngine;
using Utils;
using UnityEngine.InputSystem;

/// <summary>
/// HeroController should be responsible for hero's movement,
/// interaction, and attack, etc
/// </summary>
public class HeroController : SingletonMono<HeroController>
{
    // Move Controller
    private Move2DController m_heroMoveController;
    // the speed factor of the hero, handle in inspector
    [SerializeField]
    private float m_velocityFactor = 1.5f;
    public float LookDirection => m_heroMoveController.LookDirection;
    public Vector2 Velocity => m_heroMoveController.Velocity;

    // Interact Controller
    private HeroInteractController m_heroInteractController;
    [SerializeField]
    private LayerMask m_interactLayer;
    [SerializeField]
    private Collider2D m_interactCollider;


    [SerializeField]
    private Control control;
    // reference to components
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private Collider2D m_physicsCollider;

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
        m_heroInteractController = new(
            gameObject,
            m_interactLayer,
            m_interactCollider
        );
        this.GetAndCheckComponent(out m_animator);
        this.CheckComponent(m_physicsCollider);
        this.CheckComponent(m_interactCollider);
    }

    private void Start() {
        control = GameManager.Instance.Control;
        control.gameplay.SelectNext.performed += m_heroInteractController.OnNextSelected;
        m_heroInteractController.CheckAllContacted();
    }

    private void Update() {
        GetStatusParameters();
        SetAnimation();
    }

    private void FixedUpdate() {
        m_heroMoveController.Move();
    }

    // Get input and set status parameters
    private void GetStatusParameters() {
        if (!m_canMove) return;
        m_heroMoveController.SetVelocity(control.gameplay.Move.ReadValue<Vector2>());
        m_heroMoveController.SetLookDirection();
    }

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
        m_physicsCollider.offset += new Vector2(0, -offsetY);
        if (position == default) transform.position += new Vector3(0, offsetY, 0);
        else {
            transform.position = position + new Vector3(0, offsetY, 0);
        }
        m_climbed = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void ClimbDown(float offsetY) {
        m_physicsCollider.offset += new Vector2(0, offsetY);
        transform.position += new Vector3(0, -offsetY, 0);
        m_climbed = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
    
}
