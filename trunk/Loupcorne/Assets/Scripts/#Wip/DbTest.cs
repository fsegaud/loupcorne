using UnityEngine;
using System.Collections;

public class DbTest : MonoBehaviour
{
	void Start ()
    {
        LoupCorne.Framework.Database db = new LoupCorne.Framework.Database();
        db.RegisterDatatable<ActiveSkillElement>(new LoupCorne.Framework.Datatable<ActiveSkillElement>(System.IO.Path.GetFullPath(@"./Public/ActiveSkill.xml")));

        ActiveSkillElement activeskill01 = db.GetDatatable<ActiveSkillElement>().GetElement("ActiveSkill01");
        Debug.Log(string.Format("{0} Type={1} Gfx={2} Sfx={3}", activeskill01.Name, activeskill01.Type, activeskill01.Gfx, activeskill01.Sfx));
	}
}
