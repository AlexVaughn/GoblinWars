using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Pather))]
[RequireComponent(typeof(CollidesWithBuilding))]
[RequireComponent(typeof(CollidesWithLight))]
[RequireComponent(typeof(AttackDamage))]
[RequireComponent(typeof(Targetable))]
public class Goblin : Die
{
    Pather pather;
    CollidesWithBuilding colBuild;
    CollidesWithLight colLight;
    Animator animator;
    AttackDamage attackDamage;
    [HideInInspector]
    public Targetable targetable;
    bool isVisible;
    [SerializeField]
    SkinnedMeshRenderer renderGoblin;
    [SerializeField]
    SkinnedMeshRenderer renderClub;
    WaveManager waveManager;
    HealthBar healthBar;

    public void Init(WaveManager waveManager) {
        this.waveManager = waveManager;
    }

    void Start() {
        pather = GetComponent<Pather>();
        colBuild = GetComponent<CollidesWithBuilding>();
        colLight = GetComponent<CollidesWithLight>();
        animator = GetComponent<Animator>();
        attackDamage = GetComponent<AttackDamage>();
        targetable = GetComponent<Targetable>();
        targetable.Init(this);
        SetVisibilty(false);
        animator.Play("Idle01");
    }

    void Update() {
        UpdateVisible();
        UpdateAnimation();
        DealDamage();
    }

    void UpdateVisible() {
        if (!isVisible && colLight.hasCollided) { SetVisibilty(true); }
    }

    void UpdateAnimation() {
        if (targetable.isDead) {
            animator.Play("Die01");
        }
        else if (colBuild.building != null && !colBuild.building.GetComponent<Targetable>().isDead) {
            animator.Play("Attack01");
        }
        else if (pather.allowMove && !pather.locationReached) {
            animator.Play("Move01");
        }
        else {
            animator.Play("Idle01");
        }
    }

    void DealDamage() {
        if (!targetable.isDead) {
            if (!pather.allowMove
            && (colBuild.building == null || colBuild.building.GetComponent<Targetable>().isDead)) {
                pather.ContinueMove();
            }
            else if (colBuild.building != null && !colBuild.building.GetComponent<Targetable>().isDead) {
                if (attackDamage.AttackReady()) {
                    attackDamage.ResetCoolDown();
                    attackDamage.DealDamage(colBuild.building.GetComponent<Targetable>());
                }
            }
        }
    }

    void SetVisibilty(bool enable) {
        isVisible = enable;
        renderGoblin.enabled = enable;
        renderClub.enabled = enable;
        targetable.targetable = enable;
        GetComponent<HealthBar>().SetBarVisibility(enable);
    }

    public override void DoDie() {
        StartCoroutine(OnDeath());
    }

    IEnumerator OnDeath() {
        pather.allowMove = false;
        waveManager.enemiesRemain -= 1;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}