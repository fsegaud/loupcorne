using UnityEngine;
using System.Collections;

public class KingTest : SimObjectWrapper
{
	void Start ()
    {
        this.simObject = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("King").Clone() as LoupCorne.Framework.SimObject;
        this.Refresh();
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), string.Format("Stamina={0}", this.GetPropertyValue(SimProperties.Stamina)));
        GUI.Label(new Rect(10, 30, 200, 20), string.Format("Strength={0}", this.GetPropertyValue(SimProperties.Strength)));

        if (GUI.Button(new Rect(10, 100, 100, 20), "+HolySword"))
        {
            LoupCorne.Framework.SimDescriptor d = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>().GetElement("HolySword");
            this.RemoveDescriptorByClass("Weapon");
            this.AddDescriptor(d);
            this.Refresh();
        }

        if (GUI.Button(new Rect(10, 120, 100, 20), "+HolyArmor"))
        {
            LoupCorne.Framework.SimDescriptor d = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>().GetElement("HolyArmor");
            this.RemoveDescriptorByClass("Chest");
            this.AddDescriptor(d);
            this.Refresh();
        }

        if (GUI.Button(new Rect(10, 140, 100, 20), "DropAll"))
        {
            this.RemoveDescriptorByClass("Item");
            this.Refresh();
        }
    }
}
