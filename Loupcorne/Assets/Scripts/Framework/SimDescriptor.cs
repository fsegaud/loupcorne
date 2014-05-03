namespace LoupCorne.Framework
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SimDescriptor : DatatableElement
    {
        public SimDescriptor()
        {
            this.Modifiers = new List<SimModifier>();
        }

        [XmlAttribute]
        public string Class
        {
            get;
            set;
        }

        [XmlElement("SimModifier")]
        public List<SimModifier> Modifiers
        {
            get;
            set;
        }
    }
}