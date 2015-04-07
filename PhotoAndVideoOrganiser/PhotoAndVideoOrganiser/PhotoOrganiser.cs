using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LevDan.Exif;
using PhotoAndVideoOrganiser.Interfaces;

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
            var exceptions = new List<Exception>();

            var directoryInfo = new DirectoryInfo(directory);

            var files = directoryInfo.GetFiles("*.jp*", SearchOption.TopDirectoryOnly);

            var photos = files.Select(fileInfo => new Photo(fileInfo)).ToList();

            var i = 1;
            foreach (var photo in photos.OrderByDescending(x => x.CurrentFileInfo.Name))
            {
                Console.WriteLine(i);
                try
                {
                    var correctFileInfo = photo.GetCorrectFileInfo(TargetDirectory);
                    var comparisonResult = CompareFileNames(photo.CurrentFileInfo, correctFileInfo);

                    ComparisonResults.Add(comparisonResult);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                i++;
            }

            return ComparisonResults;
        }

        private ComparisonResult CompareFileNames(FileInfo currentFile, FileInfo correctFile)
        {

            var comparisonResult = new ComparisonResult
            {
                CurrentFileName = currentFile.FullName,
                CorrectFileName = correctFile.FullName, 
                
            };

            if (currentFile.FullName == correctFile.FullName)
            {
                comparisonResult.IsFileNameCorrect = true;
            }
            else
            {
                comparisonResult.IsFileNameCorrect = false;
            }

            if (comparisonResult.IsFileNameCorrect == false)
            {
                comparisonResult.IsDirectoryCorrect = currentFile.DirectoryName == correctFile.DirectoryName;

                if (correctFile.Exists)
                {
                    comparisonResult.IsDuplicate = true;
                }
            }

            return comparisonResult;
        }

    }
}
