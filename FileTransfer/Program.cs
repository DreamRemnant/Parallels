using System.Diagnostics;

namespace FileTransfer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            contarVocales();
        }

        private static void contarVocales()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<char> vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
            var path = Path.Combine(Environment.CurrentDirectory, "../../../Lorem Ipsum.txt");
            string content = File.ReadAllText(path);
            int vowelCount = 0;
            foreach (char c in content)
            {
                if (vowels.Contains(c))
                {
                    vowelCount++;
                }
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = ts.TotalMilliseconds.ToString();
            Console.WriteLine($"Se encontraron {vowelCount} vocales en el texto.");
            Console.WriteLine($"Tiempo de ejecucion: {elapsedTime} milisegundos");
        }
    }
}