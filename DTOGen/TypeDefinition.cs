using System.Collections.Generic;
using System.Linq;

namespace DTOgen
{
    public class TypeDefinition
    {
        private readonly string _name;
        private readonly List<PropertyDefinition> _props;

        public TypeDefinition(string name, IEnumerable<PropertyDefinition> props)
        {
            _name = name;
            _props = props.ToList();
        }

        public TypeDefinition(string typeName) :
            this(typeName, new List<PropertyDefinition>())
        {

        }

        public string TypeName
        {
            get { return _name; }
        }

        public List<PropertyDefinition> Properties
        {
            get { return _props; }
        }

        public void AddProperty(string name, string type)
        {
            _props.Add(new PropertyDefinition(name, type));
        }

        public void AddCollectionProperty(string propName, string collectionItemType)
        {
            _props.Add(new PropertyDefinition(propName, collectionItemType, isCollection: true));
        }
    }
}