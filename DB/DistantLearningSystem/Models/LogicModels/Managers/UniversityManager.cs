using DistantLearningSystem.Models.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    public class UniversityManager : Manager
    {
        public ProcessResult AddDepartment(Department department)
        {
            var dept = Entities.Departments.Add(department);
            SaveChanges();
            return null;
        }

        public IEnumerable<University> GetUniversities()
        {
            return Entities.Universities.ToList();
        }

        public IEnumerable<StudentGroup> GetGroups(int id)
        {
            return Entities.StudentGroups.Where(x => x.DepartmentId == id).ToList();
        } 

        public IEnumerable<StudentGroup> GetGroups()
        {
            return Entities.StudentGroups.ToList();
        }

        public ProcessResult AddFaculty(Faculty faculty)
        {
            faculty = Entities.Faculties.Add(faculty);
            SaveChanges();
            return null;
        }

        public Faculty GetFaculty(int facultyId)
        {
            return Entities.Faculties.FirstOrDefault(x => x.Id == facultyId);
        }

        public StudentGroup GetGroup(int id)
        {
            return Entities.StudentGroups.ToList().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Faculty> GetFaculties(int id)
        {
            return Entities.Faculties.Where(x => x.UniversityId == id).ToList();
        }

        public ProcessResult SetGroupLecturer(int lecturerId, int groupId)
        {
            var group = GetGroup(groupId);
            if (group.LecturerId == lecturerId)
                return ProcessResults.YouAreAlreadyLecturer;

            try
            {
                group.LecturerId = lecturerId;
                SaveChanges();
            }
            catch
            {
                return null;
            }

            return ProcessResults.GroupWasSuccessfullyAdded;
        }

        public ProcessResult DeleteFaculty(int facultyid)
        {
            var facultyToRemove = GetFaculty(facultyid);
            if (facultyToRemove == null)
                return ProcessResults.FacultyNotExisting;

            Entities.Faculties.Remove(facultyToRemove);
            SaveChanges();
            return ProcessResults.FacultyDeleted;
        }

        public Department GetDepartment(int departmentId)
        {
            return Entities.Departments.FirstOrDefault(x => x.Id == departmentId);
        }

        public IEnumerable<Department> GetDepartments(int id)
        {
            return Entities.Departments.Where(x => x.FacultyId == id).ToList();
        }

        public University GetUniversity(int id)
        {
            return Entities.Universities.FirstOrDefault(x => x.Id == id);
        }

        public ProcessResult DeleteDepartment(int departmentId)
        {
            var deptToDelete = GetDepartment(departmentId);
            if (deptToDelete == null)
                return ProcessResults.DepartmentNotExisting;

            Entities.Departments.Remove(deptToDelete);
            SaveChanges();
            return ProcessResults.DepartmentDeleted;
        }
    }
}