using System;

using Assets.october.controllables;

using UnityEngine;
using Assets.october.controllables.deadControllables;

namespace Assets.october
{
    public class IngameManager : MonoBehaviour
    {
        private static IngameManager _instance;
        public static IngameManager instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        [SerializeField]
        private IngameMenu _defaultIngameMenuPrefab = null;
        private static IngameMenu defaultIngamePrefab
        {
            get { return instance._defaultIngameMenuPrefab; }
        }
        [SerializeField]
        private Character _defaultCharacterPrefab = null;
        private static Character defaultCharacterPrefab
        {
            get { return instance._defaultCharacterPrefab;  }
        }
        [SerializeField]
        private DeadControllable _defaultDeadControllablePrefab = null;
        private static DeadControllable defaultDeadControllablePrefab
        {
            get { return instance._defaultDeadControllablePrefab; }
        }

        private static Rect[] userViewrects;
        private static IngameMenu ingameMenu;
        private static Controllable[] playerControllables;
        private static TriggerReaction[] pulseUserControllersListeners;

        private static bool _isGameStarted;
        public static bool isGameRunning
        {
            get { return _isGameStarted; }
            private set { _isGameStarted = value; }
        }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                setupListeners();
            }
            else
            {
                if (instance != this)
                {
                    Debug.LogError("There is more than one IngameManager instance. deactivating the new one.");
                    enabled = false;
                }
            }
        }

        public static void startGame(int numberOfPlayers)
        {
            if (isGameRunning)
                endGame();

            isGameRunning = true;

            int maxPlayers = GameManager.users.Length;
            if (numberOfPlayers > maxPlayers)
            {
                Debug.LogError("Too many players. fixing to GameManager default users - " + maxPlayers);
                numberOfPlayers = maxPlayers;
            }

            setupIngameMenu();
            userViewrects = getSplitRects(numberOfPlayers);
            playerControllables = new Controllable[numberOfPlayers];

            attach();

            for (int i = 0; i < numberOfPlayers; i++)
                startDeadScreen(i);
        }

        public static void recordDamage(IPlayer dealer, IPlayer receiver, float damage, bool mortal)
        {
            string message;

            if (mortal)
                message = string.Format("{0} has killed {1}", dealer, receiver);
            else
                message = string.Format("{0} dealt {2} points of damage to {1}", dealer, receiver, damage);

            Debug.Log(message);
        }

        public static void endGame()
        {
            if (!isGameRunning)
            {
                Debug.LogError("Can't end game when it hasn't been started");
                return;
            }

            dettach();
        }

        internal static void exitToMenu()
        {
            endGame();

            GameManager.loadMenu();
        }

        private static void setupListeners()
        {
            pulseUserControllersListeners = new TriggerReaction[GameManager.users.Length];

            for (int i = 0; i < pulseUserControllersListeners.Length; i++)
            {
                pulseUserControllersListeners[i] = new TriggerReaction(getEscapePulseListener(i));
            }
        }

        private static void attach()
        {
            for (int i = 0; i < pulseUserControllersListeners.Length; i++)
            {
                GameManager.users[i].getController().addPulseListener(
                    Trigger.Escape,
                    pulseUserControllersListeners[i]
                    );
            }
        }

        private static void dettach()
        {
            for (int i = 0; i < pulseUserControllersListeners.Length; i++)
            {
                GameManager.users[i].getController().removePulseListener(
                    Trigger.Escape,
                    pulseUserControllersListeners[i]
                    );
            }
        }

        private static TriggerReaction getEscapePulseListener(int invokerUserIndex)
        {
            return (float value, float dir) =>
                {  if (dir > 0) openIngameMenu(invokerUserIndex); };
        }

        private static void attachUserControllables()
        {
            for (int i = 0; i < playerControllables.Length; i++)
            {
                playerControllables[i].setUIResponding(true);
                playerControllables[i].controller = GameManager.users[i].getController();
            }
        }

        private static void dettachUserControllables()
        {
            for (int i = 0; i < playerControllables.Length; i++)
            {
                playerControllables[i].setUIResponding(false);
                playerControllables[i].controller = null;
            }
        }

        private static void setupIngameMenu()
        {
            if (ingameMenu == null && !ingameMenu)
            {
                ingameMenu = Instantiate(defaultIngamePrefab);
                ingameMenu.onReturnToGame += closeIngameMenu;
            }
        }

        private static void openIngameMenu(int userIndex)
        {
            Debug.Log("Opening ingame menu");

            Time.timeScale = 0;

            dettachUserControllables();
            dettach();

            ingameMenu.controller = GameManager.users[userIndex].getController();
            ingameMenu.setUser(GameManager.users[userIndex]);

            ingameMenu.setViewrect(new Rect(0,0,1,1));
            ingameMenu.setRendering(true);
            ingameMenu.setUIResponding(true);
        }

        private static void closeIngameMenu()
        {
            Debug.Log("Closing ingame menu");

            Time.timeScale = 1;

            ingameMenu.controller = null;
            ingameMenu.setRendering(false);
            ingameMenu.setUIResponding(false);

            if (isGameRunning)
            {
                attachUserControllables();
                attach();
            }
        }

        private static void startDeadScreen(int userIndex)
        {
            DeadControllable deadControllable = getDeadControllable(userIndex);

            deadControllable.setViewrect(userViewrects[userIndex]);
            deadControllable.setRendering(true);
            deadControllable.setUIResponding(true);

            deadControllable.controller = GameManager.users[userIndex].getController();
            playerControllables[userIndex] = deadControllable;

            deadControllable.onRespawn += (Vector3 pos, Quaternion rot) => { spawn(userIndex, pos, rot); };

        }

        private static void spawn(int userIndex, Vector3 position, Quaternion rotation)
        {
            Character character = getCharacter(userIndex);

            character.transform.position = position;
            character.transform.rotation = rotation;

            character.setViewrect(userViewrects[userIndex]);
            character.setRendering(true);
            character.setUIResponding(true);

            character.controller = GameManager.users[userIndex].getController();

            character.onCease += () => { startDeadScreen(userIndex); };
        }

        private static Character getCharacter(int userIndex)
        {
            Character result = Instantiate(defaultCharacterPrefab);
            result.user = GameManager.users[userIndex];
            playerControllables[userIndex] = result;

            return result;
        }

        private static DeadControllable getDeadControllable(int userIndex)
        {
            var result = Instantiate(defaultDeadControllablePrefab);
            playerControllables[userIndex] = result;

            return result;
        }

        private static Rect[] getSplitRects(int num)
        {
            if (num <= 0)
                Debug.LogError("Invalid number of split rects " + num);

            Rect[] result = new Rect[num];

            switch (num)
            {
                case 1:
                    result[0] = new Rect(0, 0, 1, 1);
                    break;
                case 2:
                    result[0] = new Rect(0f, 0.5f, 1f, 0.5f);
                    result[1] = new Rect(0f, 0f, 1f, 0.5f);
                    break;
                case 3:
                    result[0] = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    result[1] = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    result[2] = new Rect(0f, 0f, 1f, 0.5f);
                    break;
                case 4:
                    result[0] = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    result[1] = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    result[2] = new Rect(0f, 0f, 0.5f, 0.5f);
                    result[3] = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    break;
                default:
                    throw new NotImplementedException("No rect sizes for more than 4 players set");
            }

            return result;
        }
    }
}
