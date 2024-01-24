using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class Targetable : MonoBehaviour
{
    [SerializeField]
    int health;
    int maxHealth;
    [HideInInspector]
    public bool isDead = false;
    Die die;
    public bool targetable = true;

    void Start() {
        maxHealth = health;
        GetComponent<HealthBar>().SetBar(health, maxHealth);
    }

    public void Init(Die die) {
        this.die = die;
    }

    public void TakeDamage(int damage) {
        if (!isDead && targetable) {
            health -= damage;
            GetComponent<HealthBar>().SetBar(health, maxHealth);
            if (health <= 0) {
                health = 0;
                isDead = true;
                die.DoDie();
            }
        }
    }

    public void ResetHealth() {
        health = maxHealth;
        GetComponent<HealthBar>().SetBar(health, maxHealth);
    }
}


public abstract class Die : MonoBehaviour
{
    public abstract void DoDie();
}
