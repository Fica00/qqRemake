using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Device;

[Serializable]
public class DeviceData
{
    public string Model;
    public string Name;
    public string Type;
    public string OperatingSystem;
    public string UniqueIdentifier;
    public string Platform;
    public string IpAddressV4;
    public string IpAddressV6;


    public static DeviceData Get()
    {
        return new DeviceData
        {
            Model = SystemInfo.deviceModel,
            Name = SystemInfo.deviceName,
            Type = SystemInfo.deviceType.ToString(),
            OperatingSystem = SystemInfo.operatingSystem,
            UniqueIdentifier = SystemInfo.deviceUniqueIdentifier,
            Platform = Application.platform.ToString(),
            IpAddressV4 = GetIpAddress(AddressFamily.InterNetwork),
            IpAddressV6 = GetIpAddress(AddressFamily.InterNetworkV6),
        };
    }

    private static string GetIpAddress(AddressFamily _address)
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == _address)
            {
                return ip.ToString();
            }
        }

        return string.Empty;
    }
}