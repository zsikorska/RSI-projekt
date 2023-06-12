using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;


class MyData
{
    public static void Info()
    {
        Debug.Print("Zuzanna Sikorska, 260464");
        Debug.Print("Piotr Łazik, 260371");
        Debug.Print(DateTime.Now.ToString("dd MMMM, HH:mm:ss", new CultureInfo("pl-PL")));

        Debug.Print(Environment.Version.ToString());
        Debug.Print(Environment.UserName);
        Debug.Print(Environment.OSVersion.ToString());

        Debug.Print(GetLocalIPAddress());

        Debug.Print("\n");

    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
