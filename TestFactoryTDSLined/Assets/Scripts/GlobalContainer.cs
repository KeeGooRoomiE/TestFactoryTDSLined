using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GlobalContainer : MonoBehaviour
{
    #region Init

    [Header("Enemy Settings")]
    [Tooltip("Min Enemies Count")]
    public int minEnemies = 5;
    [Tooltip("Max Enemies Count")]
    public int maxEnemies = 10;
    [HideInInspector]
    public int enemiesToDefeat;
    private int enemiesToDefeatMax;

    [Tooltip("Min spawn time(s)")]
    public float minSpawnTime = 1f;
    [Tooltip("Max spawn time(s)")]
    public float maxSpawnTime = 3f;
    [HideInInspector]
    public float spawnTimeout;

    [Tooltip("Min enemy speed")]
    public float minEnemySpeed = 1f;
    [Tooltip("Max enemy speed")]
    public float maxEnemySpeed = 3f;
    [HideInInspector]
    public float enemySpeed;

    [Tooltip("Enemy health")]
    public int enemyHealth = 100;

    [Header("Player Settings")]
    [Tooltip("Player speed")]
    public float playerMoveSpeed = 5f;
    
    [Tooltip("Player radius")]
    public float playerShootRange = 5f;

    [FormerlySerializedAs("playerShootSpeed")] [Tooltip("Shoot speed")]
    public float playerShootSpeed = 1f;

    [Tooltip("Bullet dmg")]
    public int playerBulletDamage = 10;

    [Tooltip("Bullet speed")]
    public float bulletSpeed = 10f;

    [HideInInspector]
    public int playerHealth;
    [HideInInspector]
    public int playerHealthMax = 3;

    [HideInInspector] public bool canMakeFactory;

    private UIController _ui;
    private GameObject tp;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private FactoryController[] factories;

    #endregion
    
    private void Start()
    {
        playerHealth = playerHealthMax;
        GetRandomEnemyCount();
        SwitchSpawn(true);
        _ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        tp = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (enemiesToDefeatMax <= enemiesToDefeat)
        {
                ShowWinScreen();
                SwitchSpawn(false);
        }
    }
    public void GetRandomSpawnTimeout()
    {
        spawnTimeout = Random.Range(minSpawnTime, maxSpawnTime);
        int randomIndex = Random.Range(0, factories.Length);
        for (int i = 0; i < factories.Length; i++)
        {
            factories[i].thisFactoryWillCreate = false || i == randomIndex;
        }
    }
    public float GetRandomEnemySpd()
    {
        enemySpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
        return enemySpeed;
    }
    public void GetRandomEnemyCount()
    {
        enemiesToDefeatMax = Random.Range(minEnemies, maxEnemies);
        enemiesToDefeat = 0;
    }
    public void ShowLoseScreen()
    {
        _ui.ShowEndScreen();
        SwitchSpawn(false);
    } 
    public void ShowWinScreen() => _ui.ShowWinScreen();
    private void SpawnPlayer()
    {
        
        if (tp != null)
        {
            Destroy(tp);
        }
        
        tp = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        // playerHealth = playerHealthMax;
    }

    private void SwitchSpawn(bool value)
    {
        canMakeFactory = value;
    }
    public void RestartValues()
    {
        playerHealth = playerHealthMax;
        GetRandomEnemySpd();
        GetRandomSpawnTimeout();
        GetRandomEnemyCount();
        SpawnPlayer();
        SwitchSpawn(true);
    }
    
    
}