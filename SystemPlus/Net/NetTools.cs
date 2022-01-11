using System.Net;
using System.Net.Sockets;

namespace SystemPlus.Net
{
    /// <summary>
    /// Internet tools
    /// </summary>
    public static class NetTools
    {
        /// <summary>
        /// Get time from internet server
        /// </summary>
        public static DateTime GetNetworkTime(string ntpServer = "time.windows.com")
        {
            // NTP message size - 16 bytes of the digest (RFC 2030)
            byte[] ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            IPAddress[] addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //The UDP port number assigned to NTP is 123
            IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.SendTimeout = 15000;
                socket.ReceiveTimeout = 15000;

                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
            }

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = ByteTools.SwapEndianness(intPart);
            fractPart = ByteTools.SwapEndianness(fractPart);

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime;
        }

        /// <summary>
        /// Returns value indicating if port is a valid number
        /// </summary>
        public static bool IsValidPortNumber(int value)
        {
            if (value < 1)
                return false;
            if (value > 65535)
                return false;

            return true;
        }
    }
}