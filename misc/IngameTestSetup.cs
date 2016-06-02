using UnityEngine;

namespace Assets.git
{
    class IngameTestSetup : MonoBehaviour
    {
        public void Start()
        {
            IngameManager.startGame(1);
        }
    }
}
