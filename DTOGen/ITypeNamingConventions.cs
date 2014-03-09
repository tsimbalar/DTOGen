namespace DTOgen
{
    public interface ITypeNamingConventions
    {
        string FormatAnonymousPropertyTypeName(string propertyName, string containingTypeName = null);
        string FormatCollectionPropertyItemTypeName(string collectionPropertyName, string containingTypeName);
    }
}