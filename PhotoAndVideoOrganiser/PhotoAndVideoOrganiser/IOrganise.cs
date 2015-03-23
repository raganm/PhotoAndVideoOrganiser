using System.Collections.Generic;

namespace PhotoAndVideoOrganiser
{
    public interface IOrganise
    {
        List<ComparisonResult> Organise(string directory);
    }
}