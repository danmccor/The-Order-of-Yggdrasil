using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitStats : MonoBehaviour
{
    public bool Soldier = false;
    public bool Guard = false;
    public bool Archer = false;
    public bool Priest = false;
    public bool Boss = false;

    public string Name;
    public int Health;
    public int Defense;
    public int minimumAttack;
    public int maximumAttack;

    public Color OriginalRed;
    Color DesiredRed;
    float delay = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
        OriginalRed = GetComponent<MeshRenderer>().material.color;

        if (!Soldier && !Guard && !Archer && !Priest && !Boss)
        {
            Soldier = true;
        }

        if (Soldier)
        {
            Health = 10;
            Defense = 0; 
            minimumAttack = 2;
            maximumAttack = 6;
            Name = "Soldier";
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Guard)
        {
            Health = 15;
            Defense = 1;
            minimumAttack = 1;
            maximumAttack = 3;
            Name = "Guard";
            this.transform.GetChild(2).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (Boss)
        {
            Health = 20;
            Defense = 1;
            minimumAttack = 3;
            maximumAttack = 8;
            Name = "Boss";
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Health <= 0)
        {
            gameObject.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = false;
            gameObject.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = false;
            gameObject.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = null;
            if (this.gameObject.CompareTag("Unit"))
            {
                Camera.main.GetComponent<PlayerControls>().allUnits.Remove(this.gameObject);
            }
            else
            {
                GameObject.FindGameObjectWithTag("EnemyAI").gameObject.GetComponent<EnemyAI>().enemyUnits.Remove(this.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
    void TakeDamage(int amount)
    {
        int TotalDamage = amount - Defense;
        if(TotalDamage < 0)
        {
            TotalDamage = 0;
        }
        Health -= TotalDamage;
    }

    public bool AttackUnit(GameObject unit)
    {
        if(unit.GetComponent<unitStats>() != null)
        {
            DesiredRed = new Color(1, 0, 0, 1);
            unit.GetComponent<unitStats>().TakeDamage(Random.Range(minimumAttack, maximumAttack+1) - unit.GetComponent<unitStats>().Defense);
            if(this.Name == "Guard")
            {
                AudioManager.AudioPlayer.PlayAudio("Axe");
            }
            if(this.Name == "Soldier")
            {
                AudioManager.AudioPlayer.PlayAudio("Sword");
            }
            if(this.Name == "Boss")
            {
                AudioManager.AudioPlayer.PlayAudio("Sword");
            }

            
            unit.GetComponent<MeshRenderer>().material.color = DesiredRed;
            //unit.GetComponent<MeshRenderer>().material.color = OriginalRed;

            StartCoroutine(Wait(unit, delay));

            if (unit.GetComponent<unitStats>().Health <= 0)
            {
                if (unit.CompareTag("enemyUnit"))
                {
                    Camera.main.GetComponent<PlayerControls>().resources += 20;
                }
                return true;
            }
            else
            {
               // this.TakeDamage(Random.Range(unit.GetComponent<unitStats>().minimumAttack, unit.GetComponent<unitStats>().minimumAttack+1));
                return false;
            }
        }
        return false;
    }

    IEnumerator FlashToRed(GameObject unit)
    {
        while (unit.GetComponent<MeshRenderer>().material.color.r < 0.99f)
        {
            Debug.Log("Stuck in Loop! Red is: " + unit.GetComponent<MeshRenderer>().material.color.r + " out of 0.99f!");
            unit.GetComponent<MeshRenderer>().material.color = Color.Lerp(unit.GetComponent<MeshRenderer>().material.color, DesiredRed, delay * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FlashFromRed(unit));
    }

    IEnumerator FlashFromRed(GameObject unit)
    {
        while (unit.GetComponent<MeshRenderer>().material.color.r > OriginalRed.r)
        {
            unit.GetComponent<MeshRenderer>().material.color = Color.Lerp(unit.GetComponent<MeshRenderer>().material.color, OriginalRed, delay * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator Wait(GameObject unit, float time)
    {
        yield return new WaitForSeconds(time);
        if (unit != null)
        {
            unit.GetComponent<MeshRenderer>().material.color = unit.GetComponent<unitStats>().OriginalRed;
        }
    }
}
