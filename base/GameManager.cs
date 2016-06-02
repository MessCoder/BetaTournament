using Assets.git.controllables;

using UnityEngine;

namespace Assets.git
{
    // This singleton can't be accessed before GameManager's Awake method is called
    // That should be the first Awake method to be called, you can edit the order of 
    // execution of scripts in "project preferences".
    //
    // Only an instance of GameManager should exist at all moments.
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
 
        [SerializeField]
        private User[] _defaultUsers = new User[] {
            new User("P1"),
            new User("P2")
        };
        public static User[] users
        {
            get
            {
                return instance._defaultUsers;
            }
        }
        
        [SerializeField]
        private string[] _gameMapNames = null;
        public static string[] gameMapNames
        {
            get { return instance._gameMapNames; }
        }

        [SerializeField]
        private string _mainMenuName = "TestMenu";
        public static string mainMenuName
        {
            get { return instance._mainMenuName; }
        }

        private static bool startingGame;
        private static int startingGameNumberOfPlayers;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(instance);
            }
            else
            {
                if (instance != this)
                {
                    Debug.LogError("There is more than one instance of GameManager in the scene this is not supported");
                    Destroy(gameObject);
                }
            }
        }

        public static void loadMenu()
        {
            Time.timeScale = 1;

            Application.LoadLevel(mainMenuName);
        }

        public static void startGame(int gameMapIndex, int numPlayers)
        {
            if (numPlayers > users.Length)
            {
                Debug.LogError("number of players limted to number of default users: " + users.Length);
                numPlayers = users.Length;
            }

            Application.LoadLevel(gameMapIndex);
            
            startingGame = true;
            startingGameNumberOfPlayers = numPlayers;
        }

        public static void startGame(string gameMapName, int numPlayers)
        {
            if (numPlayers > users.Length)
            {
                Debug.LogError("number of players limted to number of default users: " + users.Length);
                numPlayers = users.Length;
            }

            Application.LoadLevel(gameMapName);

            startingGame = true;
            startingGameNumberOfPlayers = numPlayers;
        }

        private static void startGamePostLevelLoaded()
        {
            IngameManager.startGame(startingGameNumberOfPlayers);

            // Important!!!
            startingGame = false;
        }

        public void OnLevelWasLoaded()
        {
            if (startingGame)
                startGamePostLevelLoaded();
        }
    }
}
