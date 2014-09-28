using DistantLearningSystem.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class SourceManager : Manager
    {
        public Source GetSource(Func<Source, bool> func)
        {
            return Entities.Sources.FirstOrDefault(x => func(x));
        }

        private bool CheckForCorrectness(string title, string description) 
        {
            return !(String.IsNullOrEmpty(title) || String.IsNullOrEmpty(description));
        }

        public ProcessResult Add(int? year, string title, string description, int type, string author, string uri)
        {
            if (!CheckForCorrectness(title, description))
                return ProcessResults.NotAllFieldsFilled;

            Source source = new Source { 
                TItle = title, 
                Description = description,
                Type = type
            };

            var _type = (Sources)type;
            if (_type == Sources.WebSource)
            {
                var isUriOk = Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
                if (!isUriOk)
                    return ProcessResults.WrongUri;

                var _source = GetSource(x => x.Url == uri);
                if (_source != null)
                    return ProcessResults.SourceAlreadyExists;

                source.Url = uri;
            }
            else
            {
                if (String.IsNullOrEmpty(author) || !year.HasValue)
                    return ProcessResults.NotAllFieldsFilled;

                var _source = GetSource(x => x.TItle.ToLower() == title.ToLower() &&
                    x.Author == author &&
                    x.PublicationYear == year);
                if (_source != null)
                    return ProcessResults.SourceAlreadyExists;
                source.Author = author;
                source.PublicationYear = year;
            }

            Entities.Sources.Add(source);
            SaveChanges();

            return ProcessResults.SourceAdded;
        }

        public Source GetSource(string title)
        {
            return Entities.Sources.FirstOrDefault(x => x.TItle.ToLower() == title.ToLower());
        }

        public Source GetSource(int sourceId)
        {
            return Entities.Sources.FirstOrDefault(x => x.Id == sourceId);
        }

        public ProcessResult Edit(Source editedSource)
        {
            var src = GetSource(editedSource.Id);
            if (src == null)
                return null;
            src.Type = editedSource.Type;
            src.Author = editedSource.Author;
            src.TItle = editedSource.TItle;
            src.PublicationYear = editedSource.PublicationYear;
            SaveChanges();
            return null;
        }

        public ProcessResult Delete(int sourceId)
        {
            var source = GetSource(sourceId);
            if (source == null)
                return ProcessResults.SourceNotFound;

            Entities.Sources.Remove(source);
            SaveChanges();
            return ProcessResults.SourceDeleted;
        }

        public IEnumerable<Source> GetSources()
        {
            return Entities.Sources.ToList();
        }

        public ProcessResult EditPaperSource(int id, int typeId, string title, string description, string author, int year) 
        {
            if (!CheckForCorrectness(title, description))
                return ProcessResults.NotAllFieldsFilled;

            var source = GetSource(id);
            if (source == null)
                return ProcessResults.SourceNotFound;

            var exsts = GetSource(x => x.TItle == title 
                && x.PublicationYear == year 
                && x.Author == author);

            if (exsts != null)
                return ProcessResults.SourceAlreadyExists;

            source.Type = typeId;
            source.Description = description;
            source.TItle = title;
            source.Author = author;
            source.PublicationYear = year;

            SaveChanges();
            return ProcessResults.SourceEdited;
        }

        public ProcessResult EditElectronicSource(int id, string title, string description, string uri) 
        {
            if (!CheckForCorrectness(title, description))
                return ProcessResults.NotAllFieldsFilled;

            var source = GetSource(id);
            if (source == null)
                return ProcessResults.SourceNotFound;

            var isUriOk = Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
            if (!isUriOk)
                return ProcessResults.WrongUri;

            var exsts = GetSource(x => x.Url == uri);
            if (exsts != null)
                return ProcessResults.SourceAlreadyExists;

            source.TItle = title;
            source.Description = description;
            source.Url = uri;

            SaveChanges();
            return ProcessResults.SourceEdited;
        }
        
        public ProcessResult EditSsource(int id, int typeId, string fulltitle, string author, int year)
        {
            var source = GetSource(id);
            if (source == null)
                return ProcessResults.SourceNotFound;

            source.Type = typeId;
            source.TItle = fulltitle;
            source.Author = author;
            source.PublicationYear = year;

            SaveChanges();
            return ProcessResults.SourceEdited;
        }
    }
}