using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlackMatter : MonoBehaviour
{

    public Cell LinkedCell;
    public AudioManager audio;
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!_collision.GetComponent<WaterJet>()) return; 
        if (LinkedCell != null)
        {
            audio = FindObjectOfType<AudioManager>();
            LinkedCell.SetState(CellState.Free);
            int i = Random.Range(0, 2);
            if (i == 0)
            {
                audio.Play("clean1");
            }
            else
            {
                audio.Play("clean2");
            }
            

        }
        Destroy(gameObject);
        _collision.GetComponent<WaterJet>().StopProjectile(); 
        return; 
    }


}
