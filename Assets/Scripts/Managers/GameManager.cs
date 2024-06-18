using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Player Gameplay Variables
    private int _coins;
    private int _health;
    private int _lives;
    private const int MAX_HEALTH = 5;
    private const int MAX_LIVES = 3;

    public int coins
    {
        get => _coins;
        set
        {
            _coins += value;
            onCoinValueChange?.Invoke(_coins);
            Debug.Log("Current coins: " + _coins);
        }
    }
    public int health
    {
        get => _health;
        set
        {
            if (value <= 0)
            {
                lives--;
                return;
            }
            else if (value < health) PlayerInstance.hit();
            else if (value > MAX_HEALTH) value = MAX_HEALTH;

            _health = value;
            onHealthValueChange?.Invoke(_health);
            Debug.Log("Current lives: " + _health);
        }
    }
    public int lives
    {
        get => _lives;
        set
        {
            if (value <= 0)
            {
                GameOver();
                return;
            }
            if (value < _lives)
            {
                Respawn();
            }
            else if (value > MAX_LIVES) value = MAX_LIVES;

            _lives = value;
        }
    }


    //public UnityEvent<int> onLifeValueChange;

    public Action<int> onHealthValueChange;
    public Action<int> onCoinValueChange;

    static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private PlayerController playerPrefab;

    [HideInInspector] public PlayerController PlayerInstance => _playerInstance;
    PlayerController _playerInstance = null;
    Transform currentCheckpoint;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
    private void Start()
    {
        _health = MAX_HEALTH;
        _lives = MAX_LIVES;
        coins = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Game")
                SceneManager.LoadScene(2);
            else if (SceneManager.GetActiveScene().name == "GameOver")
                SceneManager.LoadScene(0);
            else
            {
                _health = MAX_LIVES;
                coins = 0;
                Debug.Log("Lives and coins reset");
                SceneManager.LoadScene(1);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        if (PlayerInstance)
            UnPause();
        _health = MAX_LIVES;
        coins = 0;
        Debug.Log("Lives and coins reset");
        SceneManager.LoadScene(sceneName);
    }

    private void GameOver()
    {
        Debug.Log("You died");
        SceneManager.LoadScene(2);
    }

    private void Respawn()
    {
        _health = MAX_HEALTH;
        _playerInstance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        _playerInstance.transform.position = currentCheckpoint.position;
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        _playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }

    public void Pause()
    {
        PlayerInstance.paused = true;
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        PlayerInstance.paused = false;
        Time.timeScale = 1.0f;
    }
}
