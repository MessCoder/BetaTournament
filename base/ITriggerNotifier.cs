using UnityEngine;

namespace Assets.october
{
    public delegate void TriggerReaction(float newValue, float difference);
    public delegate void VectorUpdateReaction(Vector3 newValue);

    public interface ITriggerNotifier
    {
        event VectorUpdateReaction onMovVectorUpdate;
        event VectorUpdateReaction onRotVectorUpdate;

        void addUpdateListener(Trigger trigger, TriggerReaction listener);
        void removeUpdateListener(Trigger trigger, TriggerReaction listener);
        void addPulseListener(Trigger trigger, TriggerReaction listener);
        void removePulseListener(Trigger trigger, TriggerReaction listener);
    }
}