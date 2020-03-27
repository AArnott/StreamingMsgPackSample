using MessagePack;
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingMsgPackSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
            };

            var psi = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\..\Multiplier\bin\Debug\netcoreapp3.1\Multiplier.exe"));
            psi.RedirectStandardOutput = true;
            using var child = Process.Start(psi);
            using var msgpackReader = new MessagePackStreamReader(child.StandardOutput.BaseStream);
            ReadOnlySequence<byte>? msgpackSequence;
            while ((msgpackSequence = await msgpackReader.ReadAsync(cts.Token)).HasValue)
            {
                string json = MessagePackSerializer.ConvertToJson(msgpackSequence.Value);
                Console.WriteLine(json);
            }
        }
    }
}
