using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.git.controllables.abilities
{
    public abstract class MovementAbility : MonoBehaviour
    {
        private Vector3 _movVector;

        public Vector3 movVector
        {
            get
            {
                return _movVector;
            }
            private set
            {
                _movVector = value;
            }
        }

        public void setMovVector(Vector3 movVector)
        {
            this.movVector = movVector;
        }
        
        private Vector3 _rotVector;

        public Vector3 rotVector
        {
            get
            {
                return _rotVector;
            }
            private set
            {
                _rotVector = value;
            }
        }

        public void setRotVector(Vector3 rotVector)
        {
            this.rotVector = rotVector;
        }
    }
}
