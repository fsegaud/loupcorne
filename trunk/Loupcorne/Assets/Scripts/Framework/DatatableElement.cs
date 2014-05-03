namespace LoupCorne.Framework
{
    using System.Xml.Serialization;

    public abstract class DatatableElement
    {
        [XmlAttribute("Name")]
        public string Name
        {
            get;
            protected set;
        }

        [XmlElement]
        public string Title
        {
            get;
            protected set;
        }

        [XmlElement]
        public string Description
        {
            get;
            protected set;
        }

        [XmlElement]
        public string Tags
        {
            get;
            protected set;
        }

        [XmlElement]
        public string Descriptor
        {
            get;
            set;
        }
    }
}