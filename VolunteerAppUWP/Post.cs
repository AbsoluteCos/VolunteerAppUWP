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
		public string Content { get; set; }
		public List<Tag> Tags { get; set; } // might have to use int list instead if the sql doesn't convert
	}
	
	public enum Tag
	{
		//  List all the different tags here
	};
}