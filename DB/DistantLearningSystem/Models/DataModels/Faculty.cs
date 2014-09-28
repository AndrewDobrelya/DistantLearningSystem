//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DistantLearningSystem.Models.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Faculty
    {
        public Faculty()
        {
            this.Departments = new HashSet<Department>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int UniversityId { get; set; }
        public string FullName { get; set; }
    
        public virtual ICollection<Department> Departments { get; set; }
        public virtual University University { get; set; }
    }
}
