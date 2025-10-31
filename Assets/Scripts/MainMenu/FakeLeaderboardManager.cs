using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeLeaderboardManager : MonoBehaviour
{
    public static List<PlayerData> LeaderboardData { get; private set; }

    [Header("Content Settings")]
    [SerializeField] private LeaderboardRow leaderboardRowPrefab;
    [SerializeField] private Transform leaderboardContent;
    [SerializeField] private string playerName;
    [SerializeField] private Sprite[] avatarSprites;
    [SerializeField] private TextMeshProUGUI placementText;

    [SerializeField] private int fakePlayerCount;

    [Header("Trophy Range")]
    private int minTrophies;
    private int maxTrophies;

    private void Start()
    {
        minTrophies = ArenasHolder.CurrentArenaInfo.TrophyRequirement;
        if (ArenasHolder.Instance.CurrentArenaIndex == ArenasHolder.Instance.ArenaList.Length - 1)
        {
            minTrophies = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.ArenaList.Length - 1].TrophyRequirement + 850;
            maxTrophies = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.ArenaList.Length - 1].TrophyRequirement + 1000;
        }
        else
            maxTrophies = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex + 1].TrophyRequirement;

        GenerateLeaderBoard();
    }

    private void GenerateLeaderBoard()
    {
        int playerTrophies = ArenasHolder.CurrentTrophy;
        int playerAvatarIndex = SavesManager.AvatarId;
        LeaderboardData = new List<PlayerData>();

        var player = new PlayerData { Name = playerName, Trophies = playerTrophies, Avatar = avatarSprites[playerAvatarIndex] };
        LeaderboardData.Add(player);

        for (int i = 0; i < fakePlayerCount; i++)
        {
            string fakeName = NameProvider.GetRandomName();
            int fakeTrophies = Random.Range(minTrophies, maxTrophies);
            Sprite fakeAvatar = avatarSprites[Random.Range(0, avatarSprites.Length)];
            LeaderboardData.Add(new PlayerData { Name = fakeName, Trophies = fakeTrophies, Avatar = fakeAvatar });
        }

        LeaderboardData.Sort((a, b) => b.Trophies.CompareTo(a.Trophies));

        for (int i = 0; i < LeaderboardData.Count; i++)
        {
            var row = Instantiate(leaderboardRowPrefab, leaderboardContent).Setup(LeaderboardData[i]);
        }

        placementText.text = placementText.text.Replace("XXX", (LeaderboardData.IndexOf(player) + 1).ToString());
    }
}
public class PlayerData
{
    public string Name;
    public int Trophies;
    public Sprite Avatar;
}
