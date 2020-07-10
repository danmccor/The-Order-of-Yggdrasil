using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Name;
    public Text Health;
    public Text Attack;
    public Text Defence;
    public Text AvailableTurns;
    public GameObject UI;

    Camera camera; 
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UI.SetActive(false);
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            UI.SetActive(true);
            Name.text = "Hexagon";
            Health.text = "Occupied: " + hit.transform.gameObject.GetComponent<TileScript>().occupied.ToString();
            Attack.text = "Enemy Occupied: ";
            if (hit.transform.gameObject.GetComponent<TileScript>().enemyOccupied)
                Defence.text = "True";
            if (!hit.transform.gameObject.GetComponent<TileScript>().enemyOccupied)
                Defence.text = "False";

            if (hit.transform.gameObject.GetComponent<TileScript>().occupied)
                AvailableTurns.text = "Cannot move to this tile";
            if (!hit.transform.gameObject.GetComponent<TileScript>().occupied)
                AvailableTurns.text = "Can move to this tile";
        }

        layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            UI.SetActive(true);
            Name.text = hit.transform.gameObject.GetComponent<unitStats>().Name;
            Health.text = "Health: " + hit.transform.gameObject.GetComponent<unitStats>().Health.ToString();
            Attack.text ="Attack: " + hit.transform.gameObject.GetComponent<unitStats>().minimumAttack.ToString() + " - " + hit.transform.gameObject.GetComponent<unitStats>().maximumAttack.ToString();
            Defence.text ="Defence: " + hit.transform.gameObject.GetComponent<unitStats>().Defense.ToString();
            if (hit.transform.gameObject.GetComponent<UnitControl>().turnTaken)
                AvailableTurns.text = "Available Turns: 0";
            if (!hit.transform.gameObject.GetComponent<UnitControl>().turnTaken)
                AvailableTurns.text = "Available Turns: 1";
        }
        layerMask = 1 << 10;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            UI.SetActive(true);
            Name.text = "Enemy " + hit.transform.gameObject.GetComponent<unitStats>().Name;
            Health.text = "Health: " + hit.transform.gameObject.GetComponent<unitStats>().Health.ToString();
            Attack.text = "Attack: " + hit.transform.gameObject.GetComponent<unitStats>().minimumAttack.ToString() + " - " + hit.transform.gameObject.GetComponent<unitStats>().maximumAttack.ToString();
            Defence.text = "Defence: " + hit.transform.gameObject.GetComponent<unitStats>().Defense.ToString();
            if (hit.transform.gameObject.GetComponent<UnitControl>().turnTaken)
                AvailableTurns.text = "Available Turns: 0";
            if (!hit.transform.gameObject.GetComponent<UnitControl>().turnTaken)
                AvailableTurns.text = "Available Turns: 1";
        }
       
    }
}
