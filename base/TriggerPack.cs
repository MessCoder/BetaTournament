using System;
using System.Text;

using UnityEngine;

namespace Assets.october
{
    [Serializable]
    public class TriggerPack : MonoBehaviour, ITriggerNotifier
    {
        public static readonly float deadZone = 0.1f;

        public static readonly Trigger[] allTriggers = (Trigger[])Enum.GetValues(typeof(Trigger));
        public static readonly int length = allTriggers.Length;

        [SerializeField]
        private float[] lastTriggerValues = new float[length];
        private float[] lastTriggerChanges = new float[length];
        private int[] lastPulseDirections = new int[length];

        private TriggerReaction[] onUpdate = new TriggerReaction[length];
        private TriggerReaction[] onPulse = new TriggerReaction[length];

        [SerializeField]
        public event VectorUpdateReaction onMovVectorUpdate;
        public event VectorUpdateReaction onRotVectorUpdate;

        public void addUpdateListener(Trigger trigger, TriggerReaction listener)
        {
            onUpdate[(int)trigger] += listener;
        }
        public void removeUpdateListener(Trigger trigger, TriggerReaction listener)
        {
            onUpdate[(int)trigger] -= listener;
        }
        public void addPulseListener(Trigger trigger, TriggerReaction listener)
        {
            onPulse[(int)trigger] += listener;
        }
        public void removePulseListener(Trigger trigger, TriggerReaction listener)
        {
            onPulse[(int)trigger] -= listener;
        }

        public void set(Trigger trigger, float newValue)
        {
            if (newValue != get(trigger))
            {
                if (trigger == Trigger.Escape)
                    Debug.Log(newValue);

                float difference = newValue - get(trigger);

                if (onUpdate[(int)trigger] != null)
                    onUpdate[(int)trigger](newValue, difference);

                lastTriggerChanges[(int)trigger] = difference;

                int pulseDirection = 0;

                if (newValue > 0 && difference > 0)
                    pulseDirection = 1;
                else if (newValue < 0 && difference < 0)
                    pulseDirection = -1;

                if (getLastPulse(trigger) != pulseDirection)
                {
                    if (onPulse[(int)trigger] != null)
                        onPulse[(int)trigger](newValue, difference);

                    lastPulseDirections[(int)trigger] = pulseDirection;
                }
            }

            lastTriggerValues[(int)trigger] = newValue;
        }

        public void setMovVector(Vector3 movementVector)
        {
            set(Trigger.XMov, movementVector.x);
            set(Trigger.YMov, movementVector.y);
            set(Trigger.ZMov, movementVector.z);

            if (onMovVectorUpdate != null)
                onMovVectorUpdate(movementVector);
        }

        public void setRotVector(Vector3 rotationVector)
        {
            set(Trigger.XRot, rotationVector.x);
            set(Trigger.YRot, rotationVector.y);
            set(Trigger.ZRot, rotationVector.z);

            if (onMovVectorUpdate != null)
                onRotVectorUpdate(rotationVector);
        }

        public float get(Trigger trigger)
        {
            return lastTriggerValues[(int)trigger];
        }
        
        public float getLastChange(Trigger trigger)
        {
            return lastTriggerChanges[(int)trigger];
        }

        public int getLastPulse(Trigger trigger)
        {
            return lastPulseDirections[(int)trigger];
        }

        public Vector3 getMovementVector()
        {
            return new Vector3(
                get(Trigger.XMov),
                get(Trigger.YMov),
                get(Trigger.ZMov)
                );
        }

        public Vector3 getRotationVector()
        {
            return new Vector3(
                get(Trigger.XRot),
                get(Trigger.YRot),
                get(Trigger.ZRot)
                );
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            for (int i = 0; i < lastTriggerValues.Length - 1; i++)
            {
                builder.Append(lastTriggerValues[i]);
                builder.Append(", ");
            }

            if (lastTriggerValues.Length != 0)
            {
                builder.Append(lastTriggerValues[lastTriggerValues.Length - 1]);
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}
