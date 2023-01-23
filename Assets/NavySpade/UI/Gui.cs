using System;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Level;
using NavySpade._PJ71.UI;
using NavySpade._PJ71.UI.UITween;
using UnityEngine;

public class Gui : MonoBehaviour
{
    public static Gui Instance;

    [Header("Components")] 
    [SerializeField]
    private Canvas canvas;

    [SerializeField] private ResourcePresenter[] _resourcePresenters;
    [SerializeField] private GameObject _fullGO;
    
    [field: SerializeField] public ChooseBuildingView ChooseBuildingView { get; private set; }
    
    [field: SerializeField] public UpgradePanel UpgradePanel { get; private set; }

    [field: SerializeField] public FightButtonUI FightButton { get; private set; }
    
    [field: SerializeField] public UITweenManager UITweener { get; private set; }

    private LevelLogic _levelLogic;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void Init(LevelLogic level)
    {
        _levelLogic = level;
        
        Array.ForEach(_resourcePresenters, (p) => p.Init(level.Player.Inventory));
        level.Player.Inventory.ResourcesCountChanged += CheckFullState;
        
        FightButton.Init(_levelLogic);
        ChooseBuildingView.Init(_levelLogic.Player.Inventory);
        CheckFullState(null);
    }
    
    private void CheckFullState(ItemInfo obj)
    {
        _fullGO.SetActive(_levelLogic.Player.Inventory.HasFreeSpace == false);
    }

    #region getters

    public Canvas Canvas => canvas;

    #endregion
}