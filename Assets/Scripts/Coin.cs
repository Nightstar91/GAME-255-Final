using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final
{
    public class Coin : MonoBehaviour
    {
        GameObject player;

        // On collision give the player one coin to be added to their stats
        private void OnTriggerEnter(Collider other)
        {
            player.GetComponent<Player>().GetCoin(1);
            gameObject.SetActive(false); // Using SetActive to be use for reset
        }

        private void Reset()
        {
            gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player");

            //GameManager.GetGameResetEvent().AddListener(Reset);
        }
    }
}
