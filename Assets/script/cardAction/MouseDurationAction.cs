using Assets.script.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.script.cardAction
{
    class MouseDurationAction : MonoBehaviour
    {
        private long duration;// -1表示上一次在该部件外
        public long durationLimit = (long)1 * 10000000; // 1s延迟就触发
        public delegate void EventHandler(GameObject e);
        public event EventHandler mouseOverEvent;

        // Use this for initialization
        void Start()
        {
            this.duration = -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                if (this.duration == -1)
                {
                    this.duration = DateTime.Now.Ticks;
                }else
                {
                    if (DateTime.Now.Ticks > this.duration + this.durationLimit)
                    {
                        if (this.mouseOverEvent != null)
                        {
                            this.mouseOverEvent(this.gameObject);
                        }
                    }
                }
            }else
            {
                this.duration = -1;
            }
        }
        
    }
}
