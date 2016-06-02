using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.git
{
    public class Follower : MonoBehaviour
    {
        [SerializeField]
        Transform followedTransform = null;

        [SerializeField]
        Vector3 relativeTargetPosition = new Vector3(0, 0, 0.2f);

        [SerializeField]
        float positioningTime = 0.1f;

        [SerializeField]
        float rotationTime = 0.02f;

        public void Start()
        {
            transform.SetParent(null);
        }

        public void FixedUpdate()
        {
            var targetPosition = followedTransform.position + followedTransform.TransformDirection(relativeTargetPosition);

            transform.localPosition =
                Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime / positioningTime);

            transform.localRotation =
                Quaternion.Lerp(transform.rotation, followedTransform.rotation, Time.fixedDeltaTime / rotationTime);
        }
    }
}
