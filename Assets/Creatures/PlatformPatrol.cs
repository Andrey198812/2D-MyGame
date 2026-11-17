using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlatformPatrol : Patrol
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _waitToGo;
    [SerializeField] private float _currentDirection = 1f;
    [SerializeField] private Animator _animator;

    private Creature _creature;


    private void Awake()
    {
        _creature = GetComponent<Creature>();
    }
    public override IEnumerator DoPatrol()
    {
        while (enabled)
        {
            bool isGroundAhead = IsGroundAhead();

            if (!isGroundAhead)
            {
                
                _creature.SetDirection(Vector2.zero);

               
                yield return null;

               
                if (_animator != null)
                {
                    _animator.SetBool("is_running", false);
                }

              
                yield return new WaitForSeconds(_waitToGo);

              
                _currentDirection *= -1f;

             
                Vector3 newPos = _groundCheck.position;
                newPos.x = transform.position.x + (0.5f * _currentDirection);
                _groundCheck.position = newPos;
                Physics2D.SyncTransforms();
            }

         
            Vector2 moveDirection = new Vector2(_currentDirection, 0);
            _creature.SetDirection(moveDirection);

           
            if (_animator != null && _creature != null)
            {
                _animator.SetBool("is_running", true);
            }

            yield return null;
        }
    }

    private bool IsGroundAhead()
    {
        Vector2 point = _groundCheck.position;
        Collider2D hit = Physics2D.OverlapCircle(point, _radius, _groundLayer);

        if (hit != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck == null) return;

        bool isGround = IsGroundAhead();

        Gizmos.color = isGround ? Color.green : Color.red;

        Gizmos.DrawWireSphere(_groundCheck.position, _radius);


    }
}
