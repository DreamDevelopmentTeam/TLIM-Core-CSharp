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
    
    public static void DeepDebugOut(string msg)
    {
        if (ConstValues.DeepDebugMode)
        {
            Console.WriteLine("[DPBUG] " + msg);
        }
    }
}