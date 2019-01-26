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

    [SerializeField, Range(1,5)] private int speed = 1;

    [SerializeField, Range(10, 100)] private int maxEnergy = 100;
    public int MaxEnergy { get { return maxEnergy; } }
    [SerializeField] int energy = 100;
    public int Energy { get { return energy; } }

    [SerializeField, Range(1, 5)] private int waterJetRange = 1;

    [SerializeField] private Animator characterAnimator;

    [SerializeField] private LayerMask cellLayer;

    List<Cell> cellPath = new List<Cell>(); 
    #endregion

    #region Methods
    void CheckInput()
    {
        if (isMoving || !GridManager.Instance) return; 
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D _hit = (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, cellLayer)); 
            if(_hit)
            {
                //cellPath = GridManager.Instance.ComputePath();  
            }
        }
    }
    #endregion

    #region UnityMethod
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput(); 
    }
    #endregion
}
