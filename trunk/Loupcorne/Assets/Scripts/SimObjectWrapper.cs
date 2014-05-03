
public class SimObjectWrapper : UnityEngine.MonoBehaviour
{
    protected LoupCorne.Framework.SimObject simObject;

    public double GetPropertyValue(string name)
    {
        return this.simObject.GetPropertyValue(name);
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
}