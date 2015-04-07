using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhotoAndVideoOrganiser;

namespace Runner
{
    class Program
    {
        //const string SourceDirectory = @"E:\BACKUP FILES\Photos and Video\ToSort";
        //const string TargetDirectory = @"E:\BACKUP FILES\Photos and Video\ToSort\sorted";
        //static string TargetDirectoryForDuplicates = @"E:\BACKUP FILES\Photos and Video\ToSort\Duplicates";

        const string SourceDirectory = @"E:\BACKUP FILES\Photos and Video\Photos\2004\";
        const string TargetDirectory = @"E:\BACKUP FILES\Photos and Video\Photos";
        static string TargetDirectoryForDuplicates = @"e:\BACKUP FILES\Photos and Video\Photos - Duplicates";

        static void Main(string[] args)
        {
            var organiser = new PhotoOrganiser(TargetDirectory);

            var directories = new List<string>
            {
                SourceDirectory
            };

            directories.AddRange(Directory.GetDirectories(SourceDirectory, "*.*", SearchOption.AllDirectories));

            var orderedDirectories = directories.OrderBy(q => q).ToList();

            foreach (var directory in orderedDirectories)
            {
                Console.WriteLine("Directory                   : {0}", directory);

                var results = organiser.Organise(directory);

                PrintResults(results);

                RenameFiles(results);
            }

            Console.WriteLine("COMPLETED");
            Console.ReadLine();
        }

        private static void RenameFiles(IEnumerable<ComparisonResult> results)
        {
            var incorrectFiles = results.Where(x => x.IsFileNameCorrect == false).ToList();
            if (incorrectFiles.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var comparisonResult in incorrectFiles)
                {
                    if (!File.Exists(comparisonResult.CorrectFileName))
                    {
                        var fi = new FileInfo(comparisonResult.CorrectFileName);

                        if (!fi.Directory.Exists)
                        {
                            Directory.CreateDirectory(fi.Directory.FullName);
                        }
                        File.Move(comparisonResult.CurrentFileName, comparisonResult.CorrectFileName);
                    }
                    if (comparisonResult.IsDuplicate)
                    {
                        var duplicateFilePath = comparisonResult.CorrectFileName.Replace(TargetDirectory, TargetDirectoryForDuplicates);
                        
                        if (File.Exists(duplicateFilePath))
                        {
                            TargetDirectoryForDuplicates = TargetDirectory + Guid.NewGuid().ToString();

                            duplicateFilePath = comparisonResult.CorrectFileName.Replace(TargetDirectory, TargetDirectoryForDuplicates);
                        }

                        var fi = new FileInfo(duplicateFilePath);
                        if (!fi.Directory.Exists)
                        {
                            Directory.CreateDirectory(fi.Directory.FullName);
                        }

                        try
                        {
                            File.Move(comparisonResult.CurrentFileName, duplicateFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.GetBaseException().Message);
                        }                       
                    }
                }
                Console.ResetColor();
            }
        }

        private static void PrintResults(IReadOnlyCollection<ComparisonResult> results)
        {
            var numberOfDuplicates = results.Count(x => x.IsDuplicate);
            var numberIncorrect = results.Count(x => x.IsFileNameCorrect == false);

            Console.WriteLine("Number of files parsed      : {0}", results.Count);
            Console.WriteLine("Number of incorrectly named : {0}", numberIncorrect);
            Console.WriteLine("Number of duplicates        : {0}", numberOfDuplicates);

            var incorrectFiles = results.Where(x => x.IsFileNameCorrect == false).ToList();
            if (incorrectFiles.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var comparisonResult in incorrectFiles)
                {
                    Console.WriteLine("Name mismatch : ");
                    Console.WriteLine("    {0}", comparisonResult.CurrentFileName);
                    Console.WriteLine("    {0}", comparisonResult.CorrectFileName);
                    Console.WriteLine();
                }
                Console.ResetColor();
            }


            var duplicates = results.Where(x => x.IsDuplicate).ToList();
            if (duplicates.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var comparisonResult in duplicates)
                {
                    Console.WriteLine("Duplicate File : ");
                    Console.WriteLine("    {0}", comparisonResult.CurrentFileName);
                    Console.WriteLine("    {0}", comparisonResult.CorrectFileName);
                    Console.WriteLine();
                }
                Console.ResetColor();
            }

            Console.WriteLine("----------------------------------------------------");
        }
    }
}
