using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int _health = 20;
    
    [SerializeField] TextMeshProUGUI healthText;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            _health--;
            TakeDamage();
            healthText.text = $"Health: {_health}";
        }
    }

    public void TakeDamage()
    {
        if (_health <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

}
