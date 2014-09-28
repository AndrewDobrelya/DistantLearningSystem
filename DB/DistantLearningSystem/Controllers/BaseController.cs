using System.Web.Mvc;
using DistantLearningSystem.Models;
using DistantLearningSystem.Models.LogicModels.Managers;

namespace DistantLearningSystem.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected UserModel GetUser()
        {
            return DataManager.DefineUser(HttpContext);
        }

        protected bool IsTeacher(UserModel user = null)
        {
            if (user == null)
                user = GetUser();

            return user != null && user.UserType == UserType.Lecturer;
        }

        protected bool IsStudent()
        {
            var user = GetUser();
            return user != null && user.UserType == UserType.Student;
        }

        protected bool IsLoggedIn()
        {
            var user = GetUser();
            return user != null;
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NoPermission()
        {
            return View();
        }

        protected bool HasNoAccess()
        {
            var user = GetUser();
            return user == null || user.UserType != UserType.Student;
        }

	}
}