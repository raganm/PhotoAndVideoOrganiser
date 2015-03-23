using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoAndVideoOrganiser;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var organiser = new PhotoOrganiser(@"C:\Temp\sorted");

            var results = organiser.Organise(@"C:\Temp\readytobedeleted");

            Console.WriteLine("COMPLETED");
            var numberOfDuplicates = results.Count(x => x.IsDuplicate);
            var numberIncorrect = results.Count(x => x.IsFileNameCorrect == false);

            Console.WriteLine(string.Format("Number of files parsed      : {0}", results.Count));
            Console.WriteLine(string.Format("Number of incorrectly named : {0}", numberIncorrect));
            Console.WriteLine(string.Format("Number of duplicates        : {0}", numberOfDuplicates));
            Console.ReadLine();
        }
    }
}
