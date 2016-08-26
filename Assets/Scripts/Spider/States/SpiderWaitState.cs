using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spider.States
{
    class SpiderWaitState : State
    {
        public override StateId StateId
        {
            get { return StateId.Wait; }
        }

        public override void Reason(GameObject self, GameObject player)
        {
            throw new NotImplementedException();
        }

        public override void Act(GameObject self, GameObject player)
        {
            throw new NotImplementedException();
        }
    }
}
