
namespace DistantLearningSystem.Models.DataModels
{
    using LogicModels;
    using System.Linq;

    public partial class Concept
    {
        private Evaulation evaulation;

        public Evaulation Evaulation
        {
            get
            {
                if (evaulation == null)
                    using (var db = new CourseDataBaseEntities())
                        evaulation = db.Evaulations.FirstOrDefault(x => x.ResourceId == Id
                            && x.TypeId == 1);

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