using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private void Start()
    {
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        audioSource.clip = Randomizer.GetRandomFromList(audioClips);

        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            var list = audioClips.ToList();
            list.Remove(audioSource.clip);

            audioSource.Stop();
            audioSource.clip = Randomizer.GetRandomFromList(list);
        }
    }
}
