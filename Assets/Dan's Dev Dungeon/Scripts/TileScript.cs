/*
 * AUTHOR: DANIEL MCCORMICK
 * DATE: 14/10/19
 * PURPOSE: Attach connected tiles into a game object so we are able to access what tiles can connect
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public bool playerSpawnTile = false;
    public bool enemySpawnTile = false;

    public bool TeleportTile = false;
    public GameObject ToTile;

    public bool occupied;
    public bool enemyOccupied; 
    public GameObject occupiedObj;
    public List<GameObject> touchingTiles;
    // Start is called before the first frame update
    void Start()
    {
        touchingTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hexagon"))
        {
            if (!touchingTiles.Contains(collision.gameObject))
            {
                touchingTiles.Add(collision.gameObject);
            }

            //for (int i = 0; i < 6; i++)
            //{
            //    if (touchingTiles[i] == collision.gameObject)
            //    {
            //        //Debug.Log(collision.gameObject + " Dupe found, breaking loop!");
            //        break;
            //    }
            //    else if (touchingTiles[i] == null)
            //    {
            //        //Debug.Log("No Dupe found! Adding " + collision.gameObject);
            //        touchingTiles.Add(collision.gameObject);
            //        break;
            //    }
            //}
        }
        if(collision.gameObject.CompareTag("Unit") || collision.gameObject.CompareTag("enemyUnit"))
        {
            if (this.TeleportTile)
            {
                collision.gameObject.GetComponent<UnitControl>().currentTile = ToTile;
                collision.gameObject.transform.position = ToTile.transform.position;
            }
        }
    }
}
