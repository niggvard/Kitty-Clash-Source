using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [field: SerializeField] public ChestSettings chestAsset { get; private set; }
    public int ChestNumber { get; private set; }
    public bool IsOpening => startTime != DateTime.MinValue;
    public int SecondsLeft => chestAsset.SecondsToOpen - (int)((TimeSpan)(DateTime.Now - startTime)).TotalSeconds;

    [SerializeField] private Image chestImage;
    [SerializeField] private TextMeshProUGUI timerText;
    private DateTime startTime = DateTime.MinValue;

    public static event Action<Chest> ChestStartOpening;
    public static event Action<ChestSettings> ChestOpen;

    private void Start()
    {
        timerText.text = "";
        timerText.gameObject.SetActive(false);
    }

    public Chest Setup(ChestSettings chest, int number)
    {
        chestAsset = chest;
        ChestNumber = number;

        chestImage.sprite = chest.Image;

        return this;
    }

    private void Update()
    {
        if (IsOpening)
            UpdeteTimerDisplay();
    }

    private void UpdeteTimerDisplay()
    {
        if(SecondsLeft > 0)
        {
            TimeSpan timeLeft = TimeSpan.FromSeconds(SecondsLeft);
            timerText.text = $"{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
            timerText.gameObject.SetActive(true);
        }
        else
        {
            timerText.text = "OPEN!";
            timerText.color = Color.green; 
            timerText.gameObject.SetActive(true);
        }
    }

    public string GetSaveData()
    {
        SaveData data = new();
        data.openStartTime = startTime.ToString();
        data.assetName = chestAsset.name;

        return JsonUtility.ToJson(data);
    }

    public Chest LoadSaveData(string data)
    {
        var save = JsonUtility.FromJson<SaveData>(data);
        chestAsset = Resources.Load<ChestSettings>("Chests/" + save.assetName);
        startTime = DateTime.Parse(save.openStartTime);

        return this;
    }

    public void StartOpening()
    {
        if (startTime != DateTime.MinValue) return;

        startTime = DateTime.Now;
        ChestStartOpening?.Invoke(this);

        DateTime notificationTime = ((DateTime)startTime).AddSeconds(chestAsset.SecondsToOpen + 1);
        Notification notification = new(
            "chest" + DateTime.Now.ToString(),
            "Chest is open!",
            $"Your {chestAsset.chestName} is ready to open!",
            "chest",
            "chestOpen",
            notificationTime
        );
        NotificationsManager.SetupNotification(notification);
    }

    public void Open()
    {
        TimeSpan time = (TimeSpan)(DateTime.Now - startTime);
        if (startTime == DateTime.MinValue || time.TotalSeconds < chestAsset.SecondsToOpen)
        {
            ChestInfoPanel.Instance.ShowInfo(this);
            return;
        }

        timerText.text = "OPEN!";
        timerText.color = Color.green;
        timerText.gameObject.SetActive(true);

        PlayerPrefs.SetString("Chest" + ChestNumber, "");

        int money = UnityEngine.Random.Range(chestAsset.MinMoneyDrop, chestAsset.MaxMoneyDrop);
        SavesManager.Money += money;
        ChestDropScreen.Instance.AddMoneyDrop(money);

        for (int i = 0; i < chestAsset.DropNumber; i++)
        {
            var rarity = Randomizer.GetItemByChance(chestAsset.chances).Rarity;
            var rewardList = ArenasHolder.Instance.GetAvaliableUnitsOfRarity(rarity);
            var reward = Randomizer.GetRandomFromList(rewardList);
            if (reward.CurrentLevel == 0)
                reward.CurrentLevel++;

            var cardsNumber = UnityEngine.Random.Range(chestAsset.MinDrop, chestAsset.MaxDrop);
            reward.CurrentCardsOwned += cardsNumber;
            ChestDropScreen.Instance.AddDrop(reward.CardImage, cardsNumber);
        }
        ChestDropScreen.Instance.Show();

        ChestOpen.Invoke(chestAsset);
        Destroy(gameObject);
    }

    public static void ForceOpen(ChestSettings chestAsset)
    {
        int money = UnityEngine.Random.Range(chestAsset.MinMoneyDrop, chestAsset.MaxMoneyDrop);
        SavesManager.Money += money;
        ChestDropScreen.Instance.AddMoneyDrop(money);

        for (int i = 0; i < chestAsset.DropNumber; i++)
        {
            var rarity = Randomizer.GetItemByChance(chestAsset.chances).Rarity;
            var rewardList = ArenasHolder.Instance.GetAvaliableUnitsOfRarity(rarity);
            var reward = Randomizer.GetRandomFromList(rewardList);
            if (reward.CurrentLevel == 0)
                reward.CurrentLevel++;

            var cardsNumber = UnityEngine.Random.Range(chestAsset.MinDrop, chestAsset.MaxDrop);
            reward.CurrentCardsOwned += cardsNumber;
            ChestDropScreen.Instance.AddDrop(reward.CardImage, cardsNumber);
        }
        ChestDropScreen.Instance.Show();
    }

    [Serializable]
    public struct SaveData
    {
        public string openStartTime;
        public string assetName;
    }
}
