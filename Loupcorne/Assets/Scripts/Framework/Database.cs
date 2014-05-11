namespace LoupCorne.Framework
{
    using System;
    using System.Collections.Generic;

    public class Database
    {
        private static Database instance;

        private readonly Dictionary<Type, IDatatable> datatables = new Dictionary<Type, IDatatable>();

        public static Database Instance
        {
            get
            {
                return Database.instance ?? (Database.instance = new Database());
            }
        }

        private Database()
        {
        }

        public void RegisterDatatable<T>(IDatatable datatable)
        {
            this.datatables.Add(typeof(T), datatable);
        }

        public IDatatable<T> GetDatatable<T>()
        {
            if (this.datatables.ContainsKey(typeof(T)))
            {
                return this.datatables[typeof(T)] as IDatatable<T>;
            }

            return null;
        }

        public void Clear()
        {
            this.datatables.Clear();
        }
    }
}