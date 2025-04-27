using System;
using Unity.VisualScripting;

// namespace Unity.VisualScripting
// {
// #if MODULE_PHYSICS_2D_EXISTS
    /// <summary>
    /// Called when an incoming collider makes contact with this object's collider.
    /// </summary>
     
     [UnitTitle("On Collision Enter 2D")]
     [TypeIcon(typeof(Unity.VisualScripting.OnCollisionEnter2D))]

    public sealed class OnCollisionEnter2D : CollisionEvent2DUnit
    {
        public override Type MessageListenerType => typeof(UnityOnCollisionEnter2DMessageListener);
        protected override string hookName => EventHooks.OnCollisionEnter2D;
    }
// #endif
// }
