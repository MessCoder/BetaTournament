using UnityEngine;

namespace Assets.git.controllers
{
    public class UserController : Controller
    {
        public User.Configuration userConfig{ get; set; }
        private User _user;
        private bool timeFrozen;

        public User user
        {
            get
            {
                return _user;
            }
            private set
            {
                _user = value;

                if (_user != null)
                    userConfig = user.getConfiguration();
            }
        }

        public static UserController fromUser(User user)
        {
            GameObject hostGameObject = new GameObject(user.name + "'s controller");
            hostGameObject.transform.parent = GameManager.instance.transform;

            UserController newController = hostGameObject.AddComponent<UserController>();
            newController.user = user;

            return newController;
        }

        private void refreshTriggers()
        {
            foreach (User.Configuration.TriggerAxis triggerAxes in userConfig.miscTriggerAxes)
            {
                float triggerValue = 0;

                if (Time.timeScale != 0)
                    foreach (string axis in triggerAxes.axes)
                    {
                        triggerValue += Input.GetAxis(axis);
                    }
                else
                    foreach (string axis in triggerAxes.axes)
                    {
                        triggerValue += Input.GetAxisRaw(axis);
                    }

                set(triggerAxes.trigger, triggerValue);
            }

            setMovVector(userConfig.movVectorAxes.getInputVector());
            setRotVector(userConfig.rotVectorAxes.getInputVector());
        }

        void Update()
        {
            if (Time.timeScale == 0)
                timeFrozen = true;
            else if (timeFrozen)
            {
                timeFrozen = false;
                Input.ResetInputAxes();
            }
            
            refreshTriggers();
        }
    }
}
