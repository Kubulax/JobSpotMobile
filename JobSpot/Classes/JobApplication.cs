using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSpot.Classes
{
    public class JobApplication
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int JobAdId { get; set; }
        public int UserId { get; set; }

        public JobApplication (int id, int jobAdId, int userId)
        {
            Id = id;
            JobAdId = jobAdId;
            UserId = userId;
        }

        public JobApplication (int jobAdId, int userId)
        {
            JobAdId = jobAdId;
            UserId = userId;
        }

        public JobApplication ()
        {

        }
    }
}
