using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image winPanel;
    [SerializeField] private Image losePanel;
    [SerializeField] private Button restartWinButton;
    [SerializeField] private Button restartLoseButton;
    [SerializeField] private TMP_Text headerText;

    private GlobalContainer _gc;
    // Start is called before the first frame update
    private void Awake()
    {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalContainer>();
        //restartWinButton.onClick.AddListener(() => RestartUI());
        //restartLoseButton.onClick.AddListener(() => RestartUI());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        headerText.text = "Health: " + _gc.playerHealth;
    }

    public void ShowEndScreen()
    {
        losePanel.gameObject.SetActive(true);
    }
    public void ShowWinScreen()
    {
        winPanel.gameObject.SetActive(true);
    }

    public void RestartUI()
    {
        _gc.RestartValues();
        losePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
    }
}
