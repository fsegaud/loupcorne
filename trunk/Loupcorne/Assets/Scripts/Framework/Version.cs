
public abstract class Version
{
    public static int Major = 1;
    public static int Minor = 2;
    public static string Label = "Geekopolis";

    public static string Platform
    {
        get
        {
#if UNITY_64
            return "64-bits";
#else
            return "32-bits";
#endif
        }
    }

    public static string ToString()
    {
        return ToString("v{0}.{1} {2} ({3})");
    }

    public static string ToString(string format)
    {
        return string.Format(format, Major, Minor, Label, Platform);
    }
}
