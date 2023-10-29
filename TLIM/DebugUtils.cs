namespace TLIM;

public class DebugUtils
{
    public static void DebugOut(string msg)
    {
        if (ConstValues.DebugMode)
        {
            Console.WriteLine("[DEBUG] " + msg);
        }
    }
}