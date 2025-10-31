using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public static SavesManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TMP_InputField nicknameInputField;

    private static List<Unit> selectedUtins;
    public static List<Unit> SelectedUtins
    {
        get 
        {
            return selectedUtins;
        }
    }
    public static int Money
    {
        get => PlayerPrefs.GetInt("Money", 0);
        set
        {
            PlayerPrefs.SetInt("Money", value);
            UpdateMoney();
        }
    }
    public static int AvatarId
    {
        get => PlayerPrefs.GetInt("SelectedAvatar", 0);
        set => PlayerPrefs.SetInt("SelectedAvatar", value);
    }
    public static string Nickname
    {
        get => PlayerPrefs.GetString("Nickname", "");
        set => PlayerPrefs.SetString("Nickname", value);
    }

    public static float CurrentTimeDivider
    {
        get => PlayerPrefs.GetFloat("CurrentTimeDivider", 1.0f);
        set => PlayerPrefs.SetFloat("CurrentTimeDivider", value);
    }

    private void Awake()
    {
        selectedUtins = new();
        Instance = this;
        UnlockDefaultUnits();
        UpdateMoney();
        LoadNickname();
    }

    private void UnlockDefaultUnits()
    {
        var firstArena = MainMenuManager.Instance.ArenasHolder.ArenaList[0];
        if (!firstArena.UnlockableUnits[0].IsUnlocked)
        {       
            selectedUtins = new();
            for (int i = 0; i < 3; i++)
            {
                firstArena.UnlockableUnits[i].CurrentLevel = 1;
                selectedUtins.Add(firstArena.UnlockableUnits[i]);
            }
            SaveSelectedUnits();
        }
        else
        {
            LoadSelected();
        }
    }

    public void SetSelectedCardsSave(int position, Unit unit)
    {
        selectedUtins[position] = unit;
        SaveSelectedUnits();
    }

    private void SaveSelectedUnits()
    {
        for (int i = 0; i < selectedUtins.Count; i++)
        {
            PlayerPrefs.SetString("Selected" + i, selectedUtins[i].UnitName);
        }
    }

    private void LoadSelected()
    {
        var allUnits = MainMenuManager.Instance.ArenasHolder.AllUnits;
        selectedUtins = new(3);
        for (int i = 0; i < selectedUtins.Capacity; i++)
        {
            var name = PlayerPrefs.GetString("Selected" + i, "empty");
            foreach (var unit in allUnits)
            {
                if (unit.UnitName == name)
                {
                    selectedUtins.Add(unit);
                    break;
                }
            }
        }
    }

    private void LoadNickname()
    {
        var savedName = Nickname;
        if (savedName == "")
        {
            string name = "Player" + Random.Range(100, 999);
            Nickname = name;
            nicknameInputField.text = name;
        }
        else
        {
            nicknameInputField.text = savedName;
        }
    }

    public void OnNicknameChanged()
    {
        if (nicknameInputField.text == "" || nicknameInputField.text.Length > 15)
        {
            nicknameInputField.text = Nickname;
        }
        else
        {
            Nickname = nicknameInputField.text;
        }
    }

    public static void UpdateMoney()
    {
        try
        {
            Instance.moneyText.text = Money.ToString();
        }
        catch { }
    }

    public void UnlockUnit(Unit unit)
    {
        unit.CurrentLevel = 1;
    }

    public void UpdateUnitLevel(Unit unit, int addLevel)
    {
        unit.CurrentLevel += addLevel;
    }

#if UNITY_EDITOR

    [Header("Debug")]
    [SerializeField] private bool clearAllSaves;
    [SerializeField] private bool createRandomChest;

    [SerializeField] private int trophy;
    [SerializeField] private bool setTrophy;

    [SerializeField] private int moneyGive;
    [SerializeField] private bool setMoney;

    [SerializeField] private int tutorialPhase;
    [SerializeField] private bool setTutorialPhase;

    private void OnValidate()
    {
        if (clearAllSaves)
            PlayerPrefs.DeleteAll();

        if (createRandomChest)
            ChestManager.Instance.GiveRandomChest();

        if (setTrophy)
            ArenasHolder.CurrentTrophy = trophy;

        if (setMoney)
            Money = moneyGive;

        if (setTutorialPhase)
            TutorialManager.TutorialPhase = tutorialPhase;
    }

#endif
}