using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer animationPrefab;
    [SerializeField] private float duration;
    [SerializeField] private Transform startPos, middlePos, finishPos;
    
    private float timeToMid;
    private float timeToFinish;

    private void Start()
    {
        float distanceToMid = Vector3.Distance(startPos.position, middlePos.position);
        float distanceToFinish = Vector3.Distance(middlePos.position, finishPos.position);

        float totalDistance = distanceToMid + distanceToFinish;
        timeToMid = duration * (distanceToMid / totalDistance);
        timeToFinish = duration * (distanceToFinish / totalDistance);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnBall spawnBall;
        if (spawnBall = collision.GetComponent<SpawnBall>())
        {
            StartCoroutine(Animate(spawnBall.Unit));
            VibrationManager.Vibrate(0.2f);
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Animate(Unit unit)
    {
        var cat = Instantiate(animationPrefab, startPos.position, Quaternion.identity);
        cat.sprite = unit.spawnBallImage;

        int positionIndex = Random.Range(0, spawnPositions.Length);
        var position = spawnPositions[positionIndex];

        var direction = Random.Range(0, 2) == 1 ? -1 : 1;
        cat.transform.DORotate(new(0, 0, 360 * direction), Random.Range(0.1f, 0.5f), RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        yield return  cat.transform.DOMove(middlePos.position, timeToMid).SetEase(Ease.Linear).WaitForCompletion();
        yield return cat.transform.DOMove(finishPos.position, timeToFinish).SetEase(Ease.Linear).WaitForCompletion();

        if (cat.gameObject != null)
            Destroy(cat.gameObject);
        Instantiate(unit.unitPrefab, position.position, Quaternion.identity).SetRenderOrder(positionIndex + 5);
    }
}
