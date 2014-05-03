namespace LoupCorne.Framework
{
    using System;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot("SimModifier")]
    public class SimModifier
    {
        [XmlAttribute("Operator")]
        public string StringOperator
        {
            get;
            set;
        }

        [XmlIgnore]
        public Sim.Operator Operator
        {
            get
            {
                return (Sim.Operator)Enum.Parse(typeof(Sim.Operator), this.StringOperator);
            }
        }

        [XmlAttribute]
        public string Target
        {
            get;
            set;
        }

        [XmlAttribute]
        public double Value
        {
            get;
            set;
        }

        public void Apply(SimObject simObject)
        {
            SimProperty simProperty = simObject.Properties.SingleOrDefault(p => p.Name == this.Target);
            if (simProperty == null)
            {
                return;
            }

            switch (this.Operator)
            {
                case Sim.Operator.Addition:
                    simProperty.Value += this.Value;
                    break;

                case Sim.Operator.Product:
                    simProperty.Value *= this.Value;
                    break;

                case Sim.Operator.Percent:
                    simProperty.Value += simProperty.BaseValue * this.Value;
                    break;
            }
        }
    }
}