using Assets.october.controllers;
using System;
using UnityEngine;

namespace Assets.october
{
    [Serializable]
    public class User : IPlayer
    {
        public User(string name)
        {
            this.name = name;

            configuration = getDefaultConfiguration(name);
        }

        private static Configuration getDefaultConfiguration(string userIdentifier)
        {
            Trigger[] miscTriggers = new Trigger[]{
                Trigger.Escape,
                Trigger.Respawn,
                Trigger.ShootLongstand,
                Trigger.ShootShortstand,
                Trigger.Submit
                };
            
            Configuration.TriggerAxis[] miscTriggerAxes = new Configuration.TriggerAxis[miscTriggers.Length];
            
            for (int i = 0; i < miscTriggers.Length; i++)
            {
                miscTriggerAxes[i] = new Configuration.TriggerAxis(
                    miscTriggers[i], 
                    userIdentifier + Enum.GetName(typeof(Trigger), miscTriggers[i])
                    );
            }

            string xMovAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.XMov);
            string yMovAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.YMov);
            string zMovAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.ZMov);

            Configuration.VectorAxes movVectorAxes = new Configuration.VectorAxes(xMovAxis, yMovAxis, zMovAxis);

            string xRotAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.XRot);
            string yRotAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.YRot);
            string zRotAxis = userIdentifier + Enum.GetName(typeof(Trigger), Trigger.ZRot);

            Configuration.VectorAxes rotVectorAxes = new Configuration.VectorAxes(zRotAxis, yRotAxis, zRotAxis);

            return new Configuration(miscTriggerAxes, movVectorAxes, rotVectorAxes);
        }

        [SerializeField]
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }

        private UserController controller;
        public UserController getController()
        {
            if (controller == null)
            {
                controller = UserController.fromUser(this);
            }
            return controller;
        }

        [SerializeField]
        private Configuration configuration;
        public Configuration getConfiguration()
        {
            return configuration;
        }

        [Serializable]
        public class Configuration
        {
            [Serializable]
            public class VectorAxes
            {
                [SerializeField]
                private string xAxis;
                [SerializeField]
                private string yAxis;
                [SerializeField]
                private string zAxis;

                public VectorAxes(string xAxis, string yAxis, string zAxis)
                {
                    this.xAxis = xAxis;
                    this.yAxis = yAxis;
                    this.zAxis = zAxis;
                }

                public Vector3 getInputVector()
                {
                    Vector3 result = new Vector3();

                    if (Time.timeScale != 0)
                    {
                        if (xAxis != null)
                            result.x = Input.GetAxis(xAxis);

                        if (yAxis != null)
                            result.y = Input.GetAxis(yAxis);

                        if (zAxis != null)
                            result.z = Input.GetAxis(zAxis);
                    }
                    else
                    {
                        if (xAxis != null)
                            result.x = Input.GetAxisRaw(xAxis);

                        if (yAxis != null)
                            result.y = Input.GetAxisRaw(yAxis);

                        if (zAxis != null)
                            result.z = Input.GetAxisRaw(zAxis);
                    }

                    return result;
                }
            }

            [Serializable]
            public struct TriggerAxis
            {
                public TriggerAxis(Trigger trigger, string[] axes)
                {
                    this.trigger = trigger;
                    this.axes = axes;
                }

                public TriggerAxis(Trigger trigger, string axis)
                {
                    this.trigger = trigger;
                    this.axes = new string[] { axis };
                }

                public Trigger trigger;
                public string[] axes;
            }

            public Configuration(TriggerAxis[] miscTriggerAxes, VectorAxes movVectorAxes, VectorAxes rotVectorAxes)
            {
                this.miscTriggerAxes = miscTriggerAxes;
                this.movVectorAxes = movVectorAxes;
                this.rotVectorAxes = rotVectorAxes;
            }

            public VectorAxes movVectorAxes;
            public VectorAxes rotVectorAxes;
            public TriggerAxis[] miscTriggerAxes;
        }

        public override string ToString()
        {
            return string.Format("User: {0}", name);
        }
    }
}
