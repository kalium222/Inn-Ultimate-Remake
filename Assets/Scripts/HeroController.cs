using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    
    public static HeroController instance;
    public const float speed = 1.5f;

    Rigidbody2D rigidbody2d;
    Animator animator;
    public float lookDirection = 1;
    float horizontal;
    float vertical;
    private bool canMove = true;
    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
    }
    public Vector2 velocity {
        get { return new Vector2(horizontal, vertical); }
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        getStatusParameters();
        setAnimation();
    }

    void FixedUpdate() {
        HeroMove();
    }

    // Get input and set status parameters
    private void getStatusParameters() {
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
    private void HeroMove() {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    private void setAnimation() {
        animator.SetFloat("lookDirection", lookDirection);
        animator.SetFloat("speed", horizontal * horizontal + vertical * vertical);
    }
    
}
