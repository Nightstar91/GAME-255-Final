using Final;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTeleport : MonoBehaviour
{
    [SerializeField] GameObject spotToTeleport;
    private GameObject player;
    private Vector3 positionToTeleport;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        positionToTeleport = new Vector3(spotToTeleport.transform.position.x, spotToTeleport.transform.position.y, spotToTeleport.transform.position.z);

        Debug.Log($"X:{positionToTeleport.x} Y:{positionToTeleport.y} Z:{positionToTeleport.z}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player.transform.position = positionToTeleport;
        }
    }
}
