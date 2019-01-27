using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D),typeof(Rigidbody2D))]
public class WaterJet : MonoBehaviour
{
    #region Fields and properties
    private Vector3 destination;
    private bool canMove = false;
    [SerializeField, Range(1, 10)] float speed = 1; 
    #endregion

    #region Methods
    public void ApplyDirection(Vector3 _direction, float _distance)
    {
        destination = transform.position + (_direction * _distance);
        canMove = true; 
    }
    #endregion

    #region UnityMethods
    private void Update()
    {
        if (!canMove) return;
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, destination) <.1f)
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void StopProjectile()
    {
        Destroy(gameObject); 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan; 
        Gizmos.DrawSphere(destination, .5f); 
    }
    #endregion
}
