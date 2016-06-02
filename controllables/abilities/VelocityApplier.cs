using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.git.controllables.abilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityApplier : MovementAbility
    {
        [SerializeField]
        float velocity = 10;
        [SerializeField]
        float angularVelocity = 10;

        new Rigidbody rigidbody;

        public virtual void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            var velocityVector = transform.TransformVector(movVector) * velocity;
            var angularVelocityVector = transform.TransformVector(rotVector) * angularVelocity;

            rigidbody.velocity = velocityVector;
            rigidbody.angularVelocity = angularVelocityVector;
        }
    }
}
