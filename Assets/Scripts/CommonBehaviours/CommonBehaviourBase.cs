using UnityEngine;

public abstract class CommonBehaviourBase {
    protected readonly GameObject gameObject;
    public CommonBehaviourBase(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}

public class Move2DController : CommonBehaviourBase { 
    public Move2DController(GameObject gameObject, float velocityFactor = 1f) : base(gameObject) {
        if (!gameObject.TryGetComponent(out m_rigidbody2D))
            throw Utils.LackingPropertyException.NoComponent("Ridigbody2D", gameObject.name);
        m_velocityFactor = velocityFactor;
    }

    /// <summary>
    /// the facing direction enum
    /// </summary>
    protected enum LookDirectionEnum : int {
        Left = -1,
        Right = +1,
    }
    protected LookDirectionEnum m_lookDirection = LookDirectionEnum.Right;
    /// <summary>
    /// float property, set and get the lookdirection enum
    /// </summary>
    public float LookDirection {
        get { return (float)m_lookDirection; }
        set { m_lookDirection = value > 0 ? LookDirectionEnum.Right : LookDirectionEnum.Left ; }
    }
    protected float m_velocityFactor;
    protected Vector2 m_velocity = new();
    public Vector2 Velocity => m_velocity;
    private readonly Rigidbody2D m_rigidbody2D;

    /// <summary>
    /// Move the rigidbody2D, should be called in the fixupdate()
    /// </summary>
    public void Move() {
        m_rigidbody2D.MovePosition(m_rigidbody2D.position+m_velocityFactor*Time.deltaTime*m_velocity);
    }

    /// <summary>
    /// set the lookdirection according to velocity.
    /// should be called in the update()
    /// after the SetVelocitoy()
    /// </summary>
    public void SetLookDirection() {
        if (!Mathf.Approximately(m_velocity.x, 0f))
            m_lookDirection = m_velocity.x > 0 ? LookDirectionEnum.Right : LookDirectionEnum.Left;
    }

    /// <summary>
    /// set the Velocity. should be called in the update()
    /// before the SetLookDirection()
    /// </summary>
    public virtual void SetVelocity() {
        m_velocity.x = 0.1f;
        m_velocity.y = 0.1f;
    }
}
