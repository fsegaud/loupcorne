using UnityEngine;
using System.Collections;

using LoupCorne.Framework;

public class DatabaseLoader : MonoBehaviour 
{
    void Awake()
    {
        Database db = Database.Instance;
        db.Clear();
        db.RegisterDatatable<SkillEffectElement>(new Datatable<SkillEffectElement>(System.IO.Path.GetFullPath(@"./Public/ActiveSkill.xml")));
        db.RegisterDatatable<SimDescriptor>(new Datatable<SimDescriptor>(System.IO.Path.GetFullPath(@"./Public/SimDescriptor.xml")));
        db.RegisterDatatable<SimObject>(new Datatable<SimObject>(System.IO.Path.GetFullPath(@"./Public/SimObject.xml")));
    }
}
