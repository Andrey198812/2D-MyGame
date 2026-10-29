using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Component
{
    internal class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var  hero  = go.GetComponent<Hero>();
            if (hero != null) 
            {
                hero.ArmHero();
            }
        }

    }
}
