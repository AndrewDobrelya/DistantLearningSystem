using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.Services;
using DistantLearningSystem.Models.LogicModels.ViewModels;
using System.Linq;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class AuthentificationManager : Manager
    {
        public ProcessResult LogInUser(LoginModel model, out UserModel userModel)
        {
            userModel = model.UserType == UserType.Lecturer
                ? (UserModel) DataManager.Lecturer.LogIn(model)
                : (UserModel) DataManager.Student.LogIn(model);

            return userModel == null ? ProcessResults.InvalidEmailOrPassword :
                ProcessResults.LoggedInSuccessfull;
        }

        public ProcessResult ConfirmRegistration(string hash)
        {
            var students = Entities.Students.ToList();
            var lectures = Entities.Lecturers.ToList();

            foreach (var lecturer in lectures)
            {
                var curHash = Security.GetHashString(lecturer.Email + lecturer.Password + UserType.Lecturer);
                if (curHash != hash) continue;
                lecturer.Activation = (int)UserStatus.Confirmed;
                SaveChanges();
                return ProcessResults.RegistrationConfirmed;
            }

            foreach (var student in students)
            {
                var curHash = Security.GetHashString(student.Email + student.Password + UserType.Student);
                if (curHash != hash) continue;
                student.Activation = (int)UserStatus.Confirmed;
                SaveChanges();
                return ProcessResults.RegistrationConfirmed;
            }

            return ProcessResults.ErrorOccured ;
        }
    }
}