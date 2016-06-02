using System;
using UnityEngine;

namespace Assets.october.flints
{
    /// <summary>
    /// Initializes a game session in a game map scene
    /// </summary>
    public class StartGameFlint : Flint
    {
        // Number of players on preview execution
        [SerializeField]
        private int numPlayers = 1;
        
        protected override void NormalSetup()
        {
            // This initialization is performed by the GameManager
            // in normal executions
            //
            // This class only exists in order to make preview plays 
            // possible
        }

        protected override void PreviewSetup()
        {
            IngameManager.startGame(numPlayers);
        }
    }
}
