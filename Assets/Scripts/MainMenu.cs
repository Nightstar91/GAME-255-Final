using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Final
{
    public class MainMenu : MonoBehaviour
    {
        // Declaring variable
        // Exposing a variable for TextMeshProGUI
        [SerializeField] TextMeshProUGUI sensitivityLabel;
        [SerializeField] Slider sensitivitySlider;

        // An int variable to be use for customizing the player's camera sensitivity
        public int sensitivity = 500;

        public void Start()
        {
            // Freeing the cursor and making it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UpdateSensitivity();
        }


        public void PlayGame()
        {
            // Save the sensivity found inside the value of the slider
            SaveSensitivity();

            // Resetting the score back to 0
            PlayerPrefs.SetFloat("Total Score", 0);

            // Load in the first level
            SceneManager.LoadScene(1);
        }


        public void ExitGame()
        {
            Application.Quit();
        }


        // To be use for adjusting the camera sensitivity while in the main menu
        public void UpdateSensitivity()
        {
            // Assigning the value of sensitivity based on the value of the slider
            sensitivity = Convert.ToInt32(sensitivitySlider.value);

            // Displaying the text based on the value
            sensitivityLabel.text = string.Format("Sensitivity: {0}", sensitivity);
        }


        [ContextMenu("Save")]
        private void SaveSensitivity()
        {
            PlayerPrefs.SetInt("Sensitivity", sensitivity);
        }
    }
}
