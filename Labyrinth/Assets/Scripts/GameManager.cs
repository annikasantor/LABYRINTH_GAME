using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State;

    public static float score = 0;

    public static event Action<GameState> OnGameStateChanged;
    
    private void Awake()
    {
        if (instance == null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateGameState(GameState.TitleScreen);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        
        switch(newState)
        {
            case GameState.TitleScreen:
                break;
            case GameState.Level1:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    TitleScreen,
    Level1,
    Win,
    Lose
}
