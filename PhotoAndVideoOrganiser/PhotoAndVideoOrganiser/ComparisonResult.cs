using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAndVideoOrganiser
{
    public class ComparisonResult
    {
        public string CurrentFileName { get; set; }
        public string CorrectFileName { get; set; }

        public Boolean IsFileNameCorrect
        {
            get
            {
                return (CurrentFileName.ToLower() == CorrectFileName.ToLower()); 
            }
        }

        public Boolean IsDuplicate { get; set; }
    }
}
