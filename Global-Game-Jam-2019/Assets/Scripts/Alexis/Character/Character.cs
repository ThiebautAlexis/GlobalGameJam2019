using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public bool IsExhausted
    {
        get
        {
            return energy <= maxEnergy / 2;
        }
    }
    private bool isLookingRight = true; 

    [SerializeField, Range(1,5)] private int speed = 1;

    [SerializeField, Range(10, 500)] private int maxEnergy = 100;
    public int MaxEnergy { get { return maxEnergy; } }
    [SerializeField] int energy = 100;

    [SerializeField] public AudioManager audio;

    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
            energy = Mathf.Clamp(energy, 0, maxEnergy);
            characterAnimator.SetBool("IsExhausted", IsExhausted); 
        }
    }

    [SerializeField, Range(1, 5)] private int waterJetCost = 2;
    [SerializeField, Range(1, 5)] private int waterJetRange = 1;
    public int WaterJetRange
    {
        get
        {
            return IsExhausted ? waterJetRange / 2 : waterJetRange;
        }
    }

    [SerializeField] WaterJet waterJetPrefab; 

    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private SpriteRenderer renderer; 

    private Cell currentCell;
    public Cell CurrentCell { get { return currentCell; } }

    [SerializeField] private Animator characterAnimator;

     [SerializeField] private Orientation orientation = Orientation.SouthEast; 

    #endregion

    #region Methods
    IEnumerator FollowPath(List<Vector2> _pathToFollow)
    {
        characterAnimator.SetBool("IsMoving", true); 
        isMoving = true;
        float timecount = 0.0f;
        List<Vector2> _path = _pathToFollow;
        int _index = 0; 
        while(Vector3.Distance(transform.position, _pathToFollow.Last()) > .1f)
        {
            int rand_son = UnityEngine.Random.Range(1, 8);
            float rand_pitch = UnityEngine.Random.Range(0.5f, 1.5f);
            timecount -= Time.deltaTime;
            if(timecount < 0.0f)
            {
                switch (rand_son)
                {
                    case 1:

                        audio.Play("run1", rand_pitch);
                        break;
                    case 2:
                        audio.Play("run2", rand_pitch);
                        break;
                    case 3:
                        audio.Play("run3", rand_pitch);
                        break;
                    case 4:
                        audio.Play("run4", rand_pitch);
                        break;
                    case 5:
                        audio.Play("run5", rand_pitch);
                        break;
                    case 6:
                        audio.Play("run6", rand_pitch);
                        break;
                    case 7:
                        audio.Play("run7", rand_pitch);
                        break;
                }
                timecount = 0.3f;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, _pathToFollow[_index], Time.deltaTime * speed);
            if(Vector3.Distance(transform.position, _pathToFollow[_index]) < .1f)
            {
                currentCell = GridManager.Instance.GetCellFromPosition(_pathToFollow[_index]);
                //if (renderer) renderer.sortingOrder = (int)currentCell.TilePosition.y; 
                _index++;
                Energy--; 
                if (_index >= _pathToFollow.Count)
                {
                    characterAnimator.SetBool("IsMoving", false);
                    isMoving = false;
                    if (currentCell.State == CellState.House)
                    {
                        HouseManager.Instance.StartRegeneration();
                        // QUAND MAISON -> DESACTIVATE SPRITE
                        renderer.sortingOrder = -999; 
                    }
                    yield break; 
                }
                Vector2 _dir = _pathToFollow[_index] - currentCell.TilePosition;
                UpdateOrientation(_dir); 
            }
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
        characterAnimator.SetBool("IsMoving", false);
        if (currentCell.State == CellState.House) HouseManager.Instance.StartRegeneration(); 
    }

    void CheckInput()
    {
        if (isMoving || !GridManager.Instance) return;
        RaycastHit2D _hit; 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _hit = (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, cellLayer)); 
            if(_hit)
            {
                Cell _destination = GridManager.Instance.GetClosestCell(_hit.point);
                if (_destination.State == CellState.NonNavigable) return; 
                Cell _origin = GridManager.Instance.GetClosestCell(transform.position);
                if (_destination == _origin) return; 
                List<Vector2> _cellPath = CalculatePath(_origin, _destination);
                if (_cellPath == null)
                {
                    return;
                }
                if (currentCell.State == CellState.House) renderer.sortingOrder = 2; 
                StartCoroutine(FollowPath(_cellPath)); 
            }
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && energy > 0 && currentCell.State != CellState.House)
        {
            _hit = (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, cellLayer));
            if(_hit)
            {
                Cell _c = GridManager.Instance.GetClosestCell(_hit.point);
                Vector3 _dir = GridManager.Instance.GetStraightLine(currentCell, _c);
                if (_dir == Vector3.zero) return;
                UpdateOrientation(_dir);
                characterAnimator.SetTrigger("SprayWater");
                int rand_son = UnityEngine.Random.Range(1, 3);
                float rand_pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                switch (rand_son)
                {
                    case 1:

                    audio.Play("shot1", rand_pitch);
                    break;
                    case 2:
                    audio.Play("shot2", rand_pitch);
                    break;
                }
                WaterJet _jet = Instantiate(waterJetPrefab, transform.position, Quaternion.identity).GetComponent<WaterJet>();
                float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
                _jet.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                _jet.ApplyDirection(_dir, WaterJetRange);
                Energy -= waterJetCost;
            }
        }
    }

    void UpdateOrientation(Vector2 _dir)
    {
        Orientation _previousOrientation = orientation; 
        float _angle = Vector2.SignedAngle(Vector2.right, _dir);
        //Si compris entre 0 et 90 -> NE
        if (_angle > 0 && _angle < 90) orientation = Orientation.NorthEast;
        //Si compris entre 90 et 180 -> NW
        else if (_angle > 90 && _angle < 180) orientation = Orientation.NorthWest;
        //Si compris entre 0 et -90 -> SE
        else if (_angle < 0 && _angle > -90) orientation = Orientation.SouthEast;
        //Si comrpis entre -90 et -180 -> SW
        else orientation = Orientation.SouthWest;

        bool _isNorth = orientation == Orientation.NorthEast || orientation == Orientation.NorthEast;
        characterAnimator.SetBool("isLookingNorth", _isNorth); 
        //Check si on doit inverser le sprite ou non -> Pour le faire regarder dans les 2 
        renderer.flipX = orientation == Orientation.NorthWest || orientation == Orientation.SouthWest; 
        
    }

    /// <summary>
    /// Calculate path from an origin to a destination 
    /// Set the path when it can be calculated 
    /// </summary>
    /// <returns>Return if the path can be calculated</returns>
    public List<Vector2> CalculatePath(Cell _origin, Cell _destination)
    {
        List<Cell> _cellDatas = GridManager.Instance.Cells; 
        //Open list that contains all heuristically calculated triangles 
        List<Cell> _openList = new List<Cell>();
        //returned path
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();

        /* ASTAR: Algorithm*/
        // Add the origin point to the open and close List
        // Set its heuristic cost and its selection state
        _openList.Add(_origin);
        _origin.HeuristicCostFromStart = 0;
        _origin.HasBeenSelected = true;
        _cameFrom.Add(_origin, _origin); 
        float _cost = 0;
        Cell _currentCell;
        while (_openList.Count > 0)
        {
            //Get the point with the best heuristic cost
            _currentCell = GetBestCell(_openList);
            //If this point is in the targeted triangle, 
            if (_currentCell == _destination)
            {
                _cost = _currentCell.HeuristicCostFromStart + 1;
                _destination.HeuristicCostFromStart = _cost;
                //add the destination point to the close list and set the previous point to the current point or to the parent of the current point if it is in Line of sight 

                //_cameFrom.Add(_currentCell, _currentCell);

                //Clear all points selection state
                foreach (Cell c in _cellDatas)
                {
                    c.HasBeenSelected = false;
                }
                //Build the path
                return BuildPath(_cameFrom);
            }
            //Get all linked points from the current point
            //_linkedPoints = GetLinkedPoints(_currentPoint, trianglesDatas);
            for (int i = 0; i < _currentCell.LinkedPosition.Count; i++)
            {
                Cell _linkedCell = _cellDatas.Where(c => c.TilePosition == _currentCell.LinkedPosition[i]).FirstOrDefault();
                // If the linked points is not selected yet
                if (!_linkedCell.HasBeenSelected && _linkedCell.State != CellState.NonNavigable)
                {
                    // Calculate the heuristic cost from start of the linked point
                    _cost = _currentCell.HeuristicCostFromStart + 1;
                    _linkedCell.HeuristicCostFromStart = _cost;
                    if (!_openList.Contains(_linkedCell) || _cost < _linkedCell.HeuristicCostFromStart)
                    {
                        // Set the heuristic cost from start for the linked point
                        _linkedCell.HeuristicCostFromStart = _cost;
                        //Its heuristic cost is equal to its cost from start plus the heuristic cost between the point and the destination
                        _linkedCell.HeuristicCostTotal = Vector3.Distance(_linkedCell.TilePosition, _destination.TilePosition) + _cost;
                        //Set the point selected and add it to the open and closed list
                        _linkedCell.HasBeenSelected = true;
                        _openList.Add(_linkedCell);
                        _cameFrom.Add(_linkedCell, _currentCell);
                    }
                }
            }
        }
        foreach (Cell c in _cellDatas)
        {
            c.HasBeenSelected = false;
        }
        return null;
    }

    /// <summary>
    /// Get the cell with the best heuristic cost from a list 
    /// Remove this point from the list and return it
    /// </summary>
    /// <param name="_cells">list where the points are</param>
    /// <returns>point with the best heuristic cost</returns>
    Cell GetBestCell(List<Cell> _cells)
    {
        int bestIndex = 0;
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].HeuristicCostTotal < _cells[bestIndex].HeuristicCostTotal)
            {
                bestIndex = i;
            }
        }

        Cell _bestCell = _cells[bestIndex];
        _cells.RemoveAt(bestIndex);
        return _bestCell;
    }

    /// <summary>
    /// Reverse the path and return it
    /// </summary>
    /// <param name="_pathToBuild"></param>
    /// <returns>return the path</returns>
    List<Vector2> BuildPath(Dictionary<Cell, Cell> _pathToBuild)
    {
        // Building absolute path -> Link all triangle's CenterPosition together
        // Adding _origin and destination to the path
        Cell _currentCell = _pathToBuild.Last().Key;
        List<Vector2> _absoluteTrianglePath = new List<Vector2>();
        while (_currentCell != _pathToBuild.First().Key)
        {
            _absoluteTrianglePath.Add(_currentCell.TilePosition);
            _currentCell = _pathToBuild[_currentCell];
        }
        _absoluteTrianglePath.Add(_currentCell.TilePosition);
        //Reverse the path to start at the origin 
        _absoluteTrianglePath.Reverse();
        return _absoluteTrianglePath; 
    }
    #endregion

    #region UnityMethod
    // Start is called before the first frame update
    void Start()
    {
        currentCell = GridManager.Instance.GetClosestCell(transform.position);
        if (!characterAnimator) characterAnimator = GetComponent<Animator>();
        if (!renderer) renderer = GetComponent<SpriteRenderer>();
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