namespace LoupCorne.Framework
{
    using System.IO;
    using System.Xml.Serialization;

    public static class XmlHelper
    {
        public static void Serialize<T>(T xmlSerializable, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, xmlSerializable);
            writer.Flush();
            writer.Close();
        }

        public static T Deserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            object o = serializer.Deserialize(reader);
            reader.Close();
            return (T)o;
        }
    }
}