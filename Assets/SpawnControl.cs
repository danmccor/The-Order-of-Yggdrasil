using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public List<GameObject> spawnTiles;
    public List<GameObject> spawnHex = new List<GameObject>();
    public List<GameObject> playerHex = new List<GameObject>();
   
    // Start is called before the first frame update
    void Start()
    {
        spawnTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hexagon"))
        {
            spawnTiles.Add(other.gameObject);
        }
        if (other.CompareTag("Unit"))
        {
            foreach(GameObject hex in spawnTiles)
            {
                hex.GetComponent<TileScript>().enemySpawnTile = false;
                hex.GetComponent<TileScript>().playerSpawnTile = true;
            }
            foreach(GameObject tile in spawnHex)
            {
                tile.SetActive(false);
                tile.SetActive(false);
            }
            foreach (GameObject tile in playerHex)
            {
                tile.SetActive(true);
                tile.SetActive(true);
            }
        }
    }
}
