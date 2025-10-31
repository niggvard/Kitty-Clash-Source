using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPool : MonoBehaviour
{
    public static SoundsPool Instance { get; private set; }
    [field: SerializeField] public Pool[] Pools { get; private set; }
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip[] GetPool(string targetName)
    {
        foreach (var pool in Pools)
        {
            if (pool.Name == targetName)
                return pool.Sounds;
        }
        throw new System.Exception("Wrong pool name");
    }

    public AudioClip GetRandomSoundFromPool(string targetName)
    {
        var pool = GetPool(targetName);
        return Randomizer.GetRandomFromList(pool);
    }

    public void PlaySoundFromPool(string targetName, float volume = 1)
    {
        var clip = GetRandomSoundFromPool(targetName);

        var source = Instantiate(audioSource);
        source.volume = volume;
        source.clip = clip;
        source.Play();
        Destroy(source.gameObject, clip.length);
    }

    [System.Serializable]
    public class Pool
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AudioClip[] Sounds { get; private set; }
    }
}
