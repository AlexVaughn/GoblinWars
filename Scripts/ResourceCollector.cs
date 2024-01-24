using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pather))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CollidesWithDropOff))]
[RequireComponent(typeof(CollidesWithDeposit))]
public class ResourceCollector : MonoBehaviour
{
    public ResourceType resourceType;
    [SerializeField]
    int resourceCarryMax;
    int resourceCarryAmount;
    public CollectorState state = CollectorState.Idle;
    Transform dropOffTrans;
    ResourceDropOff dropOff;
    Transform depositTrans;
    ResourceDeposit deposit;
    [SerializeField]
    float collectionTime;
    bool shouldAquireNewDropOff = true;


    void Start() {
        GetComponent<CollidesWithDeposit>().onCollide = ReachedDeposit;
        GetComponent<CollidesWithDropOff>().onCollide = ReachedDropOff;
        GoToDeposit();
    }

    void Update() {
        if (state == CollectorState.Idle) {
            resourceCarryAmount = 0;
            GoToDeposit();
        }
    }

    public void SetCollectType(ResourceType newType) {
        GetComponent<Pather>().CancelMove();
        resourceType = newType;
        depositTrans = null;
        deposit = null;
        dropOffTrans = null;
        dropOff = null;
        GoToDeposit();
    }

    void GoToDeposit() {
        if (depositTrans == null) { AquireDeposit(); }
        if (depositTrans != null) {
            GetComponent<Pather>().StartMove(depositTrans.transform.position);
            state = CollectorState.GoingToDeposit;
        }
        else if (!(state == CollectorState.NoWork)) { state = CollectorState.Idle; }
    }

    void GoToDropOff() {
        if (dropOffTrans == null || shouldAquireNewDropOff) { AquireDropOff(); }
        if (dropOffTrans != null) {
            GetComponent<Pather>().StartMove(dropOffTrans.transform.position);
            state = CollectorState.DroppingOff;
        }
        else if (!(state == CollectorState.NoWork)) { state = CollectorState.Idle; }
    }

    void ReachedDeposit() {
        state = CollectorState.Collecting;
        StartCoroutine(Collect());
    }

    void ReachedDropOff() {
        if (dropOff == null || shouldAquireNewDropOff) { AquireDropOff(); }
        if (dropOff != null) {
            dropOff.Receive(resourceType, resourceCarryAmount);
            resourceCarryAmount = 0;
            GoToDeposit();
        }
        else if (!(state == CollectorState.NoWork)) { state = CollectorState.Idle; }
    }

    IEnumerator Collect() {
        yield return new WaitForSeconds(collectionTime);
        if (deposit == null) { AquireDeposit(); }
        if (deposit != null) {
            resourceCarryAmount = deposit.GetTake(resourceCarryMax);
            GoToDropOff();
        }
        else if (!(state == CollectorState.NoWork)) { state = CollectorState.Idle; }
    }

    void AquireDeposit() {
        shouldAquireNewDropOff = true;
        float searchRadius = 0f;
        while (depositTrans == null && searchRadius <= 300f) {
            searchRadius += 3f;
            FindDeposit(searchRadius);
            if (depositTrans != null) { return; }
        }
        state = CollectorState.NoWork;
    }

    void FindDeposit(float searchRadius) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<ResourceDeposit>(out var deposit)) {
                if (deposit.type == resourceType) {
                    this.deposit = deposit;
                    this.depositTrans = deposit.transform;
                    return;
                }
            }
        }
    }

    void AquireDropOff() {
        float searchRadius = 0f;
        while (dropOffTrans == null && searchRadius <= 300f) {
            searchRadius += 3f;
            FindDropOff(searchRadius);
            if (dropOffTrans != null) { return; }
        }
        state = CollectorState.NoWork;
    }

    void FindDropOff(float searchRadius) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in colliders) {
            foreach (ResourceDropOff dropOff in collider.GetComponents<ResourceDropOff>()) {
                if (dropOff.resourceType == resourceType) {
                    this.dropOff = dropOff;
                    this.dropOffTrans = dropOff.transform;
                    return;
                }
            }
        }
    }
}


public enum CollectorState {
    Collecting,
    DroppingOff,
    GoingToDeposit,
    Idle,
    NoWork,
}