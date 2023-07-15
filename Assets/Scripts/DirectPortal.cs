using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectPortal : Portal
{
    private bool isActive = true;
    public Vector2 EnteringDirection = Vector2.up;
    private void Start() {
        isActive = true;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!isActive) return;
        HeroController controller = other.GetComponent<HeroController>();
        if (controller == HeroController.instance && (Vector2.Dot(controller.velocity, EnteringDirection) > 0.0f))
        {
            // Debug.Log("DirectPortal: " + gameObject.name);
            Teleport(controller.gameObject);
            isActive = false;
        }
    }
}
