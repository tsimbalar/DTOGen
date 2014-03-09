using System;
using System.Linq;

namespace DTOgen
{
    class UnderscoreTypeNamingConventions : ITypeNamingConventions
    {
        public string FormatAnonymousPropertyTypeName(string propertyName, string containingTypeName = null)
        {

            var capitalizedPropertyName =
                new string(
                    propertyName.Select((c, i) => i > 0 ? c : c.ToString().ToUpperInvariant()[0]).ToArray());
            if (String.IsNullOrEmpty(containingTypeName)) return capitalizedPropertyName;
            return String.Join("_", containingTypeName, capitalizedPropertyName);
        }

        public string FormatCollectionPropertyItemTypeName(string collectionPropertyName, string containingTypeName)
        {
            return FormatAnonymousPropertyTypeName(collectionPropertyName + "_Item", containingTypeName);
        }
    }
}