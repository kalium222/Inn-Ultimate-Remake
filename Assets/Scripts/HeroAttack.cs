using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private HeroController heroController;
    private HeroInteraction heroInteraction;
    private Animator heroAnimator;
    // Handler in inspector
    public GameObject MeleeAttack;
    public bool isAttacking = false;

    private void Awake() {
        heroController = GetComponent<HeroController>();
        heroInteraction = GetComponent<HeroInteraction>();
        heroAnimator = GetComponent<Animator>();
        if (heroController == null) {
            throw new System.Exception("heroController is null");
        }
        if (heroInteraction == null) {
            throw new System.Exception("heroInteraction is null");
        }
        if (heroAnimator == null) {
            throw new System.Exception("heroAnimator is null");
        }
        if (MeleeAttack == null) {
            throw new System.Exception("MeleeAttack is null");
        }
    }

    
    private void Start() {
        Weapon.OnWeaponAttack += OnWeaponAttack;
    }

    private void OnDestroy() {
        Weapon.OnWeaponAttack -= OnWeaponAttack;
    }

    private void OnWeaponAttack(Weapon.weaponsKind kind, bool isSilver) {
        Debug.Log("OnWeaponAttack");
        StartCoroutine(MeleeAttackCoroutine(kind, isSilver));
    }

    private IEnumerator MeleeAttackCoroutine(Weapon.weaponsKind kind, bool isSilver) {
        // wait a frame?
        // yield return null;

        // disable movement
        heroController.CanMove = false;
        heroInteraction.CanInteract = false;
        
        // set animation
        isAttacking = true;
        heroAnimator.SetTrigger("Attack");
        heroAnimator.SetInteger("WeaponKind", (int)kind);
        // wait for a frame
        yield return null;
        while (heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack")) {
            yield return null;
        }
        isAttacking = false;
        

        // enable movement
        heroController.CanMove = true;
        heroInteraction.CanInteract = true;
    }

}
