using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.ViewModels;
using DistantLearningSystem.Models.LogicModels.Services;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class StudentManager : Manager
    {
        public ProcessResult Registrate(
            HttpContextBase context,
            Student student,
            HttpServerUtilityBase server,
            HttpPostedFileBase imageUpload)
        {
            Func<Student, bool> func = x => x.Email == student.Email && x.Login == student.Login;
            var exists = GetStudent(func);
            using (var transaction = Entities.Database.BeginTransaction())
            {
                try
                {

                    student.Password = Security.GetHashString(student.Password);
                    student.Activation = (int)UserStatus.Unconfirmed;
                    if (exists != null)
                        return ProcessResults.UserAlreadyExists;

                    if (imageUpload != null)
                    {
                        if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                            return ProcessResults.InvalidImageFormat;

                        student.ImgSrc = SaveUserImage(student.Id,
                            StaticSettings.AvatarsUploadFolderPath,
                            student.Email,
                            imageUpload,
                            server);
                    }

                    if (!SendConfirmationMail(context,
                        student.Email,
                        student.Password,
                        UserType.Student.ToString()))
                        throw new ArgumentException();
                    student.LastVisitDate = DateTime.Now;
                    student.RegDate = DateTime.Now;
                    Entities.Students.Add(student);
                    SaveChanges();
                    transaction.Commit();
                    return ProcessResults.RegistrationCompleted;
                }
                catch (ArgumentException)
                {
                    transaction.Rollback();
                    return ProcessResults.WrongEmail;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return ProcessResults.ErrorOccured;
                }
            }

        }

        public Student LogIn(LoginModel model)
        {
            var find = GetStudent(x =>
                (x.Login == model.LoginOrEmail ||
                x.Email == model.LoginOrEmail) &&
                model.Password == x.Password);

            if (find == null)
                return null;

            UpdateLastVisitDate(find);
            return find;
        }

        public void UpdateLastVisitDate(int id)
        {
            var std = GetStudent(id);
            if (std == null)
                return;
            UpdateLastVisitDate(std);
        }

        public void UpdateLastVisitDate(Student student)
        {
            student.LastVisitDate = DateTime.Now;
            SaveChanges();
        }

        public Student GetStudent(int id, bool confirmedOnly = true)
        {
            UserStatus status = (confirmedOnly ? UserStatus.Confirmed : UserStatus.Unconfirmed | UserStatus.Confirmed);
            return Entities.Students.FirstOrDefault(x => x.Id == id && ((UserStatus)x.Activation & status) > 0);
        }

        public Student GetStudent(Func<Student, bool> predicate, bool confirmedOnly = true)
        {
            var students = GetStudents().ToList();
            var status = (confirmedOnly ? UserStatus.Confirmed : UserStatus.Unconfirmed | UserStatus.Confirmed);
            return students.Where(predicate).FirstOrDefault(student => ((UserStatus)student.Activation & status) > 0);
        }

        public ProcessResult Delete(int studentId)
        {
            var studentToRemove = GetStudent(studentId);
            if (studentToRemove == null)
                return ProcessResults.ProfileNotExising;

            Entities.Students.Remove(studentToRemove);
            SaveChanges();
            return ProcessResults.StudentDeleted;
        }

        public IEnumerable<Student> GetStudents()
        {
            return Entities.Students.ToList();
        }

        public ProcessResult Edit(Student newStudent,
            HttpServerUtilityBase server,
            HttpPostedFileBase imageUpload,
            bool deleteImage)
        {
            var studentToEdit = GetStudent(x => x.Id == newStudent.Id);
            if (studentToEdit == null)
                return ProcessResults.ProfileNotExising;

            Func<Student, bool> func = x => x.Email == newStudent.Email && x.Login == newStudent.Login;
            var exists = GetStudent(func);

            if (exists != null && exists.Id != newStudent.Id)
                return ProcessResults.UserAlreadyExists;

            studentToEdit.Login = newStudent.Login;
            studentToEdit.Name = newStudent.Name;
            if (!String.IsNullOrEmpty(newStudent.Password))
                studentToEdit.Password = Security.GetHashString(newStudent.Password);
            studentToEdit.Email = newStudent.Email;
            studentToEdit.LastName = newStudent.LastName;

            if (deleteImage && !String.IsNullOrEmpty(studentToEdit.ImgSrc))
            {
                DeleteImage(studentToEdit.ImgSrc, server);
                studentToEdit.ImgSrc = null;
            }

            if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;

                studentToEdit.ImgSrc = SaveUserImage(studentToEdit.Id,
                    StaticSettings.AvatarsUploadFolderPath,
                    studentToEdit.Email,
                    imageUpload,
                    server);
            }

            SaveChanges();
            return ProcessResults.ProfileEdited;
        }
    }
}