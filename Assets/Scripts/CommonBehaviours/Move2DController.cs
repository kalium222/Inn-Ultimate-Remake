using UnityEngine;
using Utils;

public class Move2DController : CommonBehaviourBase { 
    public Move2DController(GameObject gameObject, float velocityFactor = 1f) : base(gameObject) {
        gameObject.GetAndCheckComponent(out m_rigidbody2D);
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

    public void SetLookDirection() {
        if (!Mathf.Approximately(m_velocity.x, 0f))
            m_lookDirection = m_velocity.x > 0 ? LookDirectionEnum.Right : LookDirectionEnum.Left;
    }

    public void SetVelocity(Vector2 velocity) {
        m_velocity = velocity;
    }
}
