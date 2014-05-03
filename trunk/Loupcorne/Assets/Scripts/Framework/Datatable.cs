namespace LoupCorne.Framework
{
    using System.Collections.Generic;
    using System.Linq;

    public class Datatable<T> : IDatatable<T> where T : DatatableElement
    {
        private readonly List<T> elements = new List<T>();

        public Datatable(params string[] archives)
        {
            this.Load(archives);
        }

        public Datatable(string archive)
        {
            this.Load(archive);
        }

        public void Load(params string[] archives)
        {
            foreach (string archive in archives)
            {
                this.Load(archive);
            }
        }

        public void Load(string archive)
        {
            this.elements.AddRange(XmlHelper.Deserialize<List<T>>(archive));
        }

        public T GetElement(string name)
        {
            return this.elements.SingleOrDefault(e => e.Name == name);
        }

        public T[] GetElements()
        {
            return this.elements.ToArray();
        }
    }
}