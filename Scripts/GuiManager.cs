using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiManager : MonoBehaviour
{
    [SerializeField]
    Controller controller;
    [SerializeField]
    GameObject towerPanel;
    [SerializeField]
    GameObject dropOffPanel;
    [SerializeField]
    GameObject lightPanel;
    [SerializeField]
    Button startWaveButton;
    [SerializeField]
    Button buildButton;
    [SerializeField]
    GameObject cancelBuild;
    [SerializeField]
    Button archerTowerButton;
    [SerializeField]
    Button ballistaTowerButton;
    [SerializeField]
    Button poisonTowerButton;
    [SerializeField]
    Button cannonTowerButton;
    [SerializeField]
    Button wizardTowerButton;
    [SerializeField]
    Button lumberCampButton;
    [SerializeField]
    Button stoneCampButton;
    [SerializeField]
    Button fireButton;
    [SerializeField]
    TextMeshProUGUI woodCount;
    [SerializeField]
    TextMeshProUGUI stoneCount;
    [SerializeField]
    TextMeshProUGUI villagerCount;
    [SerializeField]
    GameObject ghostBuildPrefab;
    [SerializeField]
    GameObject archerTowerPrefab;
    [SerializeField]
    GameObject ballistaTowerPrefab;
    [SerializeField]
    GameObject poisonTowerPrefab;
    [SerializeField]
    GameObject cannonTowerPrefab;
    [SerializeField]
    GameObject wizardTowerPrefab;
    [SerializeField]
    GameObject lumberCampPrefab;
    [SerializeField]
    GameObject stoneCampPrefab;
    [SerializeField]
    GameObject firePrefab;
    [SerializeField]
    GameObject mouseMessagePrefab;
    GameObject ghostObj;
    [SerializeField]
    TextMeshProUGUI archerWoodCost;
    [SerializeField]
    TextMeshProUGUI archerStoneCost;
    [SerializeField]
    TextMeshProUGUI ballistaWoodCost;
    [SerializeField]
    TextMeshProUGUI ballistaStoneCost;
    [SerializeField]
    TextMeshProUGUI poisonWoodCost;
    [SerializeField]
    TextMeshProUGUI poisonStoneCost;
    [SerializeField]
    TextMeshProUGUI cannonWoodCost;
    [SerializeField]
    TextMeshProUGUI cannonStoneCost;
    [SerializeField]
    TextMeshProUGUI wizardWoodCost;
    [SerializeField]
    TextMeshProUGUI wizardStoneCost;
    [SerializeField]
    TextMeshProUGUI lumberWoodCost;
    [SerializeField]
    TextMeshProUGUI lumberStoneCost;
    [SerializeField]
    TextMeshProUGUI stoneWoodCost;
    [SerializeField]
    TextMeshProUGUI stoneStoneCost;
    [SerializeField]
    TextMeshProUGUI fireWoodCost;
    [SerializeField]
    TextMeshProUGUI fireStoneCost;
    [SerializeField]
    TextMeshProUGUI woodRewardText;
    [SerializeField]
    TextMeshProUGUI stoneRewardText;
    [SerializeField]
    TextMeshProUGUI villagerRewardText;
    [SerializeField]
    TextMeshProUGUI waveNumber;
    [SerializeField]
    GameObject waveCompletePanel;
    [SerializeField]
    Button dismissWaveComplete;
    [SerializeField]
    GameObject defeatPanel;
    [SerializeField]
    GameObject resourcePanel;
    [SerializeField]
    GameObject buttonsPanel;
    [SerializeField]
    Button exitGame;

    void Start() {
        controller = FindAnyObjectByType<Controller>();
        startWaveButton.onClick.AddListener(StartWaveClick);
        buildButton.onClick.AddListener(BuildClick);
        cancelBuild.GetComponent<Button>().onClick.AddListener(CancelBuildClick);
        archerTowerButton.onClick.AddListener(ArcherClick);
        ballistaTowerButton.onClick.AddListener(BallistaClick);
        poisonTowerButton.onClick.AddListener(PoisonClick);
        cannonTowerButton.onClick.AddListener(CannonClick);
        wizardTowerButton.onClick.AddListener(WizardClick);
        lumberCampButton.onClick.AddListener(LumberClick);
        stoneCampButton.onClick.AddListener(StoneClick);
        fireButton.onClick.AddListener(FireClick);
        dismissWaveComplete.onClick.AddListener(DismissWaveComplete);
        exitGame.onClick.AddListener(ExitGame);
    }

    void StartWaveClick() {
        if (controller.waveManager.waveActive) {
            CreateMouseMessage("Wave is already active!");
        }
        else {
            controller.waveManager.StartWave();
            SetCancelBuildVisibility(false);
            SetBuildVisibility(false);
            if (ghostObj != null) { Destroy(ghostObj); }
        }
    }

    void BuildClick() {
        if (controller.waveManager.waveActive) {
            CreateMouseMessage("Cannot build during wave!");
            return;
        }
        SetCancelBuildVisibility(false);
        if (!towerPanel.activeSelf) { SetBuildVisibility(true); }
        else { SetBuildVisibility(false); }
        if (ghostObj != null) { Destroy(ghostObj); }
    }

    void CancelBuildClick() {
        Destroy(ghostObj);
        SetCancelBuildVisibility(false);
    }

    public void SetCancelBuildVisibility(bool enable) {
        cancelBuild.SetActive(enable);
    }

    void SetBuildVisibility(bool enable) {
        towerPanel.SetActive(enable);
        dropOffPanel.SetActive(enable);
        lightPanel.SetActive(enable);
    }

    void SetWaveCompleteVisibility(bool enable) {
        waveCompletePanel.SetActive(enable);
    }

    void SetDefeatVisibility(bool enable) {
        defeatPanel.SetActive(enable);
    }

    void ArcherClick() {
        CreateGhostBuild(archerTowerPrefab, Product.ArcherTower1);
    }

    void BallistaClick() {
        CreateGhostBuild(ballistaTowerPrefab, Product.BallistaTower1);
    }

    void PoisonClick() {
        CreateGhostBuild(poisonTowerPrefab, Product.PoisonTower1);
    }

    void CannonClick() {
        CreateGhostBuild(cannonTowerPrefab, Product.CannonTower1);
    }

    void WizardClick() {
        CreateGhostBuild(wizardTowerPrefab, Product.WizardTower1);
    }

    void LumberClick() {
        CreateGhostBuild(lumberCampPrefab, Product.LumberCamp1);
    }

    void StoneClick() {
        CreateGhostBuild(stoneCampPrefab, Product.StoneCamp1);
    }

    void FireClick() {
        CreateGhostBuild(firePrefab, Product.Fire);
    }

    void DismissWaveComplete() {
        SetWaveCompleteVisibility(false);
    }

    void ExitGame() {
        Application.Quit();
    }

    public void SetWoodCount(string count) {
        woodCount.text = count;
    }

    public void SetStoneCount(string count) {
        stoneCount.text = count;
    }

    public void SetVillagerCount(string count) {
        villagerCount.text = count;
    }

    public void SetArcherCost(string wood, string stone) {
        archerWoodCost.text = wood;
        archerStoneCost.text = stone;
    }

    public void SetBallistaCost(string wood, string stone) {
        ballistaWoodCost.text = wood;
        ballistaStoneCost.text = stone;
    }

    public void SetPoisonCost(string wood, string stone) {
        poisonWoodCost.text = wood;
        poisonStoneCost.text = stone;
    }

    public void SetCannonCost(string wood, string stone) {
        cannonWoodCost.text = wood;
        cannonStoneCost.text = stone;
    }

    public void SetWizardCost(string wood, string stone) {
        wizardWoodCost.text = wood;
        wizardStoneCost.text = stone;
    }

    public void SetLumberCost(string wood, string stone) {
        lumberWoodCost.text = wood;
        lumberStoneCost.text = stone;
    }

    public void SetStoneCost(string wood, string stone) {
        stoneWoodCost.text = wood;
        stoneStoneCost.text = stone;
    }

    public void SetFireCost(string wood, string stone) {
        fireWoodCost.text = wood;
        fireStoneCost.text = stone;
    }

    void CreateGhostBuild(GameObject buildingPrefab, Product product) {
        SetBuildVisibility(false);
        SetCancelBuildVisibility(true);
        ghostObj = Instantiate(ghostBuildPrefab);
        ghostObj.GetComponent<GhostBuild>().SetGhost(buildingPrefab, product, this, controller);
    }

    public void CreateMouseMessage(string text) {
        GameObject mouseMessage = Instantiate(mouseMessagePrefab, transform);
        mouseMessage.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetWaveNumber(string waveNumber) {
        this.waveNumber.text = waveNumber;
    }

    public void WaveComplete(string waveNumber, string woodReward, string stoneReward, string villagerReward) {
        this.waveNumber.text = waveNumber;
        woodRewardText.text = woodReward;
        stoneRewardText.text = stoneReward;
        villagerRewardText.text = villagerReward;
        SetWaveCompleteVisibility(true);
    }

    public void Defeat() {
        CancelBuildClick();
        SetBuildVisibility(false);
        SetWaveCompleteVisibility(false);
        resourcePanel.SetActive(false);
        buttonsPanel.SetActive(false);
        SetDefeatVisibility(true);
    }
}