using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Controller controller;
    int waveNumber;
    float spawnTimeInterval;
    Vector3 currentSpawnPoint;
    int enemiesThisWave;
    int spawnedThisWave;
    int spawnedAtCurrentPoint;
    [HideInInspector]
    public int enemiesRemain;
    Dictionary<int, int> enemies = new Dictionary<int, int>();
    [SerializeField]
    GameObject enemiesContainer;
    [SerializeField]
    GameObject goblin1Prefab;
    [SerializeField]
    GameObject goblin2Prefab;
    [SerializeField]
    GameObject goblin3Prefab;
    [SerializeField]
    GameObject goblin4Prefab;
    [SerializeField]
    GameObject goblin5Prefab;
    [SerializeField]
    GameObject villagerPrefab;
    ResourceType nextResourceTypeVillager;
    Vector3 palaceLocation;
    Targetable palace;
    [HideInInspector]
    public bool waveActive;
    int woodReward;
    int stoneReward;
    int villagerReward;

    bool gameOver = false;


    public void Init(Controller controller) {
        this.controller = controller;
        palace = controller.mapGenerator.palace.GetComponent<Targetable>();
        palaceLocation = controller.mapGenerator.GetPalaceLocation() - new Vector3(30, 0, 30);
        enemies = new Dictionary<int, int>();
        spawnTimeInterval = 0.25f;
        waveNumber = 1;
        controller.guiManager.SetWaveNumber(waveNumber.ToString());
        SetWave(waveNumber);
    }

    void SetWave(int waveNumber) {
        CalculateWaveNumbers(waveNumber);
        waveActive = false;
        currentSpawnPoint = GetNewSpawnPoint();
        spawnedThisWave = 0;
        spawnedAtCurrentPoint = 0;
        this.waveNumber = waveNumber;
        SetEnemyCountThisWave();
    }

    void CalculateWaveNumbers(int x) {
        int h;

        h = 0;
        if (x >= h) {
            enemies[1] = Mathf.FloorToInt(Mathf.Pow(x - h, 1.8f) + 10);
        }
        else { enemies[1] = 0; }

        h = 5;
        if (x >= h) {
            enemies[2] = Mathf.FloorToInt(0.9f * Mathf.Pow(x - h, 1.6f) + 1);
        }
        else { enemies[2] = 0; }

        h = 10;
        if (x >= h) {
            enemies[3] = Mathf.FloorToInt(0.8f * Mathf.Pow(x - h, 1.4f) + 1);
        }
        else { enemies[3] = 0; }

        h = 15;
        if (x >= h) {
            enemies[4] = Mathf.FloorToInt(0.5f * Mathf.Pow(x - h, 1.2f) + 1);
        }
        else { enemies[4] = 0; }

        h = 20;
        if (x >= h) {
            enemies[5] = Mathf.FloorToInt(0.2f * Mathf.Pow(x - h, 1.1f) + 1);
        }
        else { enemies[5] = 0; }

        woodReward = x * 75;
        stoneReward = x * 50;
        villagerReward = 1;
    }

    void SetEnemyCountThisWave() {
        enemiesThisWave = 0;
        foreach (var keyValue in enemies) {
            enemiesThisWave += keyValue.Value;
        }
        enemiesRemain = enemiesThisWave;
    }

    public void StartWave() {
        StartCoroutine(RunWave());
    }

    void ChanceChangeSpawnPoint() {
        float chance = 0.1f * Mathf.Sqrt(spawnedAtCurrentPoint);
        float randomFloat = Random.Range(0f, 1f);
        if (chance >= randomFloat) {
            currentSpawnPoint = GetNewSpawnPoint();
        }
    }

    GameObject GetGoblinToSpawn() {
        List<int> choices = new();
        foreach (int i in enemies.Keys) {
            if (enemies[i] > 0) { choices.Add(i); }
        }
        int index = Random.Range(0, choices.Count);
        int gobId = choices[index];
        if (gobId == 1) { return goblin1Prefab; }
        if (gobId == 2) { return goblin2Prefab; }
        if (gobId == 3) { return goblin3Prefab; }
        if (gobId == 4) { return goblin4Prefab; }
        if (gobId == 5) { return goblin5Prefab; }
        return null;
    }

    void Spawn() {
        GameObject goblinPrefab = GetGoblinToSpawn();
        if (goblinPrefab == null) { return; }
        Vector3 faceDirection = palaceLocation - currentSpawnPoint;
        GameObject goblin = Instantiate(goblinPrefab,
            currentSpawnPoint,
            Quaternion.LookRotation(faceDirection),
            enemiesContainer.transform
        );
        spawnedThisWave += 1;
        spawnedAtCurrentPoint += 1;
        goblin.GetComponent<Goblin>().Init(this);
        goblin.GetComponent<Pather>().StartMove(palaceLocation);
    }

    Vector3 GetNewSpawnPoint() {
        spawnedAtCurrentPoint = 0;
        int tries = 0;
        int tileCount = controller.worldGrid.TileCount();
        while (tries < tileCount) {
            Tile tile = controller.worldGrid.RandomTile();
            if (tile.HasDarkness() && !tile.IsOccupied()) { return tile.transform.position; }
            tries += 1;
        }
        return new Vector3(200, 0, 200);
    }

    void CheckDefeat() {
        if (palace.isDead) {
            gameOver = true;
        }
    }

    IEnumerator RunWave() {
        waveActive = true;
        // Spawn enemies while there are some to spawn
        while (!gameOver && spawnedThisWave < enemiesThisWave) {
            ChanceChangeSpawnPoint();
            Spawn();
            yield return new WaitForSeconds(spawnTimeInterval);
            CheckDefeat();
        }
        // Wait for all enemies to die or kill the palace
        while (!gameOver && enemiesRemain > 0) {
            yield return null;
            CheckDefeat();
        }
        if (gameOver) {
            controller.guiManager.Defeat();
        }
        else {
            WaveComplete();
        }
    }

    void WaveComplete() {
        foreach (var building in controller.buildings) {
            Targetable targ = building.GetComponent<Targetable>();
            if (targ != null) { targ.ResetHealth(); }
        }
        waveNumber += 1;
        SetWave(waveNumber);
        controller.bank.bank[ResourceType.Wood] += woodReward;
        controller.bank.bank[ResourceType.Stone] += stoneReward;
        controller.bank.bank[ResourceType.Villager] += villagerReward;
        controller.bank.UpdateResourcePanel();
        controller.guiManager.WaveComplete(waveNumber.ToString(), woodReward.ToString(),
            stoneReward.ToString(), villagerReward.ToString());
        SpawnNewVillager();
    }

    void SpawnNewVillager() {
        GameObject villager = Instantiate(villagerPrefab);
        villager.transform.position = transform.position + new Vector3(5f, 0f, 5f);
        villager.GetComponent<ResourceCollector>().SetCollectType(nextResourceTypeVillager);
        if (nextResourceTypeVillager == ResourceType.Wood) {
            nextResourceTypeVillager = ResourceType.Stone;
        }
        else {
            nextResourceTypeVillager = ResourceType.Wood;
        }
    }
}