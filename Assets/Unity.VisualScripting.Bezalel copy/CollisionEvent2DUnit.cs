using UnityEngine;
using Unity.VisualScripting;

// namespace Unity.VisualScripting
// {
// #if MODULE_PHYSICS_2D_EXISTS
[UnitCategory("Bezalel/Physics")]
    public abstract class CollisionEvent2DUnit : GameObjectEventUnit<Collision2D>
    {
        
        [DoNotSerialize]
        public ValueOutput gameObject { get; private set; }
        
        [DoNotSerialize]
        public ValueOutput name { get; private set; }
    
        [DoNotSerialize]
        public ValueOutput tag { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            gameObject = ValueOutput<GameObject>("Object");
            name = ValueOutput<string>("Name");
            tag = ValueOutput<string>("Tag");
        }

        protected override void AssignArguments(Flow flow, Collision2D other)
        {
            flow.SetValue(gameObject, other.collider.gameObject);
            flow.SetValue(name, other.collider.name);
            flow.SetValue(tag, other.collider.tag);
        }
        
    }
// #endif
// }
