using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LevDan.Exif;

namespace PhotoAndVideoOrganiser
{
    public class PhotoOrganiser : Organiser, IOrganise
    {
        public PhotoOrganiser(string targetDirectory)
        {
            TargetDirectory = targetDirectory;
        }

        public List<ComparisonResult> Organise(string directory)
        {
            ComparisonResults = new List<ComparisonResult>();

            var directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles("*.jp*", SearchOption.AllDirectories);

            var i = 1;
            foreach (var fileInfo in files)
            {
                Console.WriteLine(string.Format("{0} of {1}",i,files.Count()));
                var correctFileName = GenerateCorrectFileName(fileInfo);
                var correctFileInfo = new FileInfo(correctFileName);

                CompareFileNames(fileInfo, correctFileInfo);
                i++;
            }

            return ComparisonResults;
        }

        private string GenerateCorrectFileName(FileInfo fileInfo)
        {
            var dateTaken = GetDateTaken(fileInfo);
            var fileName = string.Format("{0} - {1}{2}", dateTaken.ToString("yyyy-MM-dd HH.mm.ss"), fileInfo.Length / 1024, fileInfo.Extension);
            var directory = GetNewFolder(dateTaken.Year, dateTaken.Month);
            
            var correctFileName = Path.Combine(TargetDirectory, "Photos", dateTaken.Year.ToString(), directory, fileName);
            
            return correctFileName;
        }

        private DateTime GetDateTaken(FileInfo fi)
        {
            var exif = new ExifTagCollection(fi.FullName);

            var dateTaken = exif[306];

            var formattedDateTaken = ConvertDate(dateTaken.Value);

            return formattedDateTaken;
        }

        private DateTime ConvertDate(string value)
        {
            var parts = value.Split(Convert.ToChar(" "));

            var dateParts = parts[0].Split(Convert.ToChar(":"));
            var timeParts = parts[1].Split(Convert.ToChar(":"));

            int year = Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            int hour = Convert.ToInt32(timeParts[0]);
            int minute = Convert.ToInt32(timeParts[1]);
            int second = Convert.ToInt32(timeParts[2]);

            var dateTaken = new DateTime(year, month, day, hour, minute, second);

            return dateTaken;
        }

        private string GetNewFolder(int year, int month)
        {
            string monthString;

            switch (month)
            {
                case 1:
                    monthString = "01 January";
                    break;
                case 2:
                    monthString = "02 February";
                    break;
                case 3:
                    monthString = "03 March";
                    break;
                case 4:
                    monthString = "04 April";
                    break;
                case 5:
                    monthString = "05 May";
                    break;
                case 6:
                    monthString = "06 June";
                    break;
                case 7:
                    monthString = "07 July";
                    break;
                case 8:
                    monthString = "08 August";
                    break;
                case 9:
                    monthString = "09 September";
                    break;
                case 10:
                    monthString = "10 October";
                    break;
                case 11:
                    monthString = "11 November";
                    break;
                case 12:
                    monthString = "12 December";
                    break;
                default:
                    monthString = "00 Unknown";
                    break;
            }

            return string.Format("{0} {1}", year, monthString);
        }

        private void CompareFileNames(FileInfo currentFile, FileInfo correctFile)
        {
            var comparisonResult = new ComparisonResult
            {
                CurrentFileName = currentFile.FullName,
                CorrectFileName = correctFile.FullName
            };
            
            if (currentFile.FullName != correctFile.FullName)
            {
                if (correctFile.Directory != null)
                {
                    correctFile.Directory.Create();
                }

                if (correctFile.Exists)
                {
                    comparisonResult.IsDuplicate = true;
                }
                else
                {
                    File.Copy(currentFile.FullName, correctFile.FullName);
                }

            }

            ComparisonResults.Add(comparisonResult);
        }

    }
}
