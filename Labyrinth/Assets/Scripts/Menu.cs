using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Menu : MonoBehaviour
{
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonPress;
    
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPlayButton()
    {
        _audioSource.PlayOneShot(_buttonPress);
        SceneManager.LoadScene(1);
    }

    public void OnRestartButton()
    {
        _audioSource.PlayOneShot(_buttonPress);
        SceneManager.LoadScene(0);
    }

    public void OnQuitButton()
    {
        _audioSource.PlayOneShot(_buttonPress);
        Application.Quit();
    }
}
