using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.git.viewpoints
{
    [RequireComponent(typeof(Camera))]
    public class Hud : Viewpoint
    {
        private static int firstHudLayer = 10;
        private static int hudLayersAmount = 5;

        private static bool[] layerIsBooked = new bool[hudLayersAmount];
        
        private int _hudLayer = -1;
        private int hudLayer
        {
            get
            {
                return _hudLayer;
            }
            set
            {
                // Undoes possible previous inserting of hud layers into the camera's cullingmask
                if (_hudLayer != -1)
                {
                    camera.cullingMask = camera.cullingMask & ~(1 << _hudLayer);
                }

                _hudLayer = value;

                if (_hudLayer != -1)
                {
                    // Inserts the hud layer of this Hud into the camera's cullingmask
                    camera.cullingMask = camera.cullingMask | (1 << _hudLayer);

                    // Updates the layer of added HudComponents
                    foreach (HudComponent hudComponent in hudComponents)
                        hudComponent.setLayer(_hudLayer);
                }
            }
        }

        [SerializeField]
        private HudComponent[] inspectorHudComponents = new HudComponent[0];
        private List<HudComponent> hudComponents = new List<HudComponent>();

        private void bookLayer()
        {
            for (int i = 0; i < hudLayersAmount; i++)
            {
                if (!layerIsBooked[i])
                {
                    layerIsBooked[i] = true;
                    hudLayer = firstHudLayer + i;

                    break;
                }
            }

            if (hudLayer == -1)
                Debug.LogError("Not enough hudLayers");
        }

        private void freeLayer()
        {
            layerIsBooked[hudLayer - firstHudLayer] = false;
            hudLayer = -1;
        }

        public override void Awake()
        {
            base.Awake();

            bookLayer();
        }

        public void Start()
        {
            foreach(HudComponent inspectorHudComponent in inspectorHudComponents)
            {
                AddHudComponent(inspectorHudComponent);
            }
        }

        public void OnDestroy()
        {
            freeLayer();
        }

        public void AddHudComponent(HudComponent hudComponent)
        {
            if (hudLayer != -1)
                hudComponent.setLayer(hudLayer);

            hudComponent.hud = this;
            hudComponent.transform.SetParent(transform, false);

            hudComponents.Add(hudComponent);
        }
    }
}
