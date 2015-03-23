using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAndVideoOrganiser
{
    public class VideoOrganiser : Organiser, IOrganise
    {
        public VideoOrganiser()
        {

        }

        public List<ComparisonResult> Organise(string directory)
        {
            throw new NotImplementedException();
        }
    }
}
