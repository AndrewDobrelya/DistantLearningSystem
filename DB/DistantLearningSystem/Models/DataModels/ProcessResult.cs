using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistantLearningSystem.Models.DataModels
{
    //Класс будет использоваться для оповещения пользователя 
    //при выполнении каких-то действий на сайте
    //Не смотрите на излишнюю избыточность, я его взял из туриста, если что уберем ненужное
    public class ProcessResult
    {
        public int Id { get; private set; }

        public bool Succeeded { get; private set; }

        public string Message { get; set; }

        public bool IsEmpty
        {
            get { return Succeeded == false && Message == null; }
        }

        public ProcessResult(int id, bool succeeded, string message)
        {
            Id = id;
            Succeeded = succeeded;
            Message = message;
        }
    }

    public class ProcessResults
    {
        static readonly ProcessResult[] Results =
        {
            new ProcessResult(0, false, "Неверный email или пароль!"), 
            new ProcessResult(1, false, "Пользователь с такими данными уже существует!"),
            new ProcessResult(2, true, "Вы успешно прошли регестрацию. На Ваш почтовый ящик выслано письмо с подтверждением регистрации!"),
            new ProcessResult(3, true, "Регистраци подтверждена! Теперь Вы можете пользоваться своим личным кабинетом!"),
            new ProcessResult(4, true, "Вы вошли в личный кабинет!"),
            new ProcessResult(5, false, "Неправильный фомат картинки!"),
            new ProcessResult(6, false, "Произошла ошибка!"),
            new ProcessResult(7, true, "Профиль успешно отредактирован!"),
            new ProcessResult(8, false, "Вы уже преподаете в этой группе!" ),
            new ProcessResult(9, true, "Группа была добавлена в Ваш список"),
            new ProcessResult(10, true, "Понятие успешно дабавлено"),
            new ProcessResult(11, false, "Понятие уже существует"),
            new ProcessResult(12, false, "Понятие не существует!"),
            new ProcessResult(13, true, "Определение успешно добавлено"),
            new ProcessResult(14, true, "Понятие удалено"),
            new ProcessResult(15, true, "Понятие успешно отредактировано"),
            new ProcessResult(16, false, "Определение не существует"),
            new ProcessResult(17, true, "Определение успешно отредактировано"),
            new ProcessResult(18, true, "Формулировка успешно добавлена"),
            new ProcessResult(19, false, "Источник уже существует"),
            new ProcessResult(20, true, "Источник успешно добавлен"),
            new ProcessResult(21, true, "Источник успешно отредактирован"),
            new ProcessResult(22, false, "Источник не найден"),
            new ProcessResult(23, true, "Источник удален"),
            new ProcessResult(24, true, "Определение удалено"),
            new ProcessResult(25, false, "Формулировка не найдена"),
            new ProcessResult(26, true, "Формулировка удалена"),
            new ProcessResult(27, true, "Формулировка отредактирована"),
            new ProcessResult(28, true, "Классификация добавлена"),
            new ProcessResult(29, false, "Классификация уже существует"),
            new ProcessResult(30, true, "Классификация отредактирована"),
            new ProcessResult(31, true, "Классификация удалена"),
            new ProcessResult(32, false, "Определение уже существует"),
            new ProcessResult(33, false, "Формулировка уже существует"),
            new ProcessResult(34, false, "Классификация не существует"),
            new ProcessResult(35, false, "Преподаватель с такими данными уже существует"),
            new ProcessResult(36, false, "Профиль не найден"),
            new ProcessResult(37, false, "Неправильно указан email"),
            new ProcessResult(38, false, "Преподавателя не существует"),
            new ProcessResult(39, true, "Преподавателя удален"),
            new ProcessResult(40, true, "Студент удален"),
            new ProcessResult(41, false, "Связь уже существует"),
            new ProcessResult(42, true, "Связь отредактирована"),
            new ProcessResult(43, false, "Связь не существует"),
            new ProcessResult(44, true, "Неправильный адресс"),
            new ProcessResult(45, false, "Не все поля заполнены"),
            new ProcessResult(46, true, "Факультет удален"),
            new ProcessResult(47, false, "Факульутет не найден"),
            new ProcessResult(48, true, "Кафедра удалена"),
            new ProcessResult(49, false, "Кафедра не найдена")
        };

        public static ProcessResult DepartmentDeleted 
        {
            get { return Results[48]; }
        }

        public static ProcessResult DepartmentNotExisting 
        {
            get { return Results[49]; }
        }

        public static ProcessResult FacultyNotExisting 
        {
            get { return Results[47]; }
        }

        public static ProcessResult FacultyDeleted 
        {
            get { return Results[46]; }
        }

        public static ProcessResult NotAllFieldsFilled 
        {
            get { return Results[45]; }
        }

        public static ProcessResult WrongUri 
        {
            get { return Results[44]; }
        }

        public static ProcessResult ConnectionNotExisting 
        {
            get { return Results[43]; }
        }

        public static ProcessResult ConectionEdited
        {
            get { return Results[42]; }
        }

        public static ProcessResult ConnectionAlreadyExists
        {
            get { return Results[41]; }
        }


        public static ProcessResult StudentDeleted
        {
            get { return Results[40]; }
        }

        public static ProcessResult LecturerDeleted
        {
            get { return Results[39]; }
        }

        public static ProcessResult LecturerNotExisting
        {
            get { return Results[38]; }
        }

        public static ProcessResult WrongEmail
        {
            get { return Results[37]; }
        }

        public static ProcessResult ProfileNotExising
        {
            get { return Results[36]; }
        }

        public static ProcessResult ClassificationNotExisting
        {
            get { return Results[34]; }
        }

        public static ProcessResult FormulationAlreadyExists
        {
            get { return Results[33]; }
        }

        public static ProcessResult DefinitionAlreadyExists
        {
            get { return Results[32]; }
        }

        public static ProcessResult ClassificationDeleted
        {
            get { return Results[31]; }
        }

        public static ProcessResult ClassificationEdited
        {
            get { return Results[30]; }
        }

        public static ProcessResult ClassificationAdded
        {
            get { return Results[28]; }
        }

        public static ProcessResult ClassificationAlreadyExists
        {
            get { return Results[29]; }
        }

        public static ProcessResult FormulationEdited
        {
            get { return Results[27]; }
        }

        public static ProcessResult FormulationDeleted
        {
            get { return Results[26]; }
        }

        public static ProcessResult FormulationNotExisting
        {
            get { return Results[25]; }
        }

        public static ProcessResult DefinitionDeleted
        {
            get { return Results[24]; }
        }

        public static ProcessResult SourceDeleted
        {
            get { return Results[23]; }
        }

        public static ProcessResult SourceNotFound
        {
            get { return Results[22]; }
        }

        public static ProcessResult SourceEdited
        {
            get { return Results[21]; }
        }

        public static ProcessResult SourceAdded
        {
            get { return Results[20]; }
        }

        public static ProcessResult SourceAlreadyExists
        {
            get { return Results[19]; }
        }

        public static ProcessResult FormulationAdded
        {
            get { return Results[18]; }
        }

        public static ProcessResult DefinitionEdited
        {
            get { return Results[17]; }
        }

        public static ProcessResult DefinitionNotExisting
        {
            get { return Results[16]; }
        }

        public static ProcessResult ConceptEdited
        {
            get { return Results[15]; }
        }

        public static ProcessResult ConceptDeleted
        {
            get { return Results[14]; }
        }

        public static ProcessResult DefinitionAdded
        {
            get { return Results[13]; }
        }

        public static ProcessResult ConceptNotExisting
        {
            get { return Results[12]; }
        }

        public static ProcessResult ConceptAlreadyExists
        {
            get { return Results[11]; }
        }

        public static ProcessResult GetById(int id = -1)
        {
            if (id < 0 || id > Results.Length) return null;
            return Results[id];
        }

        public static ProcessResult InvalidEmailOrPassword
        {
            get { return Results[0]; }
        }

        public static ProcessResult UserAlreadyExists
        {
            get { return Results[1]; }
        }

        public static ProcessResult RegistrationCompleted
        {
            get { return Results[2]; }
        }

        public static ProcessResult RegistrationConfirmed
        {
            get { return Results[3]; }
        }

        public static ProcessResult LoggedInSuccessfull
        {
            get { return Results[4]; }
        }

        public static ProcessResult InvalidImageFormat
        {
            get { return Results[5]; }
        }

        public static ProcessResult ErrorOccured
        {
            get { return Results[6]; }
        }

        public static ProcessResult ProfileEdited
        {
            get { return Results[7]; }
        }

        public static ProcessResult YouAreAlreadyLecturer
        {
            get { return Results[8]; }
        }

        public static ProcessResult GroupWasSuccessfullyAdded
        {
            get { return Results[9]; }
        }

        public static ProcessResult ConceptAdded
        {
            get { return Results[10]; }
        }
    }
}
