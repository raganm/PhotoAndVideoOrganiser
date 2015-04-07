using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LevDan.Exif;
using System.Windows.Media.Imaging;

namespace PhotoAndVideoOrganiser
{
    public class Photo : IPhoto
    {
        private readonly FileInfo _photoInfo;
        private static Regex r = new Regex(":");

        public Photo(FileInfo photoInfo)
        {
            _photoInfo = photoInfo;
        }

        public string FullFileName
        {
            get
            {
                return _photoInfo.FullName;

            }
        }

        public FileInfo CurrentFileInfo
        {
            get
            {
                return _photoInfo;
            }
        }

        public DateTime GetDateTaken()
        {
            DateTime formattedDateTaken = DateTime.MinValue;
            ExifTagCollection exif;

            try
            {
                exif = new ExifTagCollection(_photoInfo.FullName);

                var dateTaken = exif[36867];

                bool success = DateTime.TryParse(dateTaken.Value, out formattedDateTaken);

                if (!success)
                {
                    try
                    {
                        formattedDateTaken = ConvertDate(dateTaken.Value);
                    }
                    catch (Exception)
                    {
                        //throw
                    }

                }

            }
            catch (Exception ex)
            {
                //var x = 1;
                //var y = GetDateTakenFromImage(_photoInfo.FullName);
                //throw;
            }

            return formattedDateTaken;
        }

        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            var formattedDateTaken = DateTime.MinValue;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    formattedDateTaken = DateTime.Parse(dateTaken);
                }
            }
            catch (Exception)
            {
                //var x = 1;
                throw;
            }
            return formattedDateTaken;
        }

        public string GetCorrectName(string targetDirectory)
        {
            string fileName;
            string directory;
            string year;

            string correctFileName;
            try
            {
                var dateTaken = GetDateTaken();
                if (dateTaken == DateTime.MinValue)
                {
                    dateTaken = GetDateTakenFromImage(_photoInfo.FullName);
                }

                //dateTaken = dateTaken.ToUniversalTime();

                directory = GetNewFolder(dateTaken.Year, dateTaken.Month);
                fileName = string.Format("{0} - {1}{2}", dateTaken.ToString("yyyy-MM-dd HH.mm.ss"), _photoInfo.Length / 1024, _photoInfo.Extension);

                year = dateTaken.Year.ToString();

                correctFileName = Path.Combine(targetDirectory, year, directory, fileName);
            }
            catch (Exception)
            {
                directory = _photoInfo.Directory.Name;
                fileName = string.Format("{0} - {1} - Unknown Date Time{2}", _photoInfo.Directory.Name, _photoInfo.Length, _photoInfo.Extension);

                var parts = directory.Split(Convert.ToChar(" "));
                var yearPart = parts[0];
                int n;
                var isNumeric = int.TryParse(yearPart, out n);

                if (yearPart.Length == 4 && isNumeric)
                {
                    year = yearPart;
                }
                else
                {
                    year = "Unknown";
                }
                correctFileName = Path.Combine(_photoInfo.Directory.FullName, fileName);

            }

            //var correctFileName = Path.Combine(targetDirectory, "Photos", year, directory, fileName);

            return correctFileName;
        }

        public FileInfo GetCorrectFileInfo(string targetDirectory)
        {
            var correctName = GetCorrectName(targetDirectory);

            return new FileInfo(correctName);
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
    }
}
