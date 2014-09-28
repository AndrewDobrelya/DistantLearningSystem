using DistantLearningSystem.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class ClassificationManager : Manager
    {

        public IEnumerable<Classification> GetClassifications()
        {
            return Entities.Classifications.ToList();
        }

        public Classification GetClassification(Func<Classification, bool> func)
        {
            return Entities.Classifications.FirstOrDefault(x => func(x));
        }

        public Classification GetClassification(int classificationId)
        {
            return Entities.Classifications.FirstOrDefault(x => x.Id == classificationId);
        }

        public ProcessResult Remove(int classificationId)
        {
            var classification = GetClassification(classificationId);
            if (classification == null)
                return ProcessResults.ClassificationNotExisting;

            Entities.Classifications.Remove(classification);
            SaveChanges();
            return ProcessResults.ClassificationDeleted;
        }

        public ProcessResult Add(string Base, int classificationType, int studentId)
        {
            var classification = GetClassification(x => x.Base == Base && x.ClassificationTypeId == classificationType);

            if (classification != null)
                return ProcessResults.ClassificationAlreadyExists;

            var addlassification = new Classification
            {
                Base = Base,
                ClassificationTypeId = classificationType,
                StudentId = studentId,
                AddedDate = DateTime.Now
            };

            Entities.Classifications.Add(addlassification);
            SaveChanges();

            return ProcessResults.ClassificationAdded;
        }

        public ProcessResult Edit(int id, string Base, int classificationType)
        {
            var classification = GetClassification(id);

            if (classification == null)
                return ProcessResults.ClassificationNotExisting;

            var exts = GetClassification(x => x.Base == Base &&
                x.ClassificationTypeId == classificationType);

            if (exts != null && exts.Id != id)
                return ProcessResults.ClassificationAlreadyExists;

            classification.Base = Base;
            classification.ClassificationTypeId = classificationType;
            SaveChanges();

            return ProcessResults.ClassificationEdited;
        }
    }
}