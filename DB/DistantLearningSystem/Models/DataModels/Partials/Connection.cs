namespace DistantLearningSystem.Models.DataModels
{
    using System.Linq;

    public partial class Connection
    {
        private Evaulation evaulation;

        public Evaulation Evaulation
        {
            get
            {
                if (evaulation != null) return evaulation;
                using (var db = new CourseDataBaseEntities())
                    evaulation = db.Evaulations.FirstOrDefault(x => x.ResourceId == Id
                                                                    && x.TypeId == 4);

                return evaulation;
            }
        }

        public bool Confirmed
        {
            get
            {
                return Evaulation.Result.HasValue;
            }
        }

    }
}