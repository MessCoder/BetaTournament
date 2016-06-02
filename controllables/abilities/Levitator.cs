using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.october.controllables.abilities
{
    [RequireComponent(typeof(Rigidbody))]
    class Levitator : MovementAbility
    {
        new private Rigidbody rigidbody;

        [SerializeField]
        float velocity = 10;
        [SerializeField]
        float angularVelocity = 10;

        [SerializeField]
        float groundMovementInstantaneity = 0.7f;
        [SerializeField]
        float airMovementForceMultiplier = 5;

        [SerializeField]
        float balanceInstantaneity = 0.4f;

        [SerializeField]
        private float equilibriumHeight = 1;
        
        private float adjuster;
        private float maxRayDistance;
        private static float insignificantForcePortion = 0.15f;

        // LevitatingForce(h) = adjuster / h^2

        // LevitatingForce(equilibriumHeight) = m*g
        // m*g = adjuster / equilibriumHeight^2
        // adjuster = m*g*equilibriumHeight^2

        // LevitatingForce(maxRayDistance) = m*g*insignificantForcePortion
        // m*g*insignificantForcePotion = adjuster / maxRayDistance^2
        // m*g*insignificantForcePotion = m*g*equilibriumHeight / maxRayDistance^2
        // insignificantForcePotion = equilibriumHeight / maxRayDistance^2
        // maxRayDistance = sqrt( equilibriumHeight / insignificantForcePortion )

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            adjustVariablesToHeight();
        }

        private void adjustVariablesToHeight()
        {
            adjuster = rigidbody.mass * 9.8f * Mathf.Pow(equilibriumHeight, 2);
            maxRayDistance = Mathf.Sqrt(equilibriumHeight / insignificantForcePortion);
        }

        private bool raycastToGround(out RaycastHit hit)
        {
            return Physics.Raycast(transform.position, Vector3.down, out hit, maxRayDistance);
        }

        private void applyLevitatingForce(float distanceToGround)
        {
            rigidbody.AddForce(Vector3.up * adjuster / Mathf.Pow(distanceToGround, 2));
        }

        private void balance(Vector3 upVector)
        {
            rigidbody.angularVelocity *= 1 - balanceInstantaneity;

            Vector3 forwardVector = Vector3.ProjectOnPlane(transform.forward, upVector);
            Quaternion targetRotation = Quaternion.LookRotation(forwardVector, upVector);
            
            //transform.rotation = targetRotation;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, balanceInstantaneity);
        }

        private void controlMovementGround(Vector3 movVector)
        {
            Vector3 localZPlaneMovVector = Vector3.ProjectOnPlane(movVector, Vector3.up);
            Vector3 worldLocalZPlaneMovVector = transform.TransformVector(localZPlaneMovVector);
            Vector3 worldLocalZPlaneTargetVelocity = worldLocalZPlaneMovVector * velocity;

            Vector3 worldLocalZVelocity = Vector3.Project(rigidbody.velocity, transform.up);

            Vector3 targetVelocity = worldLocalZPlaneTargetVelocity + worldLocalZVelocity;

            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, groundMovementInstantaneity);
        }

        private void controlMovementAir(Vector3 movVector)
        {
            rigidbody.AddRelativeForce(movVector * velocity * airMovementForceMultiplier);
        }
        
        private void controlRotation(Vector3 rotVector)
        {
            transform.Rotate(rotVector * angularVelocity, Space.Self);
        }

        public void FixedUpdate()
        {
            RaycastHit hit = new RaycastHit();

            if (raycastToGround(out hit))
            {
                applyLevitatingForce(hit.distance);
                balance(hit.normal);

                controlMovementGround(movVector);
            }
            else
            {
                controlMovementAir(movVector);
            }

            controlRotation(rotVector);
        }
    }
}
