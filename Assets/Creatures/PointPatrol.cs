using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Creatures
{
    internal class PointPatrol : Patrol
    {
        [SerializeField] private Transform[] _point;
        [SerializeField] private float _treshold = 1f;

        private Creature _creature;
        private int _destinationPoint;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }
        public override IEnumerator DoPatrol()
        {
           while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPoint = (int) Mathf.Repeat(_destinationPoint + 1, _point.Length);
                }

                var direction = _point[_destinationPoint].position - transform.position;
                direction.y = 0;
               
                _creature.SetDirection(direction.normalized);
                yield return null;
                
            }
        }

        private bool IsOnPoint()
        {
            return (_point[_destinationPoint].position - transform.position).magnitude < _treshold;
        }
    }
}
