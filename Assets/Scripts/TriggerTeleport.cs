using Final;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final
{
    public class TriggerTeleport : MonoBehaviour
    {
        // Declaring variable
        // Exposing a gameobject that will act as a waypoint to teleport the player
        [SerializeField] GameObject spotToTeleport;

        // Other variable to allow the teleport to function
        private GameObject player;
        private Vector3 positionToTeleport;


        // Start is called before the first frame update
        void Start()
        {
            // Find the player object inside the scene
            player = GameObject.Find("Player");

            // Setting the position that the player will teleport to based on the position of the waypoint placed by the level designer
            positionToTeleport = new Vector3(spotToTeleport.transform.position.x, spotToTeleport.transform.position.y, spotToTeleport.transform.position.z);
        }


        // Collision
        private void OnTriggerEnter(Collider other)
        {
            // When the player collide with the trigger
            if (other.gameObject.name == "Player")
            {
                // Teleport the player to the waypoint position
                player.transform.position = positionToTeleport;
            }
        }
    }
}