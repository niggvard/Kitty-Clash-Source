using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public Unit Unit { get; private set; }

    [field: SerializeField] public Rigidbody2D Body { get; private set; }
    [SerializeField] private SpriteRenderer ballImage;
    
    [HideInInspector] public Multiplier multiplier;

    public SpawnBall Setup(Unit unit)
    {
        Unit = unit;

        ballImage.sprite = unit.spawnBallImage;
        return this;
    }
}
