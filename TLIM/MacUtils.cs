using System.Security.Cryptography;
using System.Text;

namespace TLIM;

using System;
using System.Linq;
using System.Net.NetworkInformation;

public static class MacUtils
{
    public static string GetMachineCode()
    {
        string machineCode = string.Empty;

        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        if (nics != null && nics.Length > 0)
        {
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    machineCode += adapter.GetPhysicalAddress().ToString();
                    break;
                }
            }
        }

        return machineCode;
    }
    
    public static string GetMachineCodePlus()
    {
        StringBuilder machineCode = new StringBuilder();

        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        if (nics != null && nics.Length > 0)
        {
            foreach (NetworkInterface adapter in nics)
            {
                var macAddress = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(macAddress))
                {
                    machineCode.Append(macAddress);
                }
            }
        }

        return GenerateSHA256String(machineCode.ToString());
    }

    private static string GenerateSHA256String(string inputString)
    {
        SHA256 sha256 = SHA256Managed.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(inputString);
        byte[] hash = sha256.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }

    private static string GetStringFromHash(byte[] hash)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            result.Append(hash[i].ToString("X2"));
        }
        return result.ToString();
    }
}