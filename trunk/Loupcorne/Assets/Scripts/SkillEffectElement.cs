using System.Xml.Serialization;

public class SkillEffectElement : LoupCorne.Framework.DatatableElement
{
    [XmlAttribute]
    public string Type
    {
        get;
        protected set;
    }

    [XmlElement]
    public string Gfx
    {
        get;
        protected set;
    }

    [XmlElement]
    public string Sfx
    {
        get;
        protected set;
    }

    [XmlElement]
    public float Speed
    {
        get;
        protected set;
    }

    [XmlElement]
    public float Damage
    {
        get;
        protected set;
    }

    [XmlElement]
    public float Range
    {
        get;
        protected set;
    }

    [XmlElement]
    public bool DestroyOnHitSuccess
    {
        get;
        protected set;
    }

    [XmlElement]
    public bool DestroyOnHitFailure
    {
        get;
        protected set;
    }

    [XmlAttribute]
    public string Prefab
    {
        get;
        protected set;
    }

    [XmlElement]
    public float Duration
    {
        get;
        private set;
    }

    [XmlElement]
    public float Radius
    {
        get;
        protected set;
    }

    [XmlElement]
    public float AlignementGainOnUse
    {
        get;
        protected set;
    }

    [XmlElement]
    public string BuffDescriptor
    {
        get;
        set;
    }
}