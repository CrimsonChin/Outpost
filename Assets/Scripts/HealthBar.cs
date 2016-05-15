using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer healthDisplay;
    public Sprite[] healthSprites;

    public void SetHealth(float health)
    {
        int spriteIndex = (int)(Mathf.Floor((healthSprites.Length - 1) * health));
        healthDisplay.sprite = healthSprites[spriteIndex];
    }
}
