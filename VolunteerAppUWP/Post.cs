using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerAppUWP
{
	public class Post
	{
		public int ID { get; set; }
		public int UID { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public string Tags { get; set; } // 10 character string that is 01s
        public DateTime Posted { get; set; }
	}

    public class Comment
    {
        public int PID { get; set; }
        public int UID { get; set; }
        public string Content { get; set; }
    }
	
	public enum Tag
	{
        HumanRights,
        Animals,
        ArtsandCulture,
        ChildrenandYouth,
        Education,
        EldersandSeniorCitizens,
        HealthandMedicine,
        HomelessandHousing,
        PeopleWithDisabilities,
        Technology,
    };
}