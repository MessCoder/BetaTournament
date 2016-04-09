using System.Collections.Generic;
using UnityEngine;

namespace Assets.october
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField]
        private float maximumDeviationDegrees = 30;
        [SerializeField]
        private float maximumDistance = 50;

        private HashSet<Destructible> whitelist = new HashSet<Destructible>();

        public bool addToWhitelist(Destructible friend)
        {
            return whitelist.Add(friend);
        }
        public bool removeFromWhitelist(Destructible traitor)
        {
            return whitelist.Remove(traitor);
        }

        public Destructible getTargetInRange()
        {
            float lessDeviatedAngle = maximumDeviationDegrees;
            Destructible target = null;

            foreach (Destructible destructible in Destructible.destructibles)
            {
                if (whitelist.Contains(destructible))
                    continue;

                Vector3 toTarget = destructible.transform.position - transform.position;
                float distToTarget = toTarget.magnitude;

                if (distToTarget > maximumDistance)
                    continue;

                float angle = Vector3.Angle(transform.forward, toTarget);

                if (angle >= lessDeviatedAngle)
                    continue;

                Ray ray = new Ray(transform.position, toTarget);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distToTarget))
                    if (hit.transform == destructible.transform)
                    {
                        lessDeviatedAngle = angle;
                        target = destructible;
                    }
            }

            return target;
        }
    }
}
