using System.Collections.Generic;

namespace PhotoAndVideoOrganiser.Interfaces
{
    public interface IOrganise
    {
        List<ComparisonResult> Organise(string directory);
    }
}