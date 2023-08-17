using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    // handle in inspector
    // the offset of the hero sprite
    public float offsetY = 0.5f;
    // the time it takes to climb
    public float climbTime = 1f;
    // whether to set the postion of the hero when climbing
    public bool setPosition = true;

    private const float waitTime = 0.5f;

    private Collider2D[] collider2ds;

    private void Awake() {
        // Check if the object has a collider
        collider2ds = GetComponents<Collider2D>();
        if (collider2ds == null) throw new System.Exception("Collider2D not found on " + gameObject.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // if the hero collides with the climbable object
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Hero") {
            StartCoroutine(ClimbCoroutine());
        }
    }
    // if the hero is walking towards the climbable object
    // during the whole climbing process, then the hero
    // will climb up the object
    private IEnumerator ClimbCoroutine() {
        float startTime = Time.time;
        while (Time.time - startTime < climbTime) {
            if (!IsClimbingTowards()) yield break;
            yield return null;
        }
        Climb();
        // disable HeroMove for a while
        HeroController.instance.CanMove = false;
        yield return new WaitForSeconds(waitTime);
        HeroController.instance.CanMove = true;
    }
    // check if the hero is walking towards the climbable object
    private bool IsClimbingTowards() {
        return Vector3.Dot(HeroController.instance.velocity, transform.position - HeroController.instance.transform.position)>0;
    }
    // set the hero's state and sprite
    private void Climb() {
        if (HeroController.instance.Climbed) return;
        // collider2d.isTrigger = true;
        foreach (Collider2D collider2d in collider2ds) {
            collider2d.isTrigger = true;
        }
        if (setPosition) HeroController.instance.ClimbUp(offsetY, transform.position);
        else HeroController.instance.ClimbUp(offsetY);
    }

    // When hero leaves the climbable object
    void OnTriggerExit2D(Collider2D other) {
        if (!HeroController.instance.Climbed) return;
        if (other.gameObject.name == HeroController.instance.gameObject.name) {
            // collider2d.isTrigger = false;
            foreach (Collider2D collider2d in collider2ds) {
                collider2d.isTrigger = false;
            }
            HeroController.instance.ClimbDown(offsetY);
        }
    }

}
