using UnityEngine;

namespace Assets.october
{
    class IngameTestSetup : MonoBehaviour
    {
        public void Start()
        {
            IngameManager.startGame(1);
        }
    }
}
