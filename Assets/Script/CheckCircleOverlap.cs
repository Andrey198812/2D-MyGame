using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    internal class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1.0f;
       // [SerializeField] private string _tag;
        [SerializeField] private Collider2D[] _interactionresult = new Collider2D[5];
        public GameObject[] GetObjectInRangee()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionresult);
            var overLaps = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                overLaps.Add(_interactionresult[i].gameObject);
            }
            return overLaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparetRed;
            Handles.DrawSolidDisc(transform.position,Vector3.forward, _radius); 
        }
    }
}
