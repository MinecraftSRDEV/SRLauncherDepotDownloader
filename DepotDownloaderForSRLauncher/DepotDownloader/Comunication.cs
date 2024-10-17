// This file is subject to the terms and conditions defined
// in file 'LICENSE', which is part of this source code package.

using System;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DepotDownloader
{
    class DepotDownloaderClient
    {
        public static NamedPipeClientStream client;
        public static void connectToLauncher()
        {
            client = new NamedPipeClientStream(".", "DepotDownloaderPipe", PipeDirection.InOut);
            Console.WriteLine("Łączenie z Launcherem...");
            client.Connect();
            Console.WriteLine("Połączono!");
        }

        public static void formatAndSend(string messageType, string message, int logtype =0)
        {
            var send = new sendLogs
            {
                type = messageType,
                msg = message,
                logType = logtype
            };

            sendMessage(JsonSerializer.Serialize(send));
        }

        public static void sendMessage(string message)
        {
            byte[] requestBytes = Encoding.UTF8.GetBytes(message);
            client.Write(requestBytes, 0, requestBytes.Length);
        }

        public static string recieveMessage()
        {
            byte[] buffer = new byte[1024];
            string messageFromLauncher;
            int bytesRead;
            do
            {
                bytesRead = client.Read(buffer, 0, buffer.Length);
                messageFromLauncher = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (messageFromLauncher.Length > 0)
                {
                    break;
                }
            }
            while (true);
            return messageFromLauncher;
        }
    }
}
