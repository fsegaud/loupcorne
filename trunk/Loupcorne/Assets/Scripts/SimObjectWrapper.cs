using System.Linq;

public class SimObjectWrapper : UnityEngine.MonoBehaviour
{
    private LoupCorne.Framework.SimObject simObject;

    protected void SetSimObject(LoupCorne.Framework.SimObject simObject)
    {
        this.simObject = simObject;
    }

    public double GetPropertyValue(string name)
    {
        return this.simObject.GetPropertyValue(name);
    }

    public void SetPropertyBaseValue(string name, double baseValue)
    {
        this.simObject.SetPropertyBaseValue(name, baseValue);
    }

    public void AddDescriptor(LoupCorne.Framework.SimDescriptor descriptor)
    {
        this.simObject.AddDescriptor(descriptor);
    }

    public void RemoveDescriptor(LoupCorne.Framework.SimDescriptor descriptor)
    {
        this.simObject.RemoveDescriptor(descriptor);
    }

    public void RemoveDescriptorByName(string name)
    {
        this.simObject.RemoveDescriptorByName(name);
    }

    public void RemoveDescriptorByClass(string @class)
    {
        this.simObject.RemoveDescriptorByClass(@class);
    }

    public void Refresh()
    {
        this.simObject.Refresh();
    }

    public string[] GetTags()
    {
        return this.simObject.Descriptors.Select(d => d.Name).ToArray();
    }
}