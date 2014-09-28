using System;
using DistantLearningSystem.Models.DataModels;
using System.Web;
using System.IO;
using DistantLearningSystem.Models.LogicModels.Services;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public abstract class Manager
    {
        protected CourseDataBaseEntities Entities;

        protected Manager()
        {
            Entities = new CourseDataBaseEntities();
        }

        protected string SaveUserImage(int id, string folder, string email, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            var relativePath = folder
                + Security.GetHashString(id + email)
                + Path.GetExtension(imageUpload.FileName);
            imageUpload.SaveAs(server.MapPath("~") + relativePath);
            return relativePath;
        }

        protected string SaveImage(int id, string folder, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            var relativePath = folder + "/" + id + Path.GetExtension(imageUpload.FileName);
            imageUpload.SaveAs(server.MapPath("~") + relativePath);
            return relativePath;
        }

        protected void DeleteImage(string virtualPath, HttpServerUtilityBase server)
        {
            if (virtualPath == null) return;
            var path = server.MapPath("~") + virtualPath;
            var file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        protected bool SendConfirmationMail(HttpContextBase context, string email, string password, string type)
        {
            var confirmationMessageSender = new ConfirmationMailSender();
            var token = Security.GetHashString(email + password + type);

            if (context.Request.Url == null) return false;
            var path = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/User/Confirm?hash=" + token;
            var message = String.Format(StaticSettings.ConfirmationMessage + "{0}", path);
            return confirmationMessageSender.Send(StaticSettings.ConfirmationTitle, message, email);
        }

        protected int SaveChanges()
        {
            var countOfObjects = Entities.SaveChanges();
            return countOfObjects;
        }
    }
}