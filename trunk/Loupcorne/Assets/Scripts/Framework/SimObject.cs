namespace LoupCorne.Framework
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SimObject : DatatableElement, ICloneable
    {
        [XmlAttribute]
        public string Class
        {
            get;
            set;
        }

        [XmlElement("SimProperty")]
        public List<SimProperty> Properties
        {
            get;
            set;
        }

        [XmlIgnore]
        public List<SimDescriptor> Descriptors
        {
            get;
            set;
        }

        public SimObject()
        {
            this.Properties = new List<SimProperty>();
            this.Descriptors = new List<SimDescriptor>();
        }

        public double GetPropertyValue(string name)
        {
            return this.Properties.SingleOrDefault(p => p.Name == name).Value;
        }

        public void AddDescriptor(SimDescriptor descriptor)
        {
            this.Descriptors.Add(descriptor);
        }

        public void RemoveDescriptor(SimDescriptor descriptor)
        {
            if(this.Descriptors.Contains(descriptor))
            {
                this.Descriptors.Remove(descriptor);
            }
        }

        public void RemoveDescriptorByName(string name)
        {
            this.Descriptors.RemoveAll(d => d.Name == name);
        }

        public void RemoveDescriptorByClass(string @class)
        {
            this.Descriptors.RemoveAll(d => d.Class == @class);
        }

        public void Refresh()
        {
            foreach (SimProperty property in Properties)
            {
                property.Value = property.BaseValue;
            }

            List<SimModifier> sortedModifiers = new List<SimModifier>();
            foreach (SimDescriptor descriptor in Descriptors)
            {
                sortedModifiers.AddRange(descriptor.Modifiers);
            }

            sortedModifiers.Sort((a, b) => a.Operator.CompareTo(b.Operator));
            foreach (SimModifier modifier in sortedModifiers)
            {
                modifier.Apply(this);
            }
        }

        public object Clone()
        {
            SimObject clone = this.MemberwiseClone() as SimObject;
            clone.Descriptors = new List<SimDescriptor>();
            clone.Properties = new List<SimProperty>();
            foreach (SimProperty simProperty in Properties)
            {
                clone.Properties.Add(simProperty.Clone() as SimProperty);
            }

            return clone;
        }
    }
}