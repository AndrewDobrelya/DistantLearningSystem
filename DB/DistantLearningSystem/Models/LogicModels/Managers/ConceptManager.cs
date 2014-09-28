using System;
using System.Collections.Generic;
using System.Linq;
using DistantLearningSystem.Models.DataModels;
using System.Web;
using DistantLearningSystem.Models.LogicModels.Services;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class ConceptManager : Manager
    {
        public Concept GetConcept(Func<Concept, bool> func)
        {
            return Entities.Concepts.FirstOrDefault(x => func(x));
        }

        public ProcessResult Add(int userId,
            string name,
            string abbreviation,
            string definition,
            HttpPostedFileBase imageUpload,
            HttpServerUtilityBase server)
        {
            var exists = GetConcept(x => x.Name.ToLower() == name.ToLower());

            if (exists != null)
                return ProcessResults.ConceptAlreadyExists;

            using (var transaction = Entities.Database.BeginTransaction())
            {
                ProcessResult result = null;

                try
                {
                    var concept = new Concept
                    {
                        Name = name,
                        AddedDate = DateTime.Now,
                        Abbreviation = abbreviation,
                        StudentId = userId
                    };

                    var cn = Entities.Concepts.Add(concept);
                    SaveChanges();

                    if (imageUpload != null)
                    {
                        if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                            return ProcessResults.InvalidImageFormat;

                        cn.ImgSrc = SaveImage(cn.Id, StaticSettings.ConceptIconsUploadPath, imageUpload, server);
                    }

                    result = DataManager.Definition.Add(cn.Id, definition, userId);
                    if (!result.Succeeded)
                        throw new Exception();

                    SaveChanges();
                    transaction.Commit();
                    return ProcessResults.ConceptAdded;
                }
                catch
                {
                    transaction.Rollback();
                    return result;
                }
            }
        }

        public ProcessResult Edit(int id, string name,
            string abbreviation,
            HttpPostedFileBase imageUpload,
            HttpServerUtilityBase server,
            bool deleteImage)
        {
            var concept = GetConcept(id);
            if (concept == null)
                return ProcessResults.ConceptNotExisting;
            if (deleteImage)
                concept.ImgSrc = "";
            else if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;

                concept.ImgSrc = SaveImage(id, StaticSettings.ConceptIconsUploadPath, imageUpload, server);
            }

            var exsts = GetConcept(x => String.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (exsts != null && exsts.Id != id)
                return ProcessResults.ConceptAlreadyExists;

            concept.Name = name;
            concept.Abbreviation = abbreviation;

            SaveChanges();

            return ProcessResults.ConceptEdited;
        }

        public ProcessResult Delete(int conceptId)
        {
            var conceptTodelete = GetConcept(conceptId);
            if (conceptTodelete == null)
                return ProcessResults.ConceptNotExisting;

            Entities.Concepts.Remove(conceptTodelete);
            SaveChanges();
            return ProcessResults.ConceptDeleted;
        }

        public IEnumerable<Definition> GetDefinitions(int conceptId)
        {
            return Entities.Definitions.Where(x => x.ConceptId == conceptId);
        }

        public IEnumerable<Concept> GetConcepts()
        {
            return Entities.Concepts.ToList();
        }

        public Concept GetConcept(int conceptId)
        {
            return Entities.Concepts.FirstOrDefault(x => x.Id == conceptId);
        }
    }
}