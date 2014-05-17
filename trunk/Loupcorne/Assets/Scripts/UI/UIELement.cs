
public class UIELement : LoupCorne.Framework.DatatableElement
{
    [System.Xml.Serialization.XmlElement]
    public string Title
    {
        get;
        protected set;
    }

    [System.Xml.Serialization.XmlElement]
    public string Description
    {
        get;
        protected set;
    }

    [System.Xml.Serialization.XmlElement]
    public string TexturePath
    {
        get;
        protected set;
    }
}
