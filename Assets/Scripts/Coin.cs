using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final
{
    public class Coin : MonoBehaviour
    {
        // Declaring variables
        // To search for a player object in the scene
        GameObject player;

        // On collision give the player one coin to be added to their stats
        private void OnTriggerEnter(Collider other)
        {
            player.GetComponent<Player>().GetCoin(1);       // Gain coin to be counted
            player.GetComponent<Player>().GainSpeed(0.5f);  // Gain speed with each coin pick up
            player.GetComponent<Player>().GainPoint(10);    // Gain points with each coin pick up
            gameObject.SetActive(false); // Using SetActive to be use for reset
        }


        // Start is called before the first frame update
        void Start()
        {
            // Finding the player object in the scene to reference for collision
            player = GameObject.Find("Player");
        }
    }
}
