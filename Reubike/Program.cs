using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Reubike
{
    class Program
    {
        private static string SourcePath = string.Empty;
        private static string DestinationPath = string.Empty;
        private static string ActualPath = string.Empty;
        
        static void Main(string[] args)
        {
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("--         Organizador de archivos (fotos y videos) del movil         --");
            Console.WriteLine("------------------------------------------------------------------------");
            LoadProcess();
        }

        private static void LoadProcess() {
            SetSourcePath();
            SetDestinationPath();

            Console.WriteLine();
            Console.WriteLine("-- ¿Empezamos? (da igual lo que escribas, se va a empezar en cuanto pulses intro)");
            Console.ReadLine();

            OrganiceFiles();
        }

        private static void SetSourcePath() {
            Console.WriteLine("-- Introduce la ruta de origen: ");
            if (!ValidatePath(Console.ReadLine(), "SOURCE"))
            {
                SetSourcePath();
            }
        }

        private static void SetDestinationPath() {
            Console.WriteLine("-- Introduce la ruta de destino: ");
            if (!ValidatePath(Console.ReadLine(), "DESTINATION"))
            {
                SetDestinationPath();
            }
        }

        private static bool ValidatePath(string path, string pathType) {
            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                
                Console.WriteLine();
                Console.WriteLine("-- \t Esa ruta no existe");
                Console.WriteLine();

                Console.ResetColor();
                return false;
            }

            switch (pathType.ToUpper())
            {
                case "SOURCE":
                    SourcePath = path;
                    break;
                case "DESTINATION":
                    DestinationPath = path;
                    break;
            }

            return true;
        }

        private static void OrganiceFiles() {
            if (!Directory.Exists(DestinationPath))
            {
                Directory.CreateDirectory(DestinationPath);
            }

            if (Directory.EnumerateFiles(SourcePath).Where(x => IsValidDateFile(x)).Count() == 0) {

                Console.WriteLine();
                Console.WriteLine("-- \t Esa ruta no tiene archivos procesables. El patron del nombre debe ser\"XXX_YYYYMMDD*\"");
                Console.WriteLine();

                LoadProcess();
                return;
            }
            

            foreach (var file in Directory.EnumerateFiles(SourcePath).Where(x => IsValidDateFile(x)))
            {
                var nameFile = Path.GetFileName(file);
                var date = nameFile.Substring(4, 6);
                var destinationPath = Path.Combine(DestinationPath, date);
                var destinationFile = Path.Combine(destinationPath, nameFile);

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"-- Creado el directorio: {destinationPath}");
                }

                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine($"-- Copiando el archivo: {nameFile}");
                File.Copy(file, destinationFile, true);
            }
        }

        private static bool IsValidDateFile(string filename)
        {
            return new Regex(@".*_[0-9]{8}*").IsMatch(filename);
        }

    }
}
