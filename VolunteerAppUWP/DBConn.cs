using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerAppUWP
{
    public static class DBConn
    {
        private static string connStr = @"Server=DESKTOP-LG118CO\MYSQLSERVER;Database=VolunteerApp;User Id=sa;Password=as;";
        public static User GetUser(string username, string password)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<User> curr = conn.Query<User>($"select * from User_List where Username = '{username}' and Password = '{password}'").ToList();
                try
                {
                    return curr[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public static User GetUser(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<User> curr = conn.Query<User>($"select * from User_List where UID = '{UID}'").ToList();
                return curr.First();
            }
        }

        public static void AddHours(int UID, string title, double hours, DateTime date)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                conn.Query($"insert into ServiceHour_Data (UID, Title, Hours, Date) values ('{UID}', '{title}', '{hours}', '{date}')");
            }
        }

        public static List<ServiceHours> GetServiceHours(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<ServiceHours> toReturn = conn.Query<ServiceHours>($"select * from ServiceHour_Data where UID = '{UID}'").ToList();
                return toReturn;
            }
        }

        public static double GetTotalHours(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<ServiceHours> shs = conn.Query<ServiceHours>($"select * from ServiceHour_Data where UID = '{UID}'").ToList();
                double total = 0;
                foreach (ServiceHours sh in shs)
                {
                    total += sh.Hours;
                }
                return total;
            }
        }

        public static int GetTotalEvents(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<ServiceHours> shs = conn.Query<ServiceHours>($"select * from ServiceHour_Data where UID = '{UID}'").ToList();
                return shs.Count;
            }
        }

        public static int GetTotalYears(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<ServiceHours> shs = conn.Query<ServiceHours>($"select * from ServiceHour_Data where UID = '{UID}'").ToList();
                List<int> years = new List<int>();
                bool include;
                foreach (ServiceHours sh in shs)
                {
                    include = true;
                    foreach (int i in years)
                        if (i == sh.Date.Year)
                            include = false;
                    if (include)
                        years.Add(sh.Date.Year);
                }
                return years.Count;
            }
        }
        
        public static string GetPhoto(int UID)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                List<User> users = conn.Query<User>($"select * from User_List where UID = '{UID}'").ToList();
                return users[0].Photo;
            }
        }

        public static void SetPhoto(int UID, string photo)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                conn.Query($"update User_List set Photo = '{photo}' where UID = '{UID}'");
            }
        }
		
		public static List<Post> GetPosts(int begin, List<Tag> tags)
		{
            string tagString = TagEnumToString(tags);
            using (IDbConnection conn = new SqlConnection(connStr))
			{
				//change this to start at begin and select next 50 or so perhaps where id is > 0 < 50 if top is most recent, also where the items contain tags, doesn't have to be all though
				List<Post> initialList = conn.Query<Post>($"select top 20 * from Post_List where ID >= '{begin}'").ToList();
                List<Post> toReturn = new List<Post>();
                bool add;
                int i;
                foreach (Post p in initialList)
                {
                    i = 0;
                    add = true;
                    foreach (char c in p.Tags)
                    {
                        if (tagString[i++] == '1' && c == '0')
                        {
                            add = false;
                        }
                    }
                    if (add) toReturn.Add(p);
                }
                return toReturn;
			}
		}

        public static void SubmitPost(int uid, string title, string text, List<Tag> tags)
        {
            using (IDbConnection conn = new SqlConnection(connStr))
            {
                conn.Query($"insert into Post_List (ID, UID, Title, Text, Tags, Posted) values ((select count(*) from Post_List),'{uid}','{title}','{text}','{TagEnumToString(tags)}','{DateTime.Now}')");
            }
        }

        private static string TagEnumToString(List<Tag> tags)
        {
            StringBuilder toReturn = new StringBuilder("0000000000");
            foreach (Tag t in tags)
                toReturn[(int)t] = '1';
            return toReturn.ToString();
        }
    }
}
