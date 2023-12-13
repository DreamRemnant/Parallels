using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

namespace FileEncryption
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            /**
             * Se observó que al utilizar Environment.CurrentDirectory en Visual Studio 2023 se obtiene la carpeta Bin\Net 6\ o similar
             * y no se encuentra la carpeta de las imagenes especificadas. Cuando se utiliza el mismo método en Visual Studio Code se obtiene
             * la carpeta del proyecto actual y se puede utilizar /Cards con normalidad. Se usa un try catch para detectar cuando no se encuentre
             * la carpeta y se modifica la dirección de la carpeta para el 2do caso.
             */
            string cardsPath = Path.Combine(Environment.CurrentDirectory, "../../../Files");
            try
            {
                Directory.EnumerateFiles(cardsPath);
            } catch (Exception ex)
            {
                cardsPath = Path.Combine(Environment.CurrentDirectory, "/Files");
                Console.WriteLine(ex.Message);
            }
            Double tiempoSíncrono = EncriptarArchivos(cardsPath);
            Double tiempoParalelo = EncriptarArchivosParalelo(cardsPath);
            Console.WriteLine($"El método paralelo fue {tiempoSíncrono - tiempoParalelo} milisegundos más rapido que el método síncrono.");
            Console.ReadKey();
        }

        private static double EncriptarArchivos(string filesPath)
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime startTime = DateTime.Now;
            string startDateTime = startTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
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
            DateTime endTime = DateTime.Now;
            string endDateTime = startTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            TimeSpan timeSpan = stopwatch.Elapsed;
            double executionTime = timeSpan.TotalMilliseconds;
            Console.WriteLine($"Método Síncrono\nFecha de inicio: {startDateTime} || Fecha de final: {endDateTime} || Se encontraron {files.Count()} archivos || Tiempo de ejecución: {executionTime} milisegundos.\n");
            return executionTime;
        }
        private static double EncriptarArchivosParalelo(string filesPath)
        {
            Stopwatch stopwatch = new Stopwatch();
            DateTime startTime = DateTime.Now;
            string startDateTime = startTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            stopwatch.Start();
            IEnumerable<string> files = Directory.EnumerateFiles(filesPath);
            Parallel.ForEach(files, file =>
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] hashBytes;
                try
                {
                    using (var sha256 = SHA256.Create())
                    {
                        hashBytes = sha256.ComputeHash(fileBytes);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            });
            stopwatch.Stop();
            DateTime endTime = DateTime.Now;
            string endDateTime = startTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            TimeSpan timeSpan = stopwatch.Elapsed;
            double executionTime = timeSpan.TotalMilliseconds;
            Console.WriteLine($"Método Paralelo\nFecha de inicio: {startDateTime} || Fecha de final: {endDateTime} || Se encontraron {files.Count()} archivos || Tiempo de ejecución: {executionTime} milisegundos.\n");
            return executionTime;
        }
    }
}