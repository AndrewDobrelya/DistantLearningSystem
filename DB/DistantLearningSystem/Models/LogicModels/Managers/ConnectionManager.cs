using DistantLearningSystem.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class ConnectionManager : Manager
    {
        public Connection GetConnection(int id)
        {
            return Entities.Connections.FirstOrDefault(x => x.Id == id);
        }

        public Connection GetConnection(Func<Connection, bool> func)
        {
            return Entities.Connections.FirstOrDefault(x => func(x));
        }


        public ProcessResult Edit(int connectionId,
            int classificationId,
            int parentConcept,
            int childConcept)
        {
            var connection = GetConnection(connectionId);

            if (connection == null)
                return ProcessResults.ConnectionNotExisting;

            var exsts = GetConnection(x =>
                x.ClassificationId == classificationId &&
                x.ChildConceptId == childConcept &&
                x.ParentConceptId == parentConcept);

            if (exsts != null && exsts.Id != connectionId)
                return ProcessResults.ConnectionAlreadyExists;

            connection.ClassificationId = classificationId;
            connection.ChildConceptId = childConcept;
            connection.ParentConceptId = parentConcept;

            SaveChanges();
            return ProcessResults.ConectionEdited;
        }

        public bool Delete(int connectionId)
        {
            var connection = GetConnection(connectionId);
            if (connection == null)
                return false;

            Entities.Connections.Remove(connection);
            SaveChanges();
            return true;
        }

        public bool Add(int parentConcept, int childConcept, int classificationId)
        {
            var connection = new Connection
            {
                ParentConceptId = parentConcept,
                ChildConceptId = childConcept,
                ClassificationId = classificationId
            };

            var conn = GetConnection(x => x.ChildConceptId == connection.ChildConceptId
                && x.ParentConceptId == connection.ParentConceptId
                && x.ClassificationId == connection.ClassificationId);

            if (conn != null)
                return false;

            Entities.Connections.Add(connection);
            SaveChanges();
            return true;
        }
    }
}