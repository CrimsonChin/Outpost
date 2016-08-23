using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer HealthDisplay;
    public Sprite[] HealthSprites;

    public void SetHealth(float health)
    {
        int spriteIndex = (int)(Mathf.Floor((HealthSprites.Length - 1) * health));
        HealthDisplay.sprite = HealthSprites[spriteIndex];
    }
}
