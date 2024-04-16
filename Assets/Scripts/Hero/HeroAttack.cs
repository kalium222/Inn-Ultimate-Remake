using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private HeroController heroController;
    private HeroInteraction heroInteraction;
    private Animator heroAnimator;
    private const string Melee_TAG = "MeleeAttack";
    private const string TRIGGER = "Attack";
    private const string WEAPON_KIND = "WeaponKind";
    // Handler in inspector
    public GameObject MeleeAttack;
    public bool isAttacking = false;
    public float attackRadius = 0f;

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
        if (MeleeAttack == null) {
            throw new System.Exception("MeleeAttack is null");
        }
        if (MeleeAttack.GetComponent<MeleeAttack>() == null) {
            throw new System.Exception("MeleeAttack doesn't have MeleeAttack.cs");
        }
    }

    private void OnDestroy() {
        Weapon.OnWeaponAttack -= OnWeaponAttack;
    }

    private void OnWeaponAttack(Weapon.weaponsKind kind, bool isSilver) {
        StartCoroutine(MeleeAttackCoroutine(kind, isSilver));
    }

    private IEnumerator MeleeAttackCoroutine(Weapon.weaponsKind kind, bool isSilver) {
        // disable movement
        heroController.CanMove = false;
        heroInteraction.CanInteract = false;
        
        // set animation
        isAttacking = true;
        heroAnimator.SetTrigger(TRIGGER);
        heroAnimator.SetInteger(WEAPON_KIND, (int)kind);
        // wait for a frame
        yield return null;
        // instantiate attack gameobject
        Vector3 targetposition = transform.position;
        targetposition.x += attackRadius*heroController.LookDirection;
        GameObject attack = Instantiate(MeleeAttack, targetposition, Quaternion.identity);
        attack.GetComponent<MeleeAttack>().isSilver = isSilver;
        attack.GetComponent<MeleeAttack>().kind = (MeleeAttack.MeleeAttackKind)kind;
        while (heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag(Melee_TAG)) {
            yield return null;
        }
        isAttacking = false;
        Destroy(attack);
        

        // enable movement
        heroController.CanMove = true;
        heroInteraction.CanInteract = true;
    }

}
