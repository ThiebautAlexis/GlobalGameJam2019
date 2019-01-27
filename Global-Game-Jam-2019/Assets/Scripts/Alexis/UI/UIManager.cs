using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    [SerializeField] Image filledBar;
    [SerializeField] Character character; 


    void UpdateFilledBar()
    {
        if (!filledBar || !character) return;
        filledBar.fillAmount = Mathf.Lerp(filledBar.fillAmount, ((float)character.Energy / (float)character.MaxEnergy), Time.deltaTime * 10); 
    }

    void Start()
    {
        if(!character)
            character = FindObjectOfType<Character>(); 
    }

    private void Update()
    {
        UpdateFilledBar(); 
    }
}
