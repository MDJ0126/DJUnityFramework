using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game
{
    [Serializable]
    public class AimTarget
    {
        public Transform target;
        public Rig AimRig;
    }
}