using System;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.Managers;
using DistantLearningSystem.Models.LogicModels.ViewModels;

namespace DistantLearningSystem.Controllers
{
    public class JsonController : Controller
    {
        //
        // GET: /Json/
        public JsonResult Universities()
        {
            var universities = DataManager.University.GetUniversities();
            var enumerable = universities as University[] ?? universities.ToArray();
            return GetJsonResult(enumerable);
            //ajaxResponse.Length = enumerable.Length;
            //ajaxResponse.Data = new string[ajaxResponse.Length];
            //ajaxResponse.Value = new string[ajaxResponse.Length];

            //for (var i = 0; i < enumerable.Length; i++)
            //{
            //    ajaxResponse.Value[i] = enumerable[i].Id.ToString(CultureInfo.InvariantCulture);
            //    ajaxResponse.Data[i] = enumerable[i].Name;
            //}

            //var json = Json(ajaxResponse);
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return json;
        }

        public JsonResult Faculties(string data)
        {
            var id = Convert.ToInt32(data);
            var faculties = DataManager.University.GetFaculties(id);
            var enumerable = faculties as Faculty[] ?? faculties.ToArray();
            return GetJsonResult(enumerable);

            //ajaxResponse.Length = enumerable.Length;
            //ajaxResponse.Data = new string[ajaxResponse.Length];
            //ajaxResponse.Value = new string[ajaxResponse.Length];

            //for (var i = 0; i < enumerable.Length; i++)
            //{
            //    ajaxResponse.Value[i] = enumerable[i].Id.ToString(CultureInfo.InvariantCulture);
            //    ajaxResponse.Data[i] = enumerable[i].Name;
            //}

            //var json = Json(ajaxResponse);
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return json;
        }

        public JsonResult Departments(string data)
        {
            var id = Convert.ToInt32(data);
            var departments = DataManager.University.GetDepartments(id);
            var enumerable = departments as Department[] ?? departments.ToArray();
            return GetJsonResult(enumerable);
            //var ajaxResponse = new AjaxResponseModel();
            //ajaxResponse.Length = enumerable.Length;
            //ajaxResponse.Data = new string[ajaxResponse.Length];
            //ajaxResponse.Value = new string[ajaxResponse.Length];

            //for (var i = 0; i < enumerable.Length; i++)
            //{
            //    ajaxResponse.Value[i] = enumerable[i].Id.ToString(CultureInfo.InvariantCulture);
            //    ajaxResponse.Data[i] = enumerable[i].Name;
            //}

            //var json = Json(ajaxResponse);
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return json;
        }

        public JsonResult GetJsonResult(dynamic enumerable)
        {
            var ajaxResponse = new AjaxResponseModel
            {
                Length = enumerable.Length,
                Data = new string[enumerable.Length],
                Value = new string[enumerable.Length]
            };

            for (var i = 0; i < enumerable.Length; i++)
            {
                ajaxResponse.Value[i] = enumerable[i].Id.ToString(CultureInfo.InvariantCulture);
                ajaxResponse.Data[i] = enumerable[i].Name;
            }

            var json = Json(ajaxResponse);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;

        }

        public JsonResult Groups(string data)
        {
            var id = Convert.ToInt32(data);
            var departments = DataManager.University.GetGroups(id);
            var enumerable = departments as StudentGroup[] ?? departments.ToArray();
            return GetJsonResult(enumerable);
            //ajaxResponse.Length = enumerable.Length;
            //ajaxResponse.Data = new string[ajaxResponse.Length];
            //ajaxResponse.Value = new string[ajaxResponse.Length];

            //for (var i = 0; i < enumerable.Length; i++)
            //{
            //    ajaxResponse.Value[i] = enumerable[i].Id.ToString(CultureInfo.InvariantCulture);
            //    ajaxResponse.Data[i] = enumerable[i].Name;
            //}

            //var json = Json(ajaxResponse);
            //json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return json;
        }
    }
}