
namespace DistantLearningSystem.Models.LogicModels.Managers
{
    using System;
    using DataModels;
    using System.Collections.Generic;
    using System.Linq;

    public class DefinitionManager : Manager
    {
        public Definition GetDefinition(Func<Definition, bool> func)
        {
            return Entities.Definitions.FirstOrDefault(x => func(x));
        }

        public Definition GetDefinition(int definitionId)
        {
            return Entities.Definitions.FirstOrDefault(x => x.Id == definitionId);
        }

        public ProcessResult Add(int conceptId, string text, int studentId)
        {
            var concept = DataManager.Concept.GetConcept(conceptId);
            if (concept == null)
                return ProcessResults.ConceptNotExisting;

            var exsts = GetDefinition(x => x.ConceptId == conceptId && x.Text == text);
            if (exsts != null)
                return ProcessResults.DefinitionAlreadyExists;

            var def = new Definition
            {
                AddedDate = DateTime.Now,
                Text = text,
                StudentId = studentId,
                ConceptId = conceptId
            };

            Entities.Definitions.Add(def);
            SaveChanges();
            return ProcessResults.DefinitionAdded;
        }

        public ProcessResult Edit(int id, string text)
        {
            var definition = GetDefinition(id);

            if (definition == null)
                return ProcessResults.DefinitionNotExisting;

            var exsts = GetDefinition(x => x.Text == text && x.ConceptId == definition.ConceptId);
            if (exsts != null && exsts.Id != id)
                return ProcessResults.DefinitionAlreadyExists;

            definition.Text = text;
            SaveChanges();

            return ProcessResults.DefinitionEdited;
        }

        public ProcessResult Delete(int id)
        {
            var defintion = GetDefinition(id);
            if (defintion == null)
                return ProcessResults.DefinitionNotExisting;

            Entities.Definitions.Remove(defintion);
            SaveChanges();

            return ProcessResults.DefinitionDeleted;
        }

        public IEnumerable<Formulation> GetFormulations(int definitionId)
        {
            return Entities.Formulations.Where(x => x.DefinitionId == definitionId);
        }

        public Formulation GetFormulation(Func<Formulation, bool> func)
        {
            return Entities.Formulations.FirstOrDefault(x => func(x));
        }

        public Formulation GetFormulation(int formulationId)
        {
            return Entities.Formulations.FirstOrDefault(x => x.Id == formulationId); ;
        }

        public ProcessResult AddFormulation(int definitionId,
            int studentId,
            string specificdifference)
        {
            var definition = GetDefinition(definitionId);
            if (definition == null)
                return ProcessResults.DefinitionNotExisting;

            var exts = GetFormulation(x => x.DefinitionId == definitionId &&
                x.SpecificDifference.ToLower() == specificdifference.ToLower());

            if (exts != null)
                return ProcessResults.FormulationAlreadyExists;

            var formulation = new Formulation
            {
                AddedDate = DateTime.Now,
                DefinitionId = definitionId,
                StudentId = studentId,
                SpecificDifference = specificdifference,
            };

            Entities.Formulations.Add(formulation);
            SaveChanges();

            return ProcessResults.FormulationAdded;
        }

        public ProcessResult EditFormulation(int formulationId, string specificDifference)
        {
            var formulation = GetFormulation(formulationId);
            if (formulation == null)
                return ProcessResults.FormulationNotExisting;

            var exsts = GetFormulation(x => x.SpecificDifference.ToLower() == specificDifference);
            if (exsts != null)
                return ProcessResults.FormulationAlreadyExists;

            formulation.SpecificDifference = specificDifference;
            SaveChanges();
            return ProcessResults.FormulationEdited;
        }

        public ProcessResult RemoveFormulation(int formulationId)
        {
            var formulation = GetFormulation(formulationId);
            if (formulation == null)
                return ProcessResults.FormulationNotExisting;
            Entities.Formulations.Remove(formulation);

            SaveChanges();
            return ProcessResults.FormulationDeleted;
        }
    }
}