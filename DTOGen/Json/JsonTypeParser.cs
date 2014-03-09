using System;
using System.Collections.Generic;
using DTOgen;
using Newtonsoft.Json.Linq;

namespace DTOGen.Json
{
    public class JsonTypeParser
    {
        private readonly ITypeNamingConventions _namingConventions;

        public JsonTypeParser()
            : this(new UnderscoreTypeNamingConventions())
        {

        }

        public JsonTypeParser(ITypeNamingConventions namingConventions)
        {
            if (namingConventions == null) throw new ArgumentNullException("namingConventions");
            _namingConventions = namingConventions;
        }

        public IEnumerable<TypeDefinition> LoadTypes(string json)
        {
            var root = JObject.Parse(json);
            var typeDefinitionsAccumulator = new List<TypeDefinition>();
            foreach (var typeDeclaration in root.Properties())
            {
                var childTypeName = _namingConventions.FormatAnonymousPropertyTypeName(typeDeclaration.Name);
                BuildRecursiveTypeDefinition(typeDefinitionsAccumulator, childTypeName, (JObject)typeDeclaration.Value);
            }

            return typeDefinitionsAccumulator;
        }

        private void BuildRecursiveTypeDefinition(List<TypeDefinition> typesToDeclareAccumulator, string newTypeName, JObject typeJObject)
        {
            var myTypeDef = new TypeDefinition(newTypeName);
            typesToDeclareAccumulator.Add(myTypeDef); // remember that a new type has to be generated
            // generate the necessary types for each of the members
            foreach (var prop in typeJObject.Properties())
            {
                var propName = prop.Name;
                var propValue = prop.Value;
                var propValueType = propValue.Type;
                switch (propValueType)
                {
                    case JTokenType.Object:
                        // is it the declaration of an anonymous type ? (json object representing members of the type)
                        var subObjectTypeDeclaration = (JObject)propValue;
                        var newType = _namingConventions.FormatAnonymousPropertyTypeName(propName, myTypeDef.TypeName);
                        BuildRecursiveTypeDefinition(typesToDeclareAccumulator, newType, subObjectTypeDeclaration);
                        myTypeDef.AddProperty(propName, newType);
                        break;
                    case JTokenType.Array:
                        // is it the declaration of a collection ? then structure is defined by the only element
                        var subObjectCollectionDeclaration = (JArray)propValue;
                        var arrayItemSpecimen = subObjectCollectionDeclaration[0];
                        string collectionItemTypeName = null;
                        if (arrayItemSpecimen.Type == JTokenType.Object)
                        {
                            var collectionItemTypeDeclaration = (JObject) arrayItemSpecimen;
                            collectionItemTypeName = _namingConventions.FormatCollectionPropertyItemTypeName(propName, myTypeDef.TypeName);
                            BuildRecursiveTypeDefinition(typesToDeclareAccumulator, collectionItemTypeName, collectionItemTypeDeclaration);
                        }else if (arrayItemSpecimen.Type == JTokenType.String)
                        {
                            collectionItemTypeName = arrayItemSpecimen.Value<string>();
                        }
                        else
                        {
                            throw new InvalidOperationException(String.Format("Cannot do anything with the content of first item in the collection stored in property  {0} contained in {1} because it is of type {2}", propName, myTypeDef.TypeName, arrayItemSpecimen.Type));
                        }
                        myTypeDef.AddCollectionProperty(propName, collectionItemTypeName);
                        break;
                    case JTokenType.String:
                        // just a single item, consider it's a type name
                        myTypeDef.AddProperty(propName, propValue.Value<string>());
                        break;
                    default:
                         throw new InvalidOperationException(string.Format("Cannot do anything with the value of property {0} contained in {1} which is of type {2}", propName, myTypeDef.TypeName, propValue.Type));
                }
            }
        }
    }
}