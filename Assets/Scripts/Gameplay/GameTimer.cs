using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] public int sessionTime { get; private set; }

    private void Start()
    {
        string text = string.Format("{0:D2}:{1:D2}", sessionTime / 60, sessionTime % 60);
        timerText.text = text;
        StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        while (sessionTime > 0)
        {
            yield return new WaitForSeconds(1);

            sessionTime--;
            string text = string.Format("{0:D2}:{1:D2}", sessionTime / 60, sessionTime % 60);
            timerText.text = text;
        }

        EventManager.InvokeGameFinish(GameFinishStatus.draw);
    }
}
