using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Character:
 * Created by: Thiebaut Alexis
 * Date: 26/01/2019
 * Description: 
 * 
 * [CHANGES]
 * 26/01/2019 by Thiebaut Alexis
 * Initialisation of the Character Script
 */
public class Character : MonoBehaviour
{
    #region Fields and Properties
    private bool isMoving = false; 
    public bool IsExhausted { get { return energy <= maxEnergy / 2;  } }

    [SerializeField, Range(1,5)] private int speed = 1;

    [SerializeField, Range(10, 100)] private int maxEnergy = 100;
    public int MaxEnergy { get { return maxEnergy; } }
    [SerializeField] int energy = 100;
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
            Mathf.Clamp(energy, 0, maxEnergy); 
        }
    }

    [SerializeField, Range(1, 5)] private int waterJetCost = 2;
    [SerializeField, Range(1, 5)] private int waterJetRange = 1;
    public int WaterJetRange { get { return IsExhausted ? waterJetRange /2 : waterJetRange;  } }
    [SerializeField] WaterJet waterJetPrefab; 

    [SerializeField] private LayerMask cellLayer;
    private Cell currentCell;

    [SerializeField] private Animator characterAnimator;

    private Orientation orientation = Orientation.SouthEast; 

    List<Cell> cellPath = new List<Cell>(); 
    #endregion

    #region Methods
    void CheckInput()
    {
        if (isMoving || !GridManager.Instance) return;
        RaycastHit2D _hit; 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _hit = (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, cellLayer)); 
            if(_hit)
            {
                Cell _c = GridManager.Instance.GetClosestCell(_hit.point); 
                Debug.Log($"Move To {_c.TilePosition}!");
            }
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && energy > 0)
        {
            _hit = (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, cellLayer));
            if(_hit)
            {
                Cell _c = GridManager.Instance.GetClosestCell(_hit.point);

                Vector3 _dir = GridManager.Instance.GetStraightLine(currentCell, _c);
                if (_dir == Vector3.zero) return;
                WaterJet _jet = Instantiate(waterJetPrefab, transform.position, Quaternion.identity).GetComponent<WaterJet>();
                _jet.ApplyDirection(_dir, WaterJetRange);
                Energy -= waterJetCost;
            }
        }
    }

    void FireWaterJet()
    {

    }
    #endregion

    #region UnityMethod
    // Start is called before the first frame update
    void Start()
    {
        currentCell = GridManager.Instance.GetClosestCell(transform.position); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput(); 
    }
    #endregion
}

public enum Orientation
{
    NorthWest, 
    NorthEast, 
    SouthWest, 
    SouthEast
}