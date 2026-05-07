using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int _health = 20;
    
    [SerializeField] TextMeshProUGUI healthText;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _playerTakeDamage;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            _health--;
            TakeDamage();
            healthText.text = $"Health: {_health}";
            _audioSource.PlayOneShot(_playerTakeDamage);
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
