using System;
using System.Collections.Generic;
using PhotoAndVideoOrganiser.Interfaces;

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
