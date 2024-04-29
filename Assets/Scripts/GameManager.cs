using Final;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Declaring all variable to keep track of object inside a level
    private int allCoinCount;
    private int coinCount;

    // Variable for in-game timer
    public float timer = 30;
    private bool isTimerOn = true;
    private bool beginTimer = false;
    private bool collectedAllCoin = false;

    // Exposing the TextMeshProGUI label
    [SerializeField] TextMeshProUGUI coinLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI resultLabel;
    [SerializeField] GameObject levelCompletePanel;

    // For Player Object
    private GameObject player;


    // A simple method that will countdown the timer based on deltatime and display it on the UI
    private void DecrementTime()
    {
        // Decrement the timer
        timer -= Time.deltaTime;

        // Displaying the timer to the player 
        timerLabel.text = string.Format("TIMER: {0:F2}", timer);
    }


    // A status to indicate that the level is currently frozen, timer is not running and player need to confirm to start the level
    private void LevelFrozen()
    {
        // Activating the frozen state in the UI
        levelCompletePanel.GetComponent<Image>().color = new Color(0.5f, 0.6f, 0.7f, 0.35f);
        levelCompletePanel.SetActive(true);

        // Changing text for the countdown
        timerLabel.text = string.Format("Awaiting");

        // Displaying Instruction to the player to begin the level
        resultLabel.text = string.Format("Objective: Move around and collect all the coin \nLeft Click to Start!");
    }


    // A status to indicate that the level is running, the timer is running and player has control of their character
    private void LevelPlaying()
    {
        // Deactivating the frozen state in the UI
        levelCompletePanel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        levelCompletePanel.SetActive(false);

        // Setting the result text to be empty
        resultLabel.text = string.Format("");

        // Begining the timer via boolean
        beginTimer = true;
    }


    // An endscreen status to indicate the player has failed to collect all the coin within 30 seconds
    private void LevelFailed()
    {
        // Player score's is set to score 0 for failing the level

        // Activating the fail result screen
        levelCompletePanel.GetComponent<Image>().color = new Color(1, 0, 0, 0.35f);
        levelCompletePanel.SetActive(true);

        // Changing text for the countdown
        timerLabel.text = string.Format("TIME IS UP");

        // Displaying failure result to the player
        resultLabel.text = string.Format("You Failed To Collect All The Coins\nPress Space To Continue");
    }


    // An endscreen status to indicate the player has successfully collected all the coin within 30 seconds. 
    private void LevelCompleted()
    {
        // Players earns a point bonus based on level completion and time remaining
        //player.GetComponent<Player>().EndRoundBonus();

        // Activating the success result screen
        levelCompletePanel.GetComponent<Image>().color = new Color(0, 0, 1, 0.35f);
        levelCompletePanel.SetActive(true);

        // Changing text for the countdown
        timerLabel.text = string.Format("TIMER: {0:F2}", timer);

        // Displaying Success result to the player
        resultLabel.text = string.Format("You Collected All The Coins\nScores: {0:F2}\nLeft Click To Continue", player.GetComponent<Player>().totalScore);
    }


    // Keeping track of all coins find inside the scene
    private void SearchAllCoins()
    {
        allCoinCount = GameObject.FindGameObjectsWithTag("Coin").Length;
    }


    private void CoinTracker()
    {
        // Grabbing the coin amount from the player object's player script
        coinCount = player.GetComponent<Player>().coinAmount;

        // If the player coin count equals to the amount of coin inside a level...
        if (coinCount == allCoinCount)
        {
            // They have successfully collected all the coin
            collectedAllCoin = true;
        }

        // Formatting to display coins remaining inside a level for player to reference to
        coinLabel.text = string.Format("Coin Left: {0} / {1}", coinCount, allCoinCount);
    }


    // A method in which the timer and coin is being tracked. Based on the result, the player will either fail the level or win the level
    private void TryToEndLevel()
    {
        // Once the timer begin and the player hasn't collected all the coins
        if (beginTimer == true && isTimerOn == true && collectedAllCoin == false)
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

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SearchAllCoins();
        player = GameObject.Find("Player");

        // Adding listener for player specific event
        // For FreezePlayerEvent
        Player.GetFreezePlayerEvent().AddListener(LevelFrozen);

        // For BeginCountdownEvent
        Player.GetBeginCountDownEvent().AddListener(LevelPlaying);

    }

    // Update is called once per frame
    void Update()
    {
        // Methods that deals with keeping track of in game stats
        TryToEndLevel();
        CoinTracker();
    }
}
