using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    GuiManager manager;
    [HideInInspector]
    public Dictionary<ResourceType, int> bank = new Dictionary<ResourceType, int>();
    Dictionary<Product, List<Cost>> store = new Dictionary<Product, List<Cost>>();

    public void Init(GuiManager manager) {
        this.manager = manager;
        InitializeBank();
        InitializeStore();
        UpdateResourcePanel();
        UpdateCostPanels();
    }

    public bool Buy(Product product) {
        if (CanBuy(product)) {
            DoBuy(product);
            return true;
        }
        return false;
    }

    bool CanBuy(Product product) {
        foreach (Cost cost in store[product]) {
            if (bank[cost.type] < cost.amount) { return false; }
        }
        return true;
    }

    void DoBuy(Product product) {
        foreach (Cost cost in store[product]) {
            bank[cost.type] -= cost.amount;
        }
        UpdateResourcePanel();
    }

    public void UpdateResourcePanel() {
        foreach (var entry in bank) {
            if (entry.Key == ResourceType.Wood) { manager.SetWoodCount(entry.Value.ToString()); }
            else if (entry.Key == ResourceType.Stone) { manager.SetStoneCount(entry.Value.ToString()); }
            else if (entry.Key == ResourceType.Villager) { manager.SetVillagerCount(entry.Value.ToString()); }
        }
    }

    void UpdateCostPanels() {
        List<Cost> costs;
        costs = store[Product.ArcherTower1];
        manager.SetArcherCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.BallistaTower1];
        manager.SetBallistaCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.PoisonTower1];
        manager.SetPoisonCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.CannonTower1];
        manager.SetCannonCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.WizardTower1];
        manager.SetWizardCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.LumberCamp1];
        manager.SetLumberCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.StoneCamp1];
        manager.SetStoneCost(costs[0].amount.ToString(), costs[1].amount.ToString());
        costs = store[Product.Fire];
        manager.SetFireCost(costs[0].amount.ToString(), costs[1].amount.ToString());
    }

    void InitializeBank() {
        bank.Add(ResourceType.Wood, 750);
        bank.Add(ResourceType.Stone, 500);
        bank.Add(ResourceType.Villager, 0);
    }

    void InitializeStore() {
        store.Add(Product.ArcherTower1, new List<Cost>{
            new Cost(ResourceType.Wood, 250),
            new Cost(ResourceType.Stone, 50),
        });
        store.Add(Product.BallistaTower1, new List<Cost>{
            new Cost(ResourceType.Wood, 500),
            new Cost(ResourceType.Stone, 200),
        });
        store.Add(Product.PoisonTower1, new List<Cost>{
            new Cost(ResourceType.Wood, 1000),
            new Cost(ResourceType.Stone, 750),
        });
        store.Add(Product.CannonTower1, new List<Cost>{
            new Cost(ResourceType.Wood, 1500),
            new Cost(ResourceType.Stone, 2000),
        });
        store.Add(Product.WizardTower1, new List<Cost>{
            new Cost(ResourceType.Wood, 3500),
            new Cost(ResourceType.Stone, 3500),
        });
        store.Add(Product.LumberCamp1, new List<Cost>{
            new Cost(ResourceType.Wood, 250),
            new Cost(ResourceType.Stone, 250),
        });
        store.Add(Product.StoneCamp1, new List<Cost>{
            new Cost(ResourceType.Wood, 250),
            new Cost(ResourceType.Stone, 250),
        });
        store.Add(Product.Fire, new List<Cost>{
            new Cost(ResourceType.Wood, 400),
            new Cost(ResourceType.Stone, 400),
        });
    }

    public void AddResource(ResourceType resourceType, int count) {
        bank[resourceType] += count;
        UpdateResourcePanel();
    }
}

public class Cost {
    public ResourceType type;
    public int amount;
    public Cost(ResourceType type, int amount) {
        this.type = type;
        this.amount = amount;
    }
}

public enum Product
{
    ArcherTower1,
    BallistaTower1,
    PoisonTower1,
    CannonTower1,
    WizardTower1,
    LumberCamp1,
    StoneCamp1,
    Fire,
}