using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public static HeroController instance;

    // the speed of the hero, handle in inspector
    public const float speed = 1.5f;

    // reference to components
    Rigidbody2D rigidbody2d;
    Animator animator;
    Collider2D collider2d;

    // state parameters
    private float lookDirection = 1;
    public float LookDirection {
        get { return lookDirection; }
    }
    private float horizontal;
    private float vertical;
    public Vector2 velocity {
        get { return new Vector2(horizontal, vertical); }
    }
    private bool canMove = true;
    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
    }
    private bool climbed = false;
    public bool Climbed {
        get { return climbed; }
        set { climbed = value; }
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (rigidbody2d == null) throw new System.Exception("Rigidbody2D not found on " + gameObject.name);
        animator = GetComponent<Animator>();
        if (animator == null) throw new System.Exception("Animator not found on " + gameObject.name);
        collider2d = GetComponent<Collider2D>();
        if (collider2d == null) throw new System.Exception("Collider2D not found on " + gameObject.name);
    }

    void Update() {
        GetStatusParameters();
        SetAnimation();
    }

    void FixedUpdate() {
        HeroMove(speed * horizontal * Time.deltaTime, speed * vertical * Time.deltaTime);
    }

    // Get input and set status parameters
    private void GetStatusParameters() {
        if (!canMove) {
            horizontal = 0;
            vertical = 0;
            return;
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (!Mathf.Approximately(horizontal, 0.0f)) {
            lookDirection = horizontal>0?1:-1;
        }
    }

    // Move hero by changing its rigidbody2d.position
    // Called in FixedUpdate to ensure the correct physical behavior
    private void HeroMove(float x, float y) {
        if (!canMove) return;
        Vector2 position = rigidbody2d.position;
        position.x += x;
        position.y += y;
        rigidbody2d.MovePosition(position);
    }

    private void SetAnimation() {
        animator.SetFloat("lookDirection", lookDirection);
        animator.SetFloat("speed", horizontal * horizontal + vertical * vertical);
    }

    /// <summary>
    /// Climb up the climbable object
    /// </summary>
    /// <param name="offsetY">The height that Hero looks climbed</param>
    /// <param name="position">The position of the Climbable</param>
    public void ClimbUp(float offsetY, Vector3 position = default) {
        collider2d.offset += new Vector2(0, -offsetY);
        if (position == default) transform.position += new Vector3(0, offsetY, 0);
        else {
            transform.position = position + new Vector3(0, offsetY, 0);
        }
        climbed = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void ClimbDown(float offsetY) {
        collider2d.offset += new Vector2(0, offsetY);
        transform.position += new Vector3(0, -offsetY, 0);
        climbed = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
    
}
