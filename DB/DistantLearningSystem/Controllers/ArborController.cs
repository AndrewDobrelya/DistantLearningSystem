using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.Managers;
using DistantLearningSystem.Models.LogicModels.ViewModels;

namespace DistantLearningSystem.Controllers
{
    public class ArborController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Classifications()
        {
            AjaxResponseModel option = new AjaxResponseModel();

            var db = new CourseDataBaseEntities();
            List<string> bases = new List<string>();
            List<string> id = new List<string>();

            foreach (var i in db.Classifications)
            {
                bases.Add(i.Base);
                id.Add((i.Id).ToString(CultureInfo.InvariantCulture));
            }
            option.Data = bases.ToArray();
            option.Length = bases.Count;
            option.Value = id.ToArray();
            JsonResult res = Json(option);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public JsonResult EmptyConcepts()
        {
            AjaxResponseModel option = new AjaxResponseModel();

            var db = new Models.DataModels.CourseDataBaseEntities();

            List<string> enames = new List<string>();
            List<string> eid = new List<string>();

            List<string> names = new List<string>();
            List<string> id = new List<string>();

            foreach (var i in db.Connections)
            {
                if (i.Concept != null && !names.Contains(i.Concept.Name))
                {
                    names.Add(i.Concept.Name);
                    id.Add((i.Id).ToString());
                }
                if (i.Concept1 == null || names.Contains(i.Concept1.Name)) continue;

                names.Add(i.Concept1.Name);
                id.Add((i.Id).ToString());
            }

            foreach (var i in db.Concepts)
            {
                if(!names.Any())
                {
                    enames.Add(i.Name);
                    eid.Add((i.Id).ToString());
                }
                for (int j = 0; j < names.Count(); j++)
                {
                    if (names.Contains(i.Name)) continue;
                    enames.Add(i.Name);
                    eid.Add((i.Id).ToString());
                }
            }


            option.Data = enames.ToArray();
            option.Length = enames.Count;
            option.Value = eid.ToArray();
            JsonResult res = Json(option);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public JsonResult Concepts(int data)
        {
            AjaxResponseModel option = new AjaxResponseModel();

            var db = new Models.DataModels.CourseDataBaseEntities();
            List<string> names = new List<string>();
            List<string> id = new List<string>();

            foreach (var i in db.Connections.ToList())
            {
                if (i.ClassificationId == data && i.ParentConceptId == null)
                {
                    names.Add(i.Concept.Name);
                    id.Add((i.Concept.Id).ToString());
                }
            }
            option.Data = names.ToArray();
            option.Length = names.Count;
            option.Value = id.ToArray();
            JsonResult res = Json(option);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public JsonResult ByConcepts(int idclass, int idconc)
        {
            AjaxResponseModel option = new AjaxResponseModel();

            var db = new Models.DataModels.CourseDataBaseEntities();
            List<string> names = new List<string>();
            List<string> id = new List<string>();

            foreach (var i in db.Connections)
            {
                if (i.ClassificationId == idclass && i.ParentConceptId == idconc)
                {
                    names.Add(i.Concept.Name);
                    id.Add((i.ChildConceptId).ToString());
                }
            }
            option.Data = names.ToArray();
            option.Length = names.Count;
            option.Value = id.ToArray();
            JsonResult res = Json(option);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public JsonResult AddConnection(int idclass, int id1, int id2)
        {            
            AjaxResponseModel option = new AjaxResponseModel();
            List<string> names = new List<string>();
            List<string> id = new List<string>();

            var db = new Models.DataModels.CourseDataBaseEntities();

            Connection connection;

            var studentId = DataManager.DefineUser(HttpContext).Id;

            if (id1 == -1)
            {
                connection = db.Connections.Add(new Connection()
                {
                    StudentId = studentId,
                    ChildConceptId = id2,
                    ClassificationId = idclass
                });
            }
            else
            {
                connection = db.Connections.Add(new Connection()
                {
                    StudentId = studentId,
                    ChildConceptId = id2,
                    ParentConceptId = id1,
                    ClassificationId = idclass
                });
            }

            db.SaveChanges();

            option.Data = names.ToArray();
            option.Length = names.Count;
            option.Value = id.ToArray();
            JsonResult res = Json(option);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }
    }
}