using System;
using UnityEngine;

namespace Assets.october.flints
{
    public class StartGameFlint : Flint
    {
        // Number of players on preview execution
        [SerializeField]
        private int numPlayers = 1;
        
        protected override void NormalSetup()
        {
            // To initialize a game you need information about it
            // (number of players, for example)
            // you must request the initialization to GameManager.
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
