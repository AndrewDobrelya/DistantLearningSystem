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
    
    public partial class Evaulation
    {
        public Evaulation()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int ResourceId { get; set; }
        public Nullable<int> Result { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
