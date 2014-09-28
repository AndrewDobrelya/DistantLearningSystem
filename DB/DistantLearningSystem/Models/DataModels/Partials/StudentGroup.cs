using System;


namespace DistantLearningSystem.Models.DataModels
{
    public partial class StudentGroup
    {
        public int GetGroupCourse()
        {
            string[] strs = Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            DateTime dt = new DateTime(2000 + Convert.ToInt32(strs[1]), 9, 1);
            DateTime now = DateTime.Now;
            var rez = now.Subtract(dt);
            return ((int)(rez.TotalDays + 365 - 1)) / 365;
        }
    }
}