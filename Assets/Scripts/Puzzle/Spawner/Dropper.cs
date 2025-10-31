using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dropper : MonoBehaviour
{
    [SerializeField] private Transform dropperBody;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AudioSource popSound;
    public static Dropper Instance { get; private set; }
    public static event Action<SpawnBall> BallSpawned;

    [Header("Spawn")]
    [SerializeField] private SpawnBall spawnBall;
    [SerializeField] private Transform spawnPosition;
    private Slider cooldownSlider;

    private Unit currentSpawnUnit;
    private Coroutine coroutine;
    private float time;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        time = 0;
        cooldownSlider = MainCanvas.Instance.spawnCooldownSlider;
        coroutine = StartCoroutine(SpawnCooldown());
    }

    private void Update()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(MainCanvas.Instance.inputSliderPosition.position);
        dropperBody.position = new(inputPosition.x, dropperBody.position.y);

        var sliderPosition = Camera.main.WorldToScreenPoint(dropperBody.position);
        cooldownSlider.transform.position = sliderPosition;
    }

    private IEnumerator SpawnCooldown()
    {
        while (true)
        {
            time += Time.fixedDeltaTime * 1.5f;
            time = Mathf.Clamp(time, 0, 10);

            if(currentSpawnUnit != null)
            {
                cooldownSlider.maxValue = currentSpawnUnit.SpawnCooldown;
                cooldownSlider.value = time;
            }
            else
            {
                cooldownSlider.maxValue = 1;
                cooldownSlider.value = 0;
            }

            if (currentSpawnUnit != null && time >= currentSpawnUnit.SpawnCooldown)
            {
                time -= currentSpawnUnit.SpawnCooldown;
                popSound.Play();
                VibrationManager.Vibrate(0.2f);
                var ball = Instantiate(spawnBall, spawnPosition.position, Quaternion.identity).Setup(currentSpawnUnit);
                BallSpawned?.Invoke(ball);
            }

            timerText.text = time.ToString("F2");
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void SelectUnit(Unit unit)
    {
        currentSpawnUnit = unit;
    }

    public void Deselect()
    {
        currentSpawnUnit = null;
    }
}
