using Final;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Declaring all variable to keep track of object inside a level
    private int allCoinCount;
    private int coinCount;

    // Variable for in-game timer
    private float timer = 30;
    private bool isTimerOn = true;
    private bool collectedAllCoin = false;

    // Exposing the textmeshprogui label
    [SerializeField] TextMeshProUGUI coinLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] GameObject levelCompletePanel;

    // For Player Object
    private GameObject player;

    // Variable to be use for event
    public static GameManager instance;

    public UnityEvent startCountDown;

    private void DecrementTime()
    {
        // Decrement the timer
        timer -= Time.deltaTime;

        // Displaying the timer to the player 
        timerLabel.text = string.Format("TIMER: {0:F2}", timer);
    }


    private void LevelFailed()
    {
        // Activating the fail result screen
        levelCompletePanel.GetComponent<Image>().color = new Color(1, 0, 0, 0.35f);
        levelCompletePanel.SetActive(true);

        // Changing text for the countdown
        timerLabel.text = string.Format("TIME IS UP");

        // Displaying failure result to the player
        resultLabel.text = string.Format("You Failed To Collect All The Coins\nPress Space To Continue");
    }

    private void LevelCompleted()
    {
        // Activating the fail result screen
        levelCompletePanel.GetComponent<Image>().color = new Color(0, 0, 1, 0.35f);
        levelCompletePanel.SetActive(true);

        // Changing text for the countdown
        timerLabel.text = string.Format("TIMER: {0:F2}", timer);

        // Displaying failure result to the player
        resultLabel.text = string.Format("You Colleced All The Coins\nPress Space To Continue");
    }


    private void SearchAllCoins()
    {
        allCoinCount = GameObject.FindGameObjectsWithTag("Coin").Length;
    }


    private void CoinTracker()
    {
        // Grabbing the coin amount from the player object's player script
        coinCount = player.GetComponent<Player>().coinAmount;

        if (coinCount == allCoinCount)
        {
            collectedAllCoin = true;
        }


        // Formatting to display coins remaining inside a level for player to reference to
        coinLabel.text = string.Format("Coin Left: {0} / {1}", coinCount, allCoinCount);
    }


    private void TryToEndLevel()
    {
        // Once the player made his first movement
        if (player.GetComponent<Player>().firstMovement == true)
        {
            if (isTimerOn == true && collectedAllCoin == false)
            {
                // If the timer is not below or equal to 0 then continue to countdown
                if (timer >= 0)
                {
                    DecrementTime();
                }
                // Else the countdown is finished and set the boolean to false to stop the timer
                else
                {
                    timer = 0;
                    isTimerOn = false;
                }

            }

            // If either the timer fully countdown to 0 or the player collected all the coins...
            else
            {
                // Fail the player due to them running out of time
                if (isTimerOn == false)
                {
                    LevelFailed();
                }

                // Or the player win for this level!
                if (collectedAllCoin == true)
                {
                    LevelCompleted();
                }
            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        SearchAllCoins();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Methods that deals with keeping track of in game stats
        TryToEndLevel();
        CoinTracker();
    }
}
