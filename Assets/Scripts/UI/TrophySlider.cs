using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophySlider : MonoBehaviour
{
    [SerializeField] private Slider trophySlider;
    [SerializeField] private Slider nextArenaSlider;
    [SerializeField] private Image nextArenaImage;

    private void Start()
    {
        trophySlider.interactable = false;
        nextArenaSlider.interactable = false;
        UpdateTrophySlider();
    }

    private void UpdateTrophySlider()
    {
        int currentTrophies = ArenasHolder.CurrentTrophy;
        int currentArenaIndex = ArenasHolder.Instance.CurrentArenaIndex;

        Arena[] arenas = ArenasHolder.Instance.ArenaList;
        int nextArenaIndex = currentArenaIndex + 1;

        if(nextArenaIndex < arenas.Length)
        {
            int currentArenaRequirement = arenas[currentArenaIndex].TrophyRequirement;
            int nextArenaRequirement = arenas[nextArenaIndex].TrophyRequirement;

            int trophiesTowardsNextArena = currentTrophies - currentArenaRequirement;
            int trophiesNeeded = nextArenaRequirement - currentArenaRequirement;

            trophySlider.maxValue = trophiesNeeded;
            trophySlider.value = trophiesTowardsNextArena;

            nextArenaSlider.maxValue = trophiesNeeded;
            nextArenaSlider.value = trophiesTowardsNextArena;

            nextArenaImage.sprite = arenas[nextArenaIndex].arenaImage;
            nextArenaImage.gameObject.SetActive(true);
        }
        else
        {
            trophySlider.maxValue = currentTrophies;
            trophySlider.value = currentTrophies;

            nextArenaSlider.maxValue = currentTrophies;
            nextArenaSlider.value = currentTrophies;

            nextArenaImage.gameObject.SetActive(false);
        }
    }
}
