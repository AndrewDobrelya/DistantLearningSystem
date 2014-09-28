using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using DistantLearningSystem.Models;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.Managers;
using DistantLearningSystem.Models.LogicModels.Services;
using DistantLearningSystem.Models.LogicModels.ViewModels;

namespace DistantLearningSystem.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            var user = GetUser();
            return RedirectToAction(user == null ? "LogIn" : "ProfileInfo");
        }

        public ActionResult ManageGroupAdding(string groupId, string lecturerId)
        {
            var user = GetUser();
            if (!IsTeacher(user))
                return RedirectToAction("NoPermission", "Base");

            var processResult = DataManager.University.SetGroupLecturer(user.Id, Convert.ToInt32(groupId));
            return RedirectToAction("Index", "Home", new { result = processResult.Id });
        }

        public ActionResult AddGroup()
        {
            var user = GetUser();
            if (!IsTeacher(user))
                return RedirectToAction("NoPermission", "Base");

            ViewBag.Groups = DataManager.University.GetGroups();
            ViewBag.UserId = user.Id;
            return View();
        }

        public ActionResult EditProfile()
        {
            var user = GetUser();

            return user == null ? RedirectToAction("NoPermission") :
                RedirectToAction(user.UserType == UserType.Student ? "EditStudent" :
                "EditLecturer");
        }

        public ActionResult EditLecturer()
        {
            var user = GetUser();
            return !IsTeacher(user) ? (ActionResult)RedirectToAction("NoPermission") :
                View(DataManager.Lecturer.GetLecturer(user.Id));
        }

        [HttpPost]
        public ActionResult ManageLecturerEditing(int id,
            string name,
            string lastName,
            string login,
            string email,
            string password,
            HttpPostedFileBase imageUpload, 
            bool deleteImage = false)
        {
            var user = GetUser();
            if (!IsTeacher(user) || user.Id != id)
                RedirectToAction("NoPermission", "Base");

            var lecturer = new Lecturer
            {
                Id = Convert.ToInt32(id),
                Name = name,
                Login = login,
                Email = email,
                Password = password,
                LastName = lastName
            };

            var p = DataManager.Lecturer.Edit(lecturer, Server, imageUpload, deleteImage);
            return RedirectToAction("LecturerInfo", "User", new { result = p.Id });
        }

        [HttpPost]
        public ActionResult ManageStudentEditing(int id, 
            string name,
            string lastName, 
            string login,
            string email,
            string password,
            string univer,
            string faculty,
            string department,
            HttpPostedFileBase imageUpload,
            bool deleteImage = false)
        {
            var user = GetUser();
            if (IsTeacher(user) || user.Id != id)
                RedirectToAction("NoPermission", "Base");

            var student = new Student
            {
                Id = id,
                Name = name,
                Login = login,
                Email = email,
                Password = password,
                LastName = lastName
            };

            var p = DataManager.Student.Edit(student, Server, imageUpload, deleteImage);
            return RedirectToAction("Index", "Home", new { result = p.Id });
        }


        public ActionResult Activity(int id)
        {
            var student = DataManager.Student.GetStudent(id);
            return View(student);
        }

        public ActionResult EditStudent()
        {
            var user = GetUser();
            return IsTeacher(user) ? (ActionResult)RedirectToAction("NoPermission", "Base") :
                View(DataManager.Student.GetStudent(user.Id));
        }

        public ActionResult ProfileInfo()
        {
            var user = DataManager.DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn");
            return RedirectToAction(user.UserType == UserType.Student ? "StudentInfo" :
                "LecturerInfo");
        }

        public ActionResult LecturerInfo(int id = -1)
        {
            ViewBag.IsCurUser = id == -1;
            if (id == -1)
            {
                var user = GetUser();
                if (user != null && IsTeacher(user))
                    id = user.Id;
            }


            if (id == -1)
                return RedirectToAction("Error","Base");
            return View(DataManager.Lecturer.GetLecturer(id));
        }

        public ActionResult StudentInfo(int id = -1)
        {
            if (id == -1)
            {
                var user = GetUser();
                if (user != null && !IsTeacher(user))
                    id = user.Id;
            }

            if (id == -1)
                return RedirectToAction("Error","Base");
            return View(DataManager.Student.GetStudent(id));
        }

        public ActionResult Logout()
        {
            var userCookie = new HttpCookie("UserId")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            var keyCookie = new HttpCookie("Key")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            var typeCookie = new HttpCookie("UserType")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext.Response.Cookies.Set(userCookie);
            HttpContext.Response.Cookies.Set(keyCookie);
            HttpContext.Response.Cookies.Set(typeCookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogIn(int? result)
        {
            if (IsLoggedIn())
                return RedirectToAction("Index");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View();
        }

        [HttpPost]
        public ActionResult ManageLogIn(string loginOrEmail, string password, string whoAreU)
        {
            var loginModel = new LoginModel
            {
                LoginOrEmail = loginOrEmail,
                Password = Security.GetHashString(password),
                UserType = whoAreU.ToLower() == "студент" ? UserType.Student : UserType.Lecturer
            };

            UserModel user;
            var result = DataManager.Authentification.LogInUser(loginModel, out user);
            if (!result.Succeeded || user == null)
                return RedirectToAction("LogIn",
                    "User",
                    new { result = result.Id });
            SetUser(user, user.Password);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Registration(int? result)
        {
            if (IsLoggedIn())
                return RedirectToAction("Index");
            ViewBag.Groups = DataManager.University.GetGroups();

            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);

            return View();
        }

        public ActionResult AjaxRegistrationFormLoad(string data)
        {
            return RedirectToAction(data.ToLower() == "студент" ? "RegistrateStudent" : "RegistrateLecturer");
        }

        public ActionResult RegistrateLecturer()
        {
            ViewBag.Universities = DataManager.University.GetUniversities();
            return PartialView();
        }

        public ActionResult RegistrateStudent()
        {
            ViewBag.Universities = DataManager.University.GetUniversities();
            return PartialView();
        }

        [HttpPost]
        public ActionResult ManageLecturerRegistration(
            string name, 
            string lastName,
            string login,
            string email,
            string password,
            int department,
            string subject,
            string position, HttpPostedFileBase imageUpload)
        {
            if (IsLoggedIn())
                RedirectToAction("NoPermission", "Base");


            var registrationModel = new Lecturer
            {
                Name = name,
                LastName = lastName,
                Login = login,
                Password = password,
                DepartmentId = department,
                Email = email,
                Subject = subject,
                Position = position
            };

            var result = DataManager.
                Lecturer.
                Registrate(HttpContext,
                registrationModel,
                Server,
                imageUpload);

            return RedirectToAction("Registration", "User", new { result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageStudentRegistration(
            string name, 
            string lastName,
            string login,
            string email,
            string password,
            string groupId,
            HttpPostedFileBase imageUpload)
        {
            if(IsLoggedIn())
                RedirectToAction("NoPermission", "Base");

            var result = DataManager.
                Student.
                Registrate(HttpContext,
                new Student
                {
                    Name = name,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    Login = login,
                    GroupId = Convert.ToInt32(groupId)
                },
                Server,
                imageUpload);

            return RedirectToAction("Registration", "User", new { result = result.Id });
        }

        public ActionResult Confirm(string hash)
        {
            var result = DataManager.Authentification.ConfirmRegistration(hash);
            return RedirectToAction("Registration", "User", new { result = result.Id });
        }

        private void SetUser(UserModel user, string hashedKey)
        {
            if (user == null) return;
            var cookieUser = new HttpCookie("UserId")
            {
                Value = Convert.ToString(user.Id),
                Expires = DateTime.MaxValue
            };

            var cookieKey = new HttpCookie("Key")
            {
                Value = hashedKey,
                Expires = DateTime.MaxValue
            };

            var cookieuserType = new HttpCookie("UserType")
            {
                Value = ((int)user.UserType).ToString(CultureInfo.InvariantCulture),
                Expires = DateTime.MaxValue
            };

            HttpContext.Response.Cookies.Remove("UserId");
            HttpContext.Response.Cookies.Remove("Key");
            HttpContext.Response.Cookies.Remove("UserType");

            HttpContext.Response.SetCookie(cookieUser);
            HttpContext.Response.SetCookie(cookieKey);
            HttpContext.Response.SetCookie(cookieuserType);
        }
    }
}