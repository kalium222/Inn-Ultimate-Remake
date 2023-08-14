using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeleeAttack : MonoBehaviour
{
    public enum MeleeAttackKind {
        slash, // sword
        blow // axe
    }

    public MeleeAttackKind kind;
    public bool isSilver;

    void Start() {
        Debug.Log("kind: " + this.kind + ", isSilver: " + this.isSilver);
    }

    public MeleeAttack(MeleeAttackKind kind = MeleeAttackKind.slash, bool isSilver = false) {
        this.kind = kind;
        this.isSilver = isSilver;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ExecuteEvents.Execute<IAttackableHandler>(
            target: other.gameObject,
            eventData: null,
            functor: (reciever, eventData) => reciever.OnAttack(this)
        );
    }

}

// interface for attackable object
public interface IAttackableHandler : IEventSystemHandler {
    void OnAttack(in MeleeAttack meleeAttack);
}
