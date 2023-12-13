using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

namespace FileEncryption
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cardsPath = Path.Combine(Environment.CurrentDirectory, "../../../Cards");
            Console.WriteLine(Path.Combine(Environment.CurrentDirectory, "../../../Cards\n"));

            EncriptarArchivos(cardsPath);
            Console.ReadKey();
            /*
            List<char> vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
            var path = Path.Combine(Environment.CurrentDirectory, "../../../Lorem Ipsum.txt");
            double tiempoSincrono = 0.0;
            double tiempoParalelo = 0.0;

            Console.WriteLine("Contando vocales con método síncrono.");
            tiempoSincrono = ContarVocales(vowels, path);
            Console.WriteLine("Contando vocales con método paralelo.");
            tiempoParalelo = ContarVocalesParalelo(vowels, path);

            Console.WriteLine($"El método síncrono tardó {tiempoSincrono - tiempoParalelo} más que el paralelo.");
            Console.ReadLine();
            */
        }

        private static void EncriptarArchivos(string filesPath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IEnumerable<string> files = Directory.EnumerateFiles(filesPath);
            foreach (var file in files)
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] hashBytes;
                try
                {
                    using (var sha256 = SHA256.Create())
                    {
                        hashBytes = sha256.ComputeHash(fileBytes);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            double executionTime = timeSpan.TotalMilliseconds;
            Console.WriteLine($"Se encontraron {files.Count()} archivos || Tiempo de ejecución: {executionTime} milisegundos.");
        }

        private static double ContarVocales(List<char> vows, string Lorem)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string content = File.ReadAllText(Lorem);
            int vowelCount = 0;
            foreach (char c in content)
            {
                if (vows.Contains(c))
                {
                    vowelCount++;
                }
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = ts.TotalMilliseconds.ToString();
            Console.WriteLine($"Se encontraron {vowelCount} vocales en el texto.");
            Console.WriteLine($"Tiempo de ejecucion: {elapsedTime} milisegundos\n");
            return ts.TotalMilliseconds;
        }

        private static double ContarVocalesParalelo(List<char> vows, string Lorem)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            string content = File.ReadAllText(Lorem);
            ConcurrentBag<char> foundVowels = new ConcurrentBag<char>();

            Parallel.ForEach(content, character =>
            {
                if (vows.Contains(character))
                {
                    foundVowels.Add(character);
                }
            });
            sw.Stop();
            int vowelCount = foundVowels.Count();

            TimeSpan ts = sw.Elapsed;
            string elapsedTime = ts.TotalMilliseconds.ToString();
            Console.WriteLine($"Se encontraron {vowelCount} vocales en el texto.");
            Console.WriteLine($"Tiempo de ejecucion: {elapsedTime} milisegundos\n");
            return ts.TotalMilliseconds;
        }
    }
}