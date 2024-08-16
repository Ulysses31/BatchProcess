using System.Net;

/// <summary>
/// Shared class
/// </summary>
public class Shared
{
    /// <summary>
    /// GetHostIpAddress function
    /// </summary>
    /// <returns>String</returns>
    public string GetHostIpAddress()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var item in ipHostInfo.AddressList)
        {
            if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                IPAddress ipAddress = item;
                return ipAddress.ToString();
            }
        }

        return String.Empty;
    }
}
