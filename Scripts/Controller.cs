using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GuiManager guiManager;
    [HideInInspector]
    public WorldGrid worldGrid;
    [HideInInspector]
    public Bank bank;
    [HideInInspector]
    public MapGenerator mapGenerator;
    [HideInInspector]
    public WaveManager waveManager;
    [HideInInspector]
    public List<GameObject> buildings = new();


    void Start() {
        bank = GetComponent<Bank>();
        bank.Init(guiManager);
        worldGrid = GetComponent<WorldGrid>();
        worldGrid.Init();
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.Init(worldGrid, this);
        waveManager = GetComponent<WaveManager>();
        waveManager.Init(this);
    }
}
