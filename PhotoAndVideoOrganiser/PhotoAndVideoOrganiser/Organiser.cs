using System.Collections.Generic;

namespace PhotoAndVideoOrganiser
{
    public abstract class Organiser
    {
        protected string TargetDirectory;

        protected List<ComparisonResult> ComparisonResults { get; set; }
    }
}