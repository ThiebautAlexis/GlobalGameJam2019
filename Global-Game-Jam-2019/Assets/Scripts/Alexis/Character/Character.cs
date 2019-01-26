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
    [SerializeField, Range(1,5)] private int speed = 1;

    [SerializeField, Range(10, 100)] private int maxEnergy = 100;
    public int MaxEnergy { get { return maxEnergy; } }
    [SerializeField] int energy = 100;
    public int Energy { get { return energy; } }

    [SerializeField, Range(1, 5)] private int waterJetRange = 1;

    [SerializeField] private Animator characterAnimator; 
    #endregion

    #region Methods
    void SetDestination()
    {
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
        
    }
    #endregion
}
