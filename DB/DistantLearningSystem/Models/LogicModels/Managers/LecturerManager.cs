using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistantLearningSystem.Models.DataModels;
using DistantLearningSystem.Models.LogicModels.ViewModels;
using DistantLearningSystem.Models.LogicModels.Services;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class LecturerManager : Manager
    {
        public ProcessResult Registrate(
            HttpContextBase context,
            Lecturer lecturer,
            HttpServerUtilityBase server,
            HttpPostedFileBase imageUpload)
        {
            Func<Lecturer, bool> func = x => (x.Email == lecturer.Email || x.Login == lecturer.Login);
            var exists = GetLecturer(func);
            if (exists != null)
                return ProcessResults.UserAlreadyExists;

            using (var transaction = Entities.Database.BeginTransaction())
            {
                try
                {
                    lecturer.Password = Security.GetHashString(lecturer.Password);
                    lecturer.Activation = (int)UserStatus.Unconfirmed;

                    if (imageUpload != null)
                    {
                        if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                            return ProcessResults.InvalidImageFormat;

                        lecturer.ImgSrc = SaveUserImage(lecturer.Id,
                            StaticSettings.AvatarsUploadFolderPath,
                            lecturer.Email,
                            imageUpload,
                            server);
                    }

                    if (!SendConfirmationMail(context,
                        lecturer.Email,
                        lecturer.Password,
                        UserType.Lecturer.ToString()))
                        throw new ArgumentException();

                    lecturer.LastVisitDate = DateTime.Now;
                    Entities.Lecturers.Add(lecturer);
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

        public Lecturer LogIn(LoginModel model)
        {
            var find = GetLecturer(x =>
                (x.Login == model.LoginOrEmail ||
                x.Email == model.LoginOrEmail) &&
                model.Password == x.Password, true);

            if (find == null)
                return null;

            UpdateLastVisitDate(find);
            return find;
        }

        public void UpdateLastVisitDate(int id)
        {
            var lecturer = GetLecturer(id);
            if (lecturer == null)
                return;
            lecturer.LastVisitDate = DateTime.Now;
            SaveChanges();
        }

        public void UpdateLastVisitDate(Lecturer lecturer)
        {
            if (lecturer == null)
                return;
            lecturer.LastVisitDate = DateTime.Now;
            SaveChanges();
        }

        public Lecturer GetLecturer(int id, bool confirmedOnly = true)
        {
            var status = (confirmedOnly ? UserStatus.Confirmed : UserStatus.Unconfirmed | UserStatus.Confirmed);
            return Entities.Lecturers.FirstOrDefault(x => x.Id == id && ((UserStatus)x.Activation & status) > 0);
        }

        public IEnumerable<Lecturer> GetLectures()
        {
            return Entities.Lecturers.ToList();
        }

        public ProcessResult Delete(int lecturerId)
        {
            var lecturerToRemove = GetLecturer(x => x.Id == lecturerId);
            if (lecturerToRemove == null)
                return ProcessResults.ProfileNotExising;

            Entities.Lecturers.Remove(lecturerToRemove);
            SaveChanges();
            return ProcessResults.LecturerDeleted;
        }

        public IEnumerable<Lecturer> GetLecturersOfTheDepaertment(int id)
        {
            return Entities.Lecturers.ToList().Where(x => x.DepartmentId == id);
        }

        public Lecturer GetLecturer(Func<Lecturer, bool> predicate, bool confirmedOnly = false)
        {
            var lectures = GetLectures().ToList();
            var status = (confirmedOnly ? UserStatus.Confirmed : UserStatus.Unconfirmed | UserStatus.Confirmed);
            return lectures.Where(predicate).FirstOrDefault(student => ((UserStatus)student.Activation & status) > 0);
        }

        public ProcessResult Edit(Lecturer newLecturer,
            HttpServerUtilityBase server,
            HttpPostedFileBase imageUpload,
            bool deleteImage)
        {
            var lecturerToEdit = GetLecturer(newLecturer.Id);

            if (lecturerToEdit == null)
                return ProcessResults.ProfileNotExising;

            Func<Lecturer, bool> func = x => x.Email == newLecturer.Email && x.Login == newLecturer.Login;
            var exists = GetLecturer(func);

            if (exists != null && exists.Id != newLecturer.Id)
                return ProcessResults.UserAlreadyExists;

            lecturerToEdit.Login = newLecturer.Login;
            lecturerToEdit.Name = newLecturer.Name;
            if (!String.IsNullOrEmpty(newLecturer.Password))
                lecturerToEdit.Password = Security.GetHashString(newLecturer.Password);
            lecturerToEdit.Email = newLecturer.Email;
            lecturerToEdit.LastName = newLecturer.LastName;

            if (deleteImage)
            {
                DeleteImage(lecturerToEdit.ImgSrc, server);
                lecturerToEdit.ImgSrc = null;
            }

            if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !Security.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;

                lecturerToEdit.ImgSrc = SaveUserImage(lecturerToEdit.Id,
                    StaticSettings.AvatarsUploadFolderPath,
                    lecturerToEdit.Email,
                    imageUpload,
                    server);
            }

            SaveChanges();
            return ProcessResults.ProfileEdited;
        }

        public IEnumerable<Classification> GetClassifications(int userId)
        {
            var rez = Entities.Classifications.Where(x => x.Student.StudentGroup.LecturerId == userId
                && !x.Confirmed);
            return rez;
        }

        public IEnumerable<Definition> GetDefinitions(int userId)
        {
            var rez = Entities.Definitions.Where(x => x.Student.StudentGroup.LecturerId == userId
                && !x.Confirmed);
            return rez;

        }

        public IEnumerable<Concept> GetConcepts(int userId)
        {
            var rez = Entities.Concepts.Where(x => x.Student.StudentGroup.LecturerId == userId
                && !x.Confirmed);
            return rez;
        }
    }
}