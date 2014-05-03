namespace LoupCorne.Framework
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("SimProperty")]
    public class SimProperty : ICloneable
    {
        [XmlAttribute]
        public string Name
        {
            get;
            internal set;
        }

        [XmlAttribute]
        public double BaseValue
        {
            get;
            set;
        }

        [XmlIgnore]
        public double Value
        {
            get;
            internal set;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}