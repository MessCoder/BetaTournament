using System;
using UnityEngine;

namespace Assets.git.controllables.deadControllables
{
    public class RespawnPointSelector : HudlessDeadControllable
    { 
        private static GameObject[] respawnPoints;
        private static bool[] respawnPointWatched;
        private static string respawnTag = "Respawn";
        private static int lastLevelIndex = -1;

        private int _currentRespawnPoint = -1;
        private IController _controller;

        public override event Action<Vector3, Quaternion> onRespawn;
        public override event Action onCease;

        private int currentRespawnPoint
        {
            get
            {
                return _currentRespawnPoint;
            }
            set
            {
                /// Set the last respawn point free
                if (currentRespawnPoint >= 0 && 
                    currentRespawnPoint < respawnPoints.Length)
                {
                    respawnPointWatched[currentRespawnPoint] = false;
                }

                /// Fit the passed value into range
                if (value < 0)
                {
                    value = (value % respawnPoints.Length) + respawnPoints.Length;
                }
                else
                {
                    value = value % respawnPoints.Length;
                }

                /// Preparing for finding a free respawn points - get the change direction
                int dir = value - _currentRespawnPoint;
                dir /= Math.Abs(dir);

                /// Scroll through respawn points until finding a free one
                for (int i = 0; i < respawnPoints.Length && respawnPointWatched[value]; i += dir)
                {
                    //Debug.Log("Respawn point being watched, checking next one");
                    value += dir;

                    if (value < 0)
                    {
                        value = (value % respawnPoints.Length) + respawnPoints.Length;
                    }
                    else
                    {
                        value = value % respawnPoints.Length;
                    }
                }
                if (respawnPointWatched[value])
                {
                    Debug.LogError("Too many players for the amount of available respawn points");
                }

                /// Finally set the respawn point
                _currentRespawnPoint = value;

                respawnPointWatched[value] = true;

                transform.position = respawnPoints[value].transform.position;
                transform.rotation = respawnPoints[value].transform.rotation;
            }
        }

        public override IController controller
        {
            get
            {
                return _controller;
            }

            set
            {
                if (_controller != null)
                {
                    _controller.removePulseListener(Trigger.Respawn, respawnPulsed);
                    _controller.removePulseListener(Trigger.XMov, selectorAxisPulsed);
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.addPulseListener(Trigger.Respawn, respawnPulsed);
                    _controller.addPulseListener(Trigger.XMov, selectorAxisPulsed);
                }
            }
        }

        public static void findRespawnPoints()
        {
            respawnPoints = GameObject.FindGameObjectsWithTag(respawnTag);
            respawnPointWatched = new bool[respawnPoints.Length];

            if (respawnPoints.Length == 0)
            {
                Debug.LogError("No respawn points found");
            }
        }
        
        public void OnLevelWasLoaded()
        {
            if (lastLevelIndex != Application.loadedLevel)
                findRespawnPoints();
        }

        public override void Awake()
        {
            base.Awake();

            if (lastLevelIndex != Application.loadedLevel)
                findRespawnPoints();
        }

        public void Start()
        {
            currentRespawnPoint = 0;
        }

        public void respawnPulsed(float newValue, float difference)
        {
            if (difference > 0)
            {
                onRespawn(transform.position, transform.rotation);

                viewpoint.destroy();
                controller = null;
                Destroy(gameObject);
            }
        }

        public void selectorAxisPulsed(float newValue, float difference)
        {
            if (difference > 0 && newValue > 0)
                currentRespawnPoint++;

            else if (difference < 0 && newValue < 0)
                currentRespawnPoint--;
        }
    }
}
