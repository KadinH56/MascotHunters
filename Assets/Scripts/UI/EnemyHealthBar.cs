using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Image bar;
    private int maxHealth;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private void Start()
    {
        bar = GetComponent<Image>();
    }

    public void UpdateHealthbar(int health)
    {
        bar.fillAmount = (float)health / maxHealth;
    }
}
