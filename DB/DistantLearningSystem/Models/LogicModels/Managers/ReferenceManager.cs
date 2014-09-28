using DistantLearningSystem.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class ReferenceManager : Manager
    {
        public Reference Add(int conceptId, int definitionId, int sourceId, int studentId, string pages)
        {
            var reference = Entities.References.Add(new Reference
            {
                AddedDate = DateTime.Now,
                DefinitionId = definitionId,
                SourceId = sourceId,
                StudentId = studentId,
                Pages = pages
            });

            SaveChanges();
            return reference;
        }

        public Reference Get(int referenceId)
        {
            return Entities.References.FirstOrDefault(x => x.Id == referenceId);
        }

        public ProcessResult Delete(int referenceId)
        {
            var reference = Get(referenceId);
            if (reference == null)
                return null;

            Entities.References.Remove(reference);
            SaveChanges();
            return null;
        }

    }
}