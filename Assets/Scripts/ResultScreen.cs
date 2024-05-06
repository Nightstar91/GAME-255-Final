using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScreen : MonoBehaviour
{
    // Declaring variable
    // Exposing a variable for TextMeshProGUI
    [SerializeField] TextMeshProUGUI totalScoreLabel;

    // Start is called before the first frame update
    void Start()
    {
        // Allow the player's cursor to move around freely
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Displaying the total scoree of all 5 level
        totalScoreLabel.text = string.Format("Your Total Score is...\n{0}", PlayerPrefs.GetFloat("Total Score"));
    }


    public void ReturnToMainMenu()
    {
        // Resetting the score back to 0
        PlayerPrefs.SetFloat("Total Score", 0);

        // Loading back to main menu
        SceneManager.LoadScene(0);
    }
}
