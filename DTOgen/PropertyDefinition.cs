namespace DTOgen
{
    public class PropertyDefinition
    {
        private readonly string _name;
        private readonly string _type;
        private readonly bool _isCollection;

        public PropertyDefinition(string name, string type, bool isCollection = false)
        {
            _name = name;
            _type = type;
            _isCollection = isCollection;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Type
        {
            get { return _type; }
        }

        public bool IsCollection
        {
            get { return _isCollection; }
        }
    }
}