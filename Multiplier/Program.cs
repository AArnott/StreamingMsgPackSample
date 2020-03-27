using MessagePack;
using System;
using System.Threading.Tasks;

namespace Multiplier
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stdout = Console.OpenStandardOutput();
            for (int i = 0; i < 15; i++)
            {
                await MessagePackSerializer.SerializeAsync(stdout, new string('a', i));
                await Task.Delay(1000);
            }
        }
    }
}
