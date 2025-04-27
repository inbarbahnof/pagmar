using UnityEngine;
using Unity.VisualScripting;

// namespace Unity.VisualScripting
// {
// #if MODULE_PHYSICS_2D_EXISTS
[UnitCategory("Bezalel/Physics")]
    public abstract class TriggerEvent2DUnit : GameObjectEventUnit<Collider2D>
    {
        
        [DoNotSerialize]
        public ValueOutput name { get; private set; }
    
        [DoNotSerialize]
        public ValueOutput tag { get; private set; }    
        
        [DoNotSerialize]
        public ValueOutput gameObject { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            gameObject = ValueOutput<GameObject>("Object");
            name = ValueOutput<string>("Name");
            tag = ValueOutput<string>("Tag");
        }

        protected override void AssignArguments(Flow flow, Collider2D other)
        {
            flow.SetValue(gameObject, other.gameObject);
            flow.SetValue(name, other.name);
            flow.SetValue(tag, other.tag);
        }
    }
// #endif
// }
