using GammaGear.Models.DML;
using GammaGear.Services.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using SQLitePCL;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace GammaGear.Services
{
    public class PatchClientService
    {
        private readonly ILogger _logger;
        private readonly IPythonService _pythonService;

        public PatchClientService(ILogger<PatchClientService> logger, IPythonService pythonNetService)
        {
            _logger = logger;
            _pythonService = pythonNetService;
        }


        public async Task GetLiveRevision()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("patch.us.wizard101.com");
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            IPEndPoint ipEndPoint = new(ipAddress, 12500);
            using Socket client = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            await client.ConnectAsync(ipEndPoint);

            var paths = _pythonService.GetAllInstallationPaths();
            // TODO: We should get the user-preferred path rather than the native one. Will not work on steam.
            // TODO: Should search the PatchClient folder for the protocol xml so that we can find it even if BankA doesn't exist.
            string patchMessagePath = Path.Combine(paths[Models.InstallMode.Native], "PatchClient", "BankA", "PatchMessages.xml");
            ProtocolFormat PatchProtocol = new ProtocolFormat(patchMessagePath);
            Message latestFileList = new Message(PatchProtocol.Messages.First(x => x.Order == 2));
            latestFileList.SetPropertyByName("LatestVersion", BitConverter.GetBytes((UInt32)0));
            latestFileList.SetPropertyByName("ListFileName", (new DMLString("")).ToBytes());
            latestFileList.SetPropertyByName("ListFileType", BitConverter.GetBytes((UInt32)0));
            latestFileList.SetPropertyByName("ListFileTime", BitConverter.GetBytes((UInt32)0));
            latestFileList.SetPropertyByName("ListFileSize", BitConverter.GetBytes((UInt32)0));
            latestFileList.SetPropertyByName("ListFileCRC", BitConverter.GetBytes((UInt32)0));
            latestFileList.SetPropertyByName("ListFileURL", (new DMLString("")).ToBytes());
            latestFileList.SetPropertyByName("URLPrefix", (new DMLString("")).ToBytes());
            latestFileList.SetPropertyByName("URLSuffix", (new DMLString("")).ToBytes());
            latestFileList.SetPropertyByName("Locale", (new DMLString("English")).ToBytes());
            byte[] url_req = latestFileList.ToBytes();
            //byte[] url_req = new byte[]
            //{
            //    0x0D, 0xF0, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    0x08, 0x01, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //};
            byte[] buffer = new byte[4_096];
            buffer.Initialize();
            _logger.LogInformation("Awaiting url send");
            _ = await client.SendAsync(url_req);

            Message response = null;
            while (response == null)
            {
                _logger.LogInformation("Waiting on message");
                _ = await client.ReceiveAsync(buffer);
                _logger.LogInformation("Got message with header {}", BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt64(buffer)).ToString("x16"));
                response = PatchProtocol.ParseMessage(buffer);
            }

            //_logger.LogInformation("Waiting on message");

            //string bufferString = "";
            //bufferString = Encoding.ASCII.GetString(buffer);

            File.WriteAllBytes("test.txt", buffer);

            //// If the message was a session offer receive again.
            //if (buffer[4] == 1)
            //{
            //    buffer.Initialize();
            //    _ = await client.ReceiveAsync(buffer);
            //}

            //string _read_url(int start)
            //{
            //    ushort StringLength = BitConverter.ToUInt16(buffer, 0);
            //    string message = Encoding.ASCII.GetString(buffer, start + 2, StringLength);
            //    return message;
            //}

            //int _find_subarray(byte[] subarray, bool reverseSearch)
            //{
            //    if (reverseSearch)
            //    {
            //        for (int i = (subarray.Length) - (4); i >= 0; i--)
            //        {
            //            if (buffer[i] == subarray[0] &&
            //            buffer[i + 1] == subarray[1] &&
            //            buffer[i + 2] == subarray[2] &&
            //            buffer[i + 3] == subarray[3])
            //                return i;
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < subarray.Length - 3; i++)
            //        {
            //            if (buffer[i] == subarray[0] &&
            //            buffer[i + 1] == subarray[1] &&
            //            buffer[i + 2] == subarray[2] &&
            //            buffer[i + 3] == subarray[3])
            //                return i;
            //        }
            //    }
            //    return -1;
            //}

            //string bstring = Encoding.ASCII.GetString(buffer);
            //byte[] http = "http"u8.ToArray();
            //int file_list_url_start = _find_subarray(http, false) - 2;
            //int base_url_start = _find_subarray(http, true) - 2;
            //string file_list_url = _read_url(file_list_url_start);
            //string base_url = _read_url(base_url_start);

            string ListFileName = new DMLString(response.GetPropertyByName("ListFileName"));
            string ListFileURL = new DMLString(response.GetPropertyByName("ListFileURL"));

            _logger.LogInformation("Server returned {}", response);

            client.Shutdown(SocketShutdown.Both);

        }
    }
}
