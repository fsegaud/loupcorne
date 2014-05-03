using UnityEngine;
using System.Collections;

using LoupCorne.Framework;

public class DbTest : MonoBehaviour
{
    void Awake()
    {
        Database db = Database.Instance;
        db.RegisterDatatable<SkillEffectElement>(new Datatable<SkillEffectElement>(System.IO.Path.GetFullPath(@"./Public/ActiveSkill.xml")));
        db.RegisterDatatable<SimDescriptor>(new Datatable<SimDescriptor>(System.IO.Path.GetFullPath(@"./Public/SimDescriptor.xml")));
        db.RegisterDatatable<SimObject>(new Datatable<SimObject>(System.IO.Path.GetFullPath(@"./Public/SimObject.xml")));
    }

	void Start ()
    {
        //SimDescriptor descriptor = new SimDescriptor();
        //descriptor.Name = "TestDescriptor1";
        //descriptor.Modifiers.Add(new SimModifier() { StringOperator = "Addition", Target = "Stamina", Value = 10 });
        //descriptor.Modifiers.Add(new SimModifier() { StringOperator = "Percent", Target = "Stamina", Value = .5d });
        //XmlHelper.Serialize<SimDescriptor>(descriptor, @"D:\desciptor.xml");

        //SimDescriptor d = db.GetDatatable<SimDescriptor>().GetElement("TestDescriptor1");
        //Debug.Log(string.Format("[db,sim] {0}: {1} {2} {3}", d.Name, d.Modifiers[0].Target, d.Modifiers[0].Operator, d.Modifiers[0].Value));
        //d = db.GetDatatable<SimDescriptor>().GetElement("TestDescriptor2");
        //Debug.Log(string.Format("[db,sim] {0}: {1} {2} {3}", d.Name, d.Modifiers[0].Target, d.Modifiers[0].Operator, d.Modifiers[0].Value));

        //SimObject king = db.GetDatatable<SimObject>().GetElement("King");
        //Debug.Log(string.Format("{0}({1})", king.Name, king.Class));
        //Debug.Log(string.Format("{0} {1}/{2}", king.Properties[0].Name, king.Properties[0].Value, king.Properties[0].BaseValue));
        //Debug.Log(string.Format("{0} {1}/{2}", king.Properties[1].Name, king.Properties[1].Value, king.Properties[1].BaseValue));
	}
}
