using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager Instance; 

    Cell houseCell;
    [SerializeField, Range(1,25)] private int regenerationAmountPerSeconds = 5;
    [SerializeField] private GameObject debugSprite; 
    private Character character; 

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
        if (debugSprite) debugSprite.SetActive(false); 
    }

    private void Start()
    {
        character = FindObjectOfType<Character>(); 
        List<Cell> _c = GridManager.Instance.Cells.Where(c => c.State == CellState.House).ToList();
        if (_c.Count == 0) return;
        houseCell = _c.First(); 
    }

    public void StartRegeneration()
    {
        if (debugSprite) debugSprite.SetActive(true);
        StartCoroutine(RegeneratePlayer()); 
    }
    void StopRegeneration()
    {
        if (debugSprite) if (debugSprite) debugSprite.SetActive(false);
    }
    IEnumerator RegeneratePlayer()
    {
        if (houseCell == null || character == null) yield break;
        SpriteRenderer _renderer = character.GetComponent<SpriteRenderer>();
        while (character.CurrentCell == houseCell)
        {
            character.Energy += regenerationAmountPerSeconds;
            yield return new WaitForSeconds(1); 
        }
        StopRegeneration(); 
        yield break; 
    }
}
