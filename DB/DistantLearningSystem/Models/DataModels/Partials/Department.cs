using System;

namespace DistantLearningSystem.Models.DataModels
{
    public partial class Department
    {
        public string CombinedName
        {
            get { return String.Format("{0} ({1})", Name, FullName); }
        }
    }
}