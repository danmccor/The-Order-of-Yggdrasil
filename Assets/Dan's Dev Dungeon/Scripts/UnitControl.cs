/*
 * AUTHOR: DANIEL MCCORMICK
 * DATE: 14/10/19
 * PURPOSE: Move unit to selected tile, track which tile currently is stood on.
 * VERSION 1.4
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitControl : MonoBehaviour
{
    public bool Enemy;
    public bool turnTaken = false;
    public bool pickedUp = false;
    bool firstLoop = true;
    GameObject[] tiles;
    public GameObject currentTile = null;
    public Camera camera;
    public GameObject freeTile;
    public GameObject notFreeTile;
    public GameObject myTile;
    public GameObject lastTile;

    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        tiles = new GameObject[7];
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        //Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if (turnTaken)
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(4).gameObject.SetActive(false);
        }

        if (Camera.main.gameObject.GetComponent<PlayerControls>().allUnits.Count > 0)
        {
            if (!pickedUp)
            {
                this.transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (currentTile.transform.lossyScale.y / 1.75f), currentTile.transform.position.z);
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    pickedUp = false;
                    for (int j = 0; j < tiles.Length - 1; j++)
                    {
                        if (tiles[j] != null)
                        {
                            Destroy(tiles[j].gameObject);
                            this.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else
                        {
                            this.transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                    firstLoop = true;
                }
                if (firstLoop && pickedUp)
                {
                    this.transform.GetChild(0).gameObject.SetActive(true);
                    for (int i = 0; i < currentTile.GetComponent<TileScript>().touchingTiles.Count; i++)
                    {
                        if (currentTile.GetComponent<TileScript>().touchingTiles[i] != null && currentTile.GetComponent<TileScript>().touchingTiles[i].GetComponent<TileScript>().occupied == false && currentTile.GetComponent<TileScript>().touchingTiles[i].GetComponent<TileScript>().enemyOccupied == false)
                        {
                            tiles[i] = Instantiate(freeTile, currentTile.GetComponent<TileScript>().touchingTiles[i].transform.position, Quaternion.Euler(-90, 0, 0));
                        }
                        else if (currentTile.GetComponent<TileScript>().touchingTiles[i] != null && currentTile.GetComponent<TileScript>().touchingTiles[i].GetComponent<TileScript>().enemyOccupied == true)
                        {
                            tiles[i] = Instantiate(notFreeTile, currentTile.GetComponent<TileScript>().touchingTiles[i].transform.position, Quaternion.Euler(-90, 0, 0));
                        }

                    }
                    firstLoop = false;
                }

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    lookTowards(hit.transform.gameObject);
                    //transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + (hit.transform.lossyScale.y + 1), hit.transform.position.z);
                    if (Input.GetMouseButtonDown(0))
                    {
                        for (int i = 0; i < currentTile.GetComponent<TileScript>().touchingTiles.Count; i++)
                        {
                            if (currentTile.GetComponent<TileScript>().touchingTiles[i] == hit.transform.gameObject || currentTile == hit.transform.gameObject)
                            {
                                if (hit.transform.gameObject.GetComponent<TileScript>().occupied == false && hit.transform.gameObject.GetComponent<TileScript>().enemyOccupied == false)
                                {
                                    currentTile.gameObject.GetComponent<TileScript>().occupied = false;
                                    currentTile = hit.transform.gameObject;
                                    hit.transform.gameObject.GetComponent<TileScript>().occupied = true;
                                    hit.transform.gameObject.GetComponent<TileScript>().occupiedObj = this.gameObject;
                                    turnTaken = true;
                                    pickedUp = false;
                                    firstLoop = true;
                                    AudioManager.AudioPlayer.PlayAudio("Step");
                                    deleteTiles();
                                }
                                else if (hit.transform.gameObject.GetComponent<TileScript>().occupied == true && hit.transform.gameObject.GetComponent<TileScript>().enemyOccupied == true)
                                {
                                    if (this.GetComponent<unitStats>().AttackUnit(hit.transform.gameObject.GetComponent<TileScript>().occupiedObj))
                                    {
                                        currentTile.gameObject.GetComponent<TileScript>().occupied = false;
                                        currentTile = hit.transform.gameObject;
                                        hit.transform.gameObject.GetComponent<TileScript>().occupied = true;
                                        AudioManager.AudioPlayer.PlayAudio("Step");
                                        //Camera.main.GetComponent<PlayerControls>().resources += 10;
                                    }
                                    turnTaken = true;
                                    pickedUp = false;
                                    firstLoop = true;
                                    deleteTiles();
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + (currentTile.transform.lossyScale.y), currentTile.transform.position.z);
                }

            }
        }
    }
    public void lookTowards(GameObject Object)
    {
        transform.LookAt(Object.transform.position);
        transform.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hexagon"))
        {
            lastTile = currentTile;
            currentTile = collision.gameObject;
            if (this.gameObject.layer == 10)
            {
                currentTile.GetComponent<TileScript>().enemyOccupied = true;
                currentTile.GetComponent<TileScript>().occupied = true;
            }
            else
            {
                currentTile.GetComponent<TileScript>().enemyOccupied = false;
                currentTile.GetComponent<TileScript>().occupied = true;
            }
            currentTile.GetComponent<TileScript>().occupiedObj = this.gameObject;
        }
    }

    void deleteTiles()
    {
        for (int j = 0; j < tiles.Length - 1; j++)
        {
            if (tiles[j] != null)
            {
                Destroy(tiles[j].gameObject);
                this.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
