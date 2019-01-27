using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager Instance;

    [SerializeField] AudioManager audio;
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
        {
            StartCoroutine(RegeneratePlayer());
            audio.Volume("boucle_tortue",0.1f);
            audio.Play("rest");
            audio.Play("rest_jingle");
        }
        
    }
    void StopRegeneration()
    {
        if (debugSprite) if (debugSprite) debugSprite.SetActive(false);
        audio.Stop("rest");
        audio.Stop("rest_jingle");
        audio.Volume("boucle_tortue", 0.8f);
    }
    IEnumerator RegeneratePlayer()
    {
        int count = 0;
        if (houseCell == null || character == null) yield break;
        SpriteRenderer _renderer = character.GetComponent<SpriteRenderer>();
        while (character.CurrentCell == houseCell)
        {
           
            character.Energy += regenerationAmountPerSeconds;
            switch (count)
            {
                case 0:
                    audio.Play("up1");
                    count++;
                    break;
                case 1:
                    audio.Play("up2");
                    count++;
                    break;
                case 2:
                    audio.Play("up3");
                    count++;
                    break;
                case 3:
                    audio.Play("up4");
                    count++;
                    break;
                case 4:
                    audio.Play("up5");
                    count++;
                    break;
                case 5:
                    audio.Play("up6");
                    count = 0;
                    break;
            }
            yield return new WaitForSeconds(1); 
        }
        StopRegeneration(); 
        yield break; 
    }
}
