using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Placeable))]
[RequireComponent(typeof(Building))]
[RequireComponent(typeof(AttackDamage))]
[RequireComponent(typeof(Targetable))]
public class Tower : MonoBehaviour
{
    WaveManager waveManager;
    Goblin goblin;
    AttackDamage attackDamage;
    [SerializeField]
    float radius;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    Vector3 projectileSpawnOffset;
    Vector3 projectileSpawnPoint;

    void Start() {
        waveManager = FindFirstObjectByType<WaveManager>();
        attackDamage = GetComponent<AttackDamage>();
    }

    void Update() {
        HandleWave();
    }

    void HandleWave() {
        if (waveManager.waveActive) {
            // Remove goblin if it is out of range or the goblin is dead
            if (goblin != null
            && (Vector3.Distance(goblin.transform.position, transform.position) > radius
                || goblin.GetComponent<Targetable>().isDead))
            {
                goblin = null;
            }
            // Try to get a new goblin
            if (goblin == null) { AquireGoblin(); }
            // Deal damage if this has a goblin
            if (goblin != null && attackDamage.AttackReady() && goblin.GetComponent<Targetable>().targetable) {
                attackDamage.ResetCoolDown();
                Vector3 faceDirection = goblin.transform.position - transform.position;
                GameObject proj = Instantiate(projectilePrefab,
                    transform.position + projectileSpawnOffset,
                    Quaternion.LookRotation(faceDirection),
                    transform.parent
                );
                proj.transform.rotation *= Quaternion.Euler(0, 90, 0);
                proj.GetComponent<Projectile>().MoveToPosition(
                    goblin.transform.position, () => attackDamage.DealDamage(goblin.targetable));
            }
        }
    }

    void AquireGoblin() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<Goblin>(out var goblin)) {
                this.goblin = goblin;
                return;
            }
        }
    }

}
