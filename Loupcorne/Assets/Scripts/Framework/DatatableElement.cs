namespace LoupCorne.Framework
{
    using System.Xml.Serialization;

    public abstract class DatatableElement
    {
        [XmlAttribute("Name")]
        public string Name
        {
            get;
            private set;
        }
    }
}