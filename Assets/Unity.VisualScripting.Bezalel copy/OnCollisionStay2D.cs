using System;
using Unity.VisualScripting;

// namespace Unity.VisualScripting
// {
// #if MODULE_PHYSICS_2D_EXISTS
    /// <summary>
    /// Called each frame where a collider on another object is touching this object's collider.
    /// </summary>

    [UnitTitle("On Collision Stay 2D")]
    [TypeIcon(typeof(Unity.VisualScripting.OnCollisionStay2D))]
    public sealed class OnCollisionStay2D : CollisionEvent2DUnit
    {
        public override Type MessageListenerType => typeof(UnityOnCollisionStay2DMessageListener);
        protected override string hookName => EventHooks.OnCollisionStay2D;
    }
// #endif
// }
