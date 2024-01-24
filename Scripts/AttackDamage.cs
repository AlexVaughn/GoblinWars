using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    int damageOnImpact = 0;
    [SerializeField]
    float attackSpeed = 0;
    [SerializeField]
    int areaDamage = 0;
    [SerializeField]
    float damageRadius = 0;
    [SerializeField]
    int damagePerTick = 0;
    [SerializeField]
    float damageInterval = 0;
    [SerializeField]
    float timeDamageTicks = 0;
    float currentDamageTick = 0;
    float attackCooldown = 0;

    void Update() {
        if (attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
        }
    }

    public bool AttackReady() {
        if (attackCooldown <= 0) { return true; }
        else { return false; }
    }

    public void ResetCoolDown() {
        attackCooldown = 1f / attackSpeed;
    }

    public void DealDamage(Targetable target) {
        if (target != null && attackType == AttackType.Single) {
            DealSingleDamage(target);
        }
        else if (target != null && attackType == AttackType.Area) {
            DealSingleDamage(target);
            DealAreaDamage(target);
        }
        else if (target != null && attackType == AttackType.OverTime) {
            DealSingleDamage(target);
            StartCoroutine(DealOverTimeDamage(target));
        }
        else if (target != null && attackType == AttackType.All) {
            DealSingleDamage(target);
            DealAreaDamage(target);
            StartCoroutine(DealOverTimeDamage(target));
        }
    }

    void DealSingleDamage(Targetable target) {
        target.TakeDamage(damageOnImpact);
    }

    void DealAreaDamage(Targetable target) {
        Collider[] colliders = Physics.OverlapSphere(target.transform.position, damageRadius);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<Goblin>(out var goblin)) {
                goblin.targetable.TakeDamage(areaDamage);
            }
        }
    }

    IEnumerator DealOverTimeDamage(Targetable target) {
        Collider[] colliders = Physics.OverlapSphere(target.transform.position, damageRadius);
        while (target != null && currentDamageTick > 0) {
            foreach (Collider collider in colliders) {
                if (collider != null && collider.TryGetComponent<Goblin>(out var goblin)) {
                    goblin.targetable.TakeDamage(damagePerTick);
                }
            }
            yield return new WaitForSeconds(damageInterval);
            currentDamageTick -= 1;
        }
        currentDamageTick = timeDamageTicks;
    }
}


enum AttackType
{
    Single,
    Area,
    OverTime,
    All,
}