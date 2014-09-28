using System.Web;
using DistantLearningSystem.Models.LogicModels.Managers;
using System.Web.Mvc;
using DistantLearningSystem.Models.DataModels;

namespace DistantLearningSystem.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/

        #region Concept

        public ActionResult AddConcept(int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View();
        }

        public ActionResult EditConcept(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Concept.GetConcept(id));
        }

        public ActionResult DeleteConcept(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            ViewBag.conceptId = id;
            return View(DataManager.Concept.GetConcept(id));
        }

        [HttpPost]
        public ActionResult ManageConceptAdding(string name, 
            string abbreviation,
            string definition,
            HttpPostedFileBase imageUpload)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var user = DataManager.DefineUser(HttpContext);
            ProcessResult result = DataManager.Concept.Add(user.Id, name,
                abbreviation,
                definition,
                imageUpload,
                Server);
            return result.Succeeded ? RedirectToAction("Concepts", "Search", new { result = result.Id })
                : RedirectToAction("AddConcept", new { result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageConceptEditing(int id,
            string name,
            string abbreviation,
            HttpPostedFileBase imageUpload,
            bool deleteImage = false)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var processResult = DataManager.Concept.Edit(id, name, abbreviation, imageUpload,
                Server, deleteImage);
            return processResult.Succeeded ? RedirectToAction("Concept", "Search", 
                new { result = processResult.Id, id }) :
                RedirectToAction("EditConcept", new {result = processResult.Id, id});
        }

        [HttpPost]
        public ActionResult ManageConceptDeleting(int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var processResult = DataManager.Concept.Delete(id);
            if (processResult.Succeeded)
                return RedirectToAction("Concepts", "Search", new { result = processResult.Id });
            return RedirectToAction("DeleteConcept", new {id, result = processResult.Id });
        }

        #endregion

        #region Definition

        public ActionResult AddDefinition(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            ViewBag.ConceptId = id;
            return View();
        }

        public ActionResult EditDefinition(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Definition.GetDefinition(id));
        }

        public ActionResult DeleteDefinition(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            ViewBag.definitionId = id;
            return View(DataManager.Definition.GetDefinition(id));
        }

        [HttpPost]
        public ActionResult ManageDefinitionAdding(string text, int conceptId)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var user = DataManager.DefineUser(HttpContext);
            var result = DataManager.Definition.Add(conceptId, text, user.Id);
            return result.Succeeded ? RedirectToAction("Concept", "Search", new {id = conceptId, result = result.Id}) : 
                RedirectToAction("AddDefinition", new { id = conceptId, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageDefinitionEditing(string text, int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Definition.Edit(id, text);
            return result.Succeeded ? RedirectToAction("Concept", "Search", new { result = result.Id, id }) :
                RedirectToAction("EditDefinition", new {id, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageDefinitionDeleting(int id, int conceptId)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Definition.Delete(id);
            return RedirectToAction("Concept", "Search", new { id = conceptId, result = result.Id });
        }

        #endregion

        #region Source

        public ActionResult AddSource(int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View();
        }

        public ActionResult EditSource(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Source.GetSource(id));
        }

        [HttpPost]
        public ActionResult ManageSourceAdding(int typeId, string fulltitle, string author, int? year, string uri, string description)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Source.Add(year, fulltitle, description, typeId, author, uri);
            return result.Succeeded ? RedirectToAction("Sources", "Search", new { result = result.Id }) 
                : RedirectToAction("AddSource", new { result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageSourceEditing(int id, int typeId,
            string fulltitle,
            string author, int year)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Source.EditSsource(id, typeId, fulltitle, author, year);

            return result.Succeeded ? RedirectToAction("Sources", "Search", new { result = result.Id }) : 
                RedirectToAction("AddSource", new { result = result.Id });
        }

        public ActionResult ManageSourceDeleting(int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Source.Delete(id);
            return RedirectToAction("Sources", "Search", new { result = result.Id });
        }

        #endregion

        #region Formulations

        public ActionResult AddFormulation(int id, int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = result.Value;
            ViewBag.definitionId = id;
            return View();
        }

        [HttpPost]
        public ActionResult ManageFormulationAdding(string specificDifference,
            int definitionId)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var user = DataManager.DefineUser(HttpContext);
            var result = DataManager.Definition.AddFormulation(definitionId, user.Id, specificDifference);
            return result.Succeeded ? RedirectToAction("Definition", "Search", new { id = definitionId }) :
                RedirectToAction("AddFormulation", "Dashboard", new { id = definitionId });
        }

        [HttpPost]
        public ActionResult ManageFormulationEditing(string specificDifference,
            int id, int definitionId)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Definition.EditFormulation(id, specificDifference);
            return result.Succeeded ? RedirectToAction("Definition", "Search", new { id = definitionId, result = result.Id }) : 
                RedirectToAction("EditFormulation", "Dashboard", new {id, result = result.Id });
        }

        public ActionResult EditFormulation(int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            return View(DataManager.Definition.GetFormulation(id));
        }

        public ActionResult DeleteFormulation(int id, int definitionId)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Definition.RemoveFormulation(id);
            return RedirectToAction("Definition", "Search", new { id = definitionId, result = result.Id });
        }

        #endregion

        #region Classifications

        public ActionResult AddClassification(int? result)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View();
        }

        [HttpPost]
        public ActionResult ManageClassificationAdding(string Base, int type)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var user = DataManager.DefineUser(HttpContext);
            var result = DataManager.Classification.Add(Base, type, user.Id);
            return result.Succeeded ? RedirectToAction("Classifications", "Search", new { result = result.Id }) :
                RedirectToAction("AddClassification", new { result = result.Id });
        }

        public ActionResult EditClassification(int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            return View(DataManager.Classification.GetClassification(id));
        }

        [HttpPost]
        public ActionResult ManageClassificationEditing(int id, string Base, int type)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            ProcessResult result = DataManager.Classification.Edit(id, Base, type);
            return RedirectToAction("Classifications", "Search", new { result = result.Id });
        }

        public ActionResult DeleteClassification(int id)
        {
            if (HasNoAccess())
                return RedirectToAction("NoPermission", "Base");

            var result = DataManager.Classification.Remove(id);
            return RedirectToAction("Classifications", "Search", new { result = result.Id });
        }

        #endregion
    }
}