using DistantLearningSystem.Models;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.Managers;
using System.Web.Mvc;

namespace DistantLearningSystem.Controllers
{
    public class SearchController : BaseController
    {
        //
        // GET: /Search/

        public ActionResult Concept(int id, int? result)
        {
            var concept = DataManager.Concept.GetConcept(id);
            ViewBag.IsCurUser = IsCurUser(concept.StudentId);
            ViewBag.IsLoggedIn = IsStudent();
            ViewBag.Result = GetProcessResult(result);
            return View(concept);
        }

        private bool IsCurUser(int? id)
        {
            var user = GetUser();

            return user != null &&
                user.UserType == UserType.Student &&
                user.Id == id;

        }

        private static ProcessResult GetProcessResult(int? result)
        {
            return result.HasValue ? ProcessResults.GetById(result.Value) : null;
        }

        public ActionResult Classification(int id, int? result)
        {

            var classification = DataManager.Classification.GetClassification(id);
            ViewBag.Result = GetProcessResult(result);
            ViewBag.IsLoggedIn = IsStudent();
            ViewBag.IsCurUser = IsCurUser(classification.StudentId);
            return View(classification);
        }

        public ActionResult Definition(int id, int? result)
        {
            var definition = DataManager.Definition.GetDefinition(id);
            ViewBag.Result = GetProcessResult(result);
            ViewBag.IsLoggedIn = IsStudent();
            ViewBag.IsCurUser = IsCurUser(definition.StudentId);
            return View(definition);
        }

        public ActionResult Universities()
        {
            return View(DataManager.University.GetUniversities());
        }

        public ActionResult University(int id)
        {
            return View(DataManager.University.GetUniversity(id));
        }

        public ActionResult Department(int id)
        {
            return View(DataManager.University.GetDepartment(id));
        }

        public ActionResult Faculty(int id)
        {
            return View(DataManager.University.GetFaculty(id));
        }

        public ActionResult Classifications(int? result)
        {
            ViewBag.Result = GetProcessResult(result);
            ViewBag.IsLoggedIn = IsStudent();
            return View(DataManager.Classification.GetClassifications());
        }

        public ActionResult Groups()
        {
            return View(DataManager.University.GetGroups());
        }

        public ActionResult Concepts(int? result)
        {
            ViewBag.IsLoggedIn = IsStudent();
            ViewBag.Result = GetProcessResult(result);
            return View(DataManager.Concept.GetConcepts());
        }

        public ActionResult Lectures(int id = -1)
        {
            if (id == -1)
                return View(DataManager.Lecturer.GetLectures());

            ViewBag.Department = DataManager.University.GetDepartment(id);
            return View(DataManager.Lecturer.GetLecturersOfTheDepaertment(id));
        }

        public ActionResult Students()
        {
            var students = DataManager.Student.GetStudents();
            return View(students);
        }

        public ActionResult Sources(int? result)
        {
            ViewBag.Result = GetProcessResult(result);
            return View(DataManager.Source.GetSources());
        }

        public ActionResult Group(int id)
        {
            var group = DataManager.University.GetGroup(id);
            return View(group);
        }
    }
}