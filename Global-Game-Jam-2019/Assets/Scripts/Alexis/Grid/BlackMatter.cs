using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlackMatter : MonoBehaviour
{

    public Cell LinkedCell; 

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!_collision.GetComponent<WaterJet>()) return; 
        if (LinkedCell != null)
        {
            LinkedCell.SetState(CellState.Free);
        }
        Destroy(gameObject);
        return; 
    }


}
