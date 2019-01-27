using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : MonoBehaviour
{
    private void Start()
    {
        BlockTile(); 
    }

    void BlockTile()
    {
        Cell _c = GridManager.Instance.GetCellFromPosition(transform.position);
        _c.MakeCellNonNavigable(); 
    }
}
