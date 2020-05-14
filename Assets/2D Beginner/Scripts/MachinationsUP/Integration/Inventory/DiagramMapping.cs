using System;
using System.Runtime.Serialization;
using MachinationsUP.GameEngineAPI.States;
using MachinationsUP.Integration.Elements;
using MachinationsUP.SyncAPI;

namespace MachinationsUP.Integration.Inventory
{
    /// <summary>
    /// Summarizes information about how a Machinations Game Object Property is mapped to the Diagram.
    /// Can be seen as the "coordinates" of a Game Object Property.
    /// See <see cref="MachinationsUP.Integration.Binder"/>
    /// </summary>
    [DataContract(Name = "MachinationsDiagramMapping", Namespace = "http://www.machinations.io")]
    public class DiagramMapping
    {

        private string _gameObjectName;

        /// <summary>
        /// The name of the Machinations Game Object that owns this property.
        /// See <see cref="MachinationsUP.Integration.GameObject.MachinationsGameObject"/>
        /// </summary>
        [DataMember()]
        public string GameObjectName
        {
            get => _gameObjectName;
            set => _gameObjectName = value;
        }

        private string _gameObjectPropertyName;

        /// <summary>
        /// The name of this Property.
        /// </summary>
        [DataMember()]
        public string GameObjectPropertyName
        {
            get => _gameObjectPropertyName;
            set => _gameObjectPropertyName = value;
        }

        private StatesAssociation _statesAssociation;

        /// <summary>
        /// The States Association where this Property applies.
        /// </summary>
        [DataMember()]
        public StatesAssociation StatesAssoc
        {
            get => _statesAssociation;
            set => _statesAssociation = value;
        }

        private int _diagramElementID;

        /// <summary>
        /// Machinations Back-end Element ID.
        /// </summary>
        [DataMember()]
        public int DiagramElementID
        {
            get => _diagramElementID;
            set => _diagramElementID = value;
        }

        /// <summary>
        /// How to handle situations when different values come from the Diagram.
        /// </summary>
        public OverwriteRules OvewriteRule { get; set; }

        /// <summary>
        /// A default ElementBase, in case no answer comes from the Machinations Diagram.
        /// </summary>
        public ElementBase DefaultElementBase { get; set; }

        /// <summary>
        /// The last value received from the Machinations back-end.
        /// </summary>
        [DataMember()]
        public ElementBase CachedElementBase { get; set; }

        override public string ToString ()
        {
            return "DiagramMapping for " + GameObjectName + "." + GameObjectPropertyName + "." +
                   (_statesAssociation != null ? _statesAssociation.Title : "N/A") +
                   " bound to DiagramID: " + DiagramElementID;
        }

    }
}
