using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class db
    {
        public static MySqlConnection getcon()
        {

            //string ConnectionString = "database=sql8123701;server=sql8.freemysqlhosting.net;uid=sql8123701;pwd=usELRuR1Lp";
            //string ConnectionString = "database=db_a053e9_natota;server=MYSQL5011.Smarterasp.net;uid=a053e9_natota;pwd=123natota456";
            string ConnectionString = "database=social_network;server=localhost;uid=root;pwd=123456";
            //string ConnectionString = "database=social_network;server=192.168.1.102;uid=remote;pwd=123456";
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            try
            {
                Connection.Open();
            }
            catch (MySqlException)
            {
                return null;
            }

            return Connection;
        }

        public static bool validate_user(user u)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "validate_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("user_email", u.Email));
                sql.Parameters.Add(new MySqlParameter("user_password", u.UserPassWord));
                sql.Connection = Connection;
                long val = (long)sql.ExecuteScalar();

                Connection.CloseAsync();
                return val == 1;
            }
            return false;
        }
        public static bool check_mail(String mail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "check_mail";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("mail", mail));
                sql.Connection = Connection;
                long val = (long)sql.ExecuteScalar();

                Connection.CloseAsync();
                return val == 0;
            }
            return false;
        }

        

        public static bool insertUser(user u)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "insert_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("first_name", u.FirstName));
                sql.Parameters.Add(new MySqlParameter("last_name", u.LastName));
                sql.Parameters.Add(new MySqlParameter("email", u.Email));
                sql.Parameters.Add(new MySqlParameter("pass", u.UserPassWord));
                sql.Parameters.Add(new MySqlParameter("gender", u.Gender));
                sql.Parameters.Add(new MySqlParameter("birthdate", u.BirthDate));
                sql.Connection = Connection;
                int val = sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }

        public static bool updateUserImage(int imageId, String userMail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "update_user_image";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("mail", userMail));
                sql.Parameters.Add(new MySqlParameter("imageId", imageId));
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }
       

        public static user getUser(string email, bool friend)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email", email));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                user u = new user();
                if (reader.HasRows && reader.Read())
                {
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PhoneNumber"].GetType().Name != "DBNull")
                        u.PhoneNumber = (String)reader["PhoneNumber"];
                    if (reader["Gender"].GetType().Name != "DBNull")
                        u.Gender = (String)reader["Gender"];
                    if (friend && reader["BirthDate"].GetType().Name != "DBNull")
                        u.BirthDate = (DateTime)reader["BirthDate"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    if (reader["HomeTown"].GetType().Name != "DBNull")
                        u.HomeTown = (String)reader["HomeTown"];
                    if (reader["MaritalStatus"].GetType().Name != "DBNull")
                        u.MaritalStatus = (String)reader["MaritalStatus"];
                    if (friend && reader["Aboutme"].GetType().Name != "DBNull")
                        u.Aboutme = (String)reader["Aboutme"];
                }
                else
                {
                    u = null;
                }
                reader.Close();
                Connection.CloseAsync();
                return u;
            }
            return null;
        }

        public static bool isFriend(string own, string friend)
        {

            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "is_friend";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_1", own));
                sql.Parameters.Add(new MySqlParameter("email_2", friend));
                sql.Connection = Connection;
                long val = (long)sql.ExecuteScalar();
                Connection.CloseAsync();
                return val == 1;
            }
            return false;
        }
        public static bool isWaiting(string own, string friend)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "is_waiting";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_1", own));
                sql.Parameters.Add(new MySqlParameter("email_2", friend));
                sql.Connection = Connection;
                long val = (long)sql.ExecuteScalar();
                Connection.CloseAsync();
                return val == 1;
            }
            return false;
        }

        
        public static int insertImage(String url)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "insert_image";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("url", url));
                MySqlParameter output = new MySqlParameter("id",MySqlDbType.Int32);
                output.Direction = System.Data.ParameterDirection.Output;
                sql.Parameters.Add(output);
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                int id = (int)sql.Parameters["id"].Value;
                Connection.CloseAsync();
                return id;
            }
            return -1;
        }

        public static String getUserImage(String mail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_image_of_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("mail", mail));
                sql.Connection = Connection;
                String val = (String)sql.ExecuteScalar();
                Connection.CloseAsync();
                return val;
            }
            return null;
        }

        public static IEnumerable<user> getFriends(string email)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_friend";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_", email));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> friends = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    friends.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return friends;
            }
            return null;
        }

        public static IEnumerable<user> getRequests(string email)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_friend_request";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_", email));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> friends = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    friends.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return friends;
            }
            return null;
        }

        public static int getRequestsCount(string email)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_friend_request_count";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_", email));

                sql.Connection = Connection;
                long val = (long)sql.ExecuteScalar();
                Connection.CloseAsync();
                return (int)val;
            }
            return 0;
        }

        public static bool insertFriend(String friendMail, String userMail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "insert_friend";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("sender_email", friendMail));
                sql.Parameters.Add(new MySqlParameter("reciever_email", userMail));
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }

        public static bool acceptRequest(String sender, String reciever)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "accept_friend";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("sender_email", sender));
                sql.Parameters.Add(new MySqlParameter("reciever_email", reciever));
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }

        public static bool rejectRequest(String sender, String reciever)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "remove_friend";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("sender_email", sender));
                sql.Parameters.Add(new MySqlParameter("reciever_email", reciever));
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }

        public static LinkedList<post> getProfilePosts(string email, bool friend)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_post_single_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email", email));
                sql.Parameters.Add(new MySqlParameter("flag", friend?"TRUE":"FALSE"));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<post> posts = new LinkedList<post>();
                while (reader.HasRows && reader.Read())
                {
                    post p = new post();
                    p.id = (int)reader["PostId"];
                    if (reader["poster_name"].GetType().Name != "DBNull")
                        p.PosterName = (String)reader["poster_name"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        p.PosterMail = (String)reader["Email"];
                    if (reader["IsPublic"].GetType().Name != "DBNull" && (String)reader["IsPublic"]=="Y")
                        p.IsPublic = true;
                    if (reader["PostTime"].GetType().Name != "DBNull")
                        p.PostingDate = (DateTime)reader["PostTime"];
                    if (reader["Caption"].GetType().Name != "DBNull")
                        p.Caption = (String)reader["Caption"];
                    if (reader["image_path"].GetType().Name != "DBNull")
                        p.ImagePath = (String)reader["image_path"];
                    posts.AddLast(p);
                }
                reader.Close();
                Connection.CloseAsync();
                return posts;
            }
            return null;
        }

        public static LinkedList<post> getAllPosts(string email)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "get_all_posts";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email", email));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<post> posts = new LinkedList<post>();
                while (reader.HasRows && reader.Read())
                {
                    post p = new post();
                    p.id = (int)reader["PostId"];
                    if (reader["poster_name"].GetType().Name != "DBNull")
                        p.PosterName = (String)reader["poster_name"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        p.PosterMail = (String)reader["Email"];
                    if (reader["IsPublic"].GetType().Name != "DBNull" && (String)reader["IsPublic"]=="Y")
                        p.IsPublic = true;
                    if (reader["PostTime"].GetType().Name != "DBNull")
                        p.PostingDate = (DateTime)reader["PostTime"];
                    if (reader["Caption"].GetType().Name != "DBNull")
                        p.Caption = (String)reader["Caption"];
                    if (reader["image_path"].GetType().Name != "DBNull")
                        p.ImagePath = (String)reader["image_path"];
                    posts.AddLast(p);
                }
                reader.Close();
                Connection.CloseAsync();
                return posts;
            }
            return null;
        }
        

        
        public static bool insertPost(post p)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "insert_post";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email", p.PosterMail));
                sql.Parameters.Add(new MySqlParameter("caption_", p.Caption));
                sql.Parameters.Add(new MySqlParameter("date_post", DateTime.Now));
                sql.Parameters.Add(new MySqlParameter("picture_id", p.ImagePath));
                sql.Parameters.Add(new MySqlParameter("is_public", p.IsPublic?"Y":"N"));
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }

        public static IEnumerable<user> SearchMail(string email)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "search_by_email";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_", email));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> res = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    res.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return res;
            }
            return null;
        }

        public static IEnumerable<user> SearchPhone(string phone)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "search_by_phone_number";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("p_number", phone));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> res = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    res.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return res;
            }
            return null;
        }

        public static IEnumerable<user> SearchHome(string home)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "search_by_home_town";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("h_town", home));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> res = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    res.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return res;
            }
            return null;
        }

        public static IEnumerable<user> SearchCaption(string caption)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "search_by_caption";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("caption_", caption));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> res = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    res.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return res;
            }
            return null;
        }

        public static IEnumerable<user> SearchName(String first, String last)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "search_by_name";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("f_name", first));
                sql.Parameters.Add(new MySqlParameter("l_name", last));

                sql.Connection = Connection;
                MySqlDataReader reader = sql.ExecuteReader();
                LinkedList<user> res = new LinkedList<user>();
                while (reader.HasRows && reader.Read())
                {
                    user u = new user();
                    if (reader["FirstName"].GetType().Name != "DBNull")
                        u.FirstName = (String)reader["FirstName"];
                    if (reader["LastName"].GetType().Name != "DBNull")
                        u.LastName = (String)reader["LastName"];
                    if (reader["Email"].GetType().Name != "DBNull")
                        u.Email = (String)reader["Email"];
                    if (reader["PicturePath"].GetType().Name != "DBNull")
                        u.PicturePath = (String)reader["PicturePath"];
                    res.AddLast(u);
                }
                reader.Close();
                Connection.CloseAsync();
                return res;
            }
            return null;
        }


        public static bool ResetProfilePicture(String mail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "remove_image";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("email_", mail));
                
                sql.Connection = Connection;
                sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }
        
         public static bool UpdateUser(user u, String oldEmail)
        {
            MySqlConnection Connection = db.getcon();
            if (Connection != null)
            {
                MySqlCommand sql = new MySqlCommand();
                sql.CommandText = "update_user";
                sql.CommandType = System.Data.CommandType.StoredProcedure;
                sql.Parameters.Add(new MySqlParameter("old_email", oldEmail));
               
                sql.Parameters.Add(new MySqlParameter("first_name", u.FirstName));
                sql.Parameters.Add(new MySqlParameter("last_name", u.LastName));
                sql.Parameters.Add(new MySqlParameter("new_email", u.Email));
                sql.Parameters.Add(new MySqlParameter("phone_number", u.PhoneNumber));
                sql.Parameters.Add(new MySqlParameter("home_town", u.HomeTown));
                sql.Parameters.Add(new MySqlParameter("marital_status", u.MaritalStatus));
                sql.Parameters.Add(new MySqlParameter("about_me", u.Aboutme));
                sql.Connection = Connection;
                int val = sql.ExecuteNonQuery();
                Connection.CloseAsync();
                return true;
            }
            return false;
        }
         public static bool ChangePassword(String mail, String oldPassword, String newPassword)
         {
             MySqlConnection Connection = db.getcon();
             if (Connection != null)
             {
                 MySqlCommand sql = new MySqlCommand();
                 sql.CommandText = "change_pass";
                 sql.CommandType = System.Data.CommandType.StoredProcedure;

                 sql.Parameters.Add(new MySqlParameter("user_email", mail));
                 sql.Parameters.Add(new MySqlParameter("old_password", oldPassword));
                 sql.Parameters.Add(new MySqlParameter("new_password", newPassword));

                 MySqlParameter output = new MySqlParameter("done", MySqlDbType.String);
                 output.Direction = System.Data.ParameterDirection.Output;
                 sql.Parameters.Add(output);
                 sql.Connection = Connection;
                 sql.ExecuteNonQuery();
                 String done = (String)sql.Parameters["done"].Value;
                 Connection.CloseAsync();
                 return done == "true";
             }
             return false;
         }


         public static bool insertLike(String mail, int postId)
         {
             MySqlConnection Connection = db.getcon();
             if (Connection != null)
             {
                 MySqlCommand sql = new MySqlCommand();
                 sql.CommandText = "insert_like";
                 sql.CommandType = System.Data.CommandType.StoredProcedure;
                 sql.Parameters.Add(new MySqlParameter("liker_mail", mail));
                 sql.Parameters.Add(new MySqlParameter("post_id", postId));
                 sql.Connection = Connection;
                 int val = sql.ExecuteNonQuery();
                 Connection.CloseAsync();
                 return true;
             }
             return false;
         }

         public static LinkedList<int> getLikesOfUser(String mail)
         {
             MySqlConnection Connection = db.getcon();
             if (Connection != null)
             {
                 MySqlCommand sql = new MySqlCommand();
                 sql.CommandText = "get_likes_of_user";
                 sql.CommandType = System.Data.CommandType.StoredProcedure;
                 sql.Parameters.Add(new MySqlParameter("mail", mail));

                 sql.Connection = Connection;
                 MySqlDataReader reader = sql.ExecuteReader();
                 LinkedList<int> res = new LinkedList<int>();
                 while (reader.HasRows && reader.Read())
                 {
                     int i = 0;
                     if (reader["PostId"].GetType().Name != "DBNull")
                     {
                         i = (int)reader["PostId"];
                         res.AddLast(i);
                     }

                 }
                 reader.Close();
                 Connection.CloseAsync();
                 return res;
             }
             return null;
         }

         public static LinkedList<user> getPostLikes(int postId)
         {
             MySqlConnection Connection = db.getcon();
             if (Connection != null)
             {
                 MySqlCommand sql = new MySqlCommand();
                 sql.CommandText = "get_likes_of_post";
                 sql.CommandType = System.Data.CommandType.StoredProcedure;
                 sql.Parameters.Add(new MySqlParameter("postId", postId));

                 sql.Connection = Connection;
                 MySqlDataReader reader = sql.ExecuteReader();
                 LinkedList<user> res = new LinkedList<user>();
                 while (reader.HasRows && reader.Read())
                 {
                     user u = new user();
                     if (reader["FirstName"].GetType().Name != "DBNull")
                         u.FirstName = (String)reader["FirstName"];
                     if (reader["LastName"].GetType().Name != "DBNull")
                         u.LastName = (String)reader["LastName"];
                     if (reader["Email"].GetType().Name != "DBNull")
                         u.Email = (String)reader["Email"];
                     if (reader["PicturePath"].GetType().Name != "DBNull")
                         u.PicturePath = (String)reader["PicturePath"];
                     res.AddLast(u);
                 }
                 reader.Close();
                 Connection.CloseAsync();
                 return res;
             }
             return null;
         }

         public static String getUserMail(int id)
         {
             MySqlConnection Connection = db.getcon();
             if (Connection != null)
             {
                 MySqlCommand sql = new MySqlCommand();
                 sql.CommandText = "get_mail_of_user";
                 sql.CommandType = System.Data.CommandType.StoredProcedure;
                 sql.Parameters.Add(new MySqlParameter("id", id));

                 sql.Connection = Connection;
                 MySqlDataReader reader = sql.ExecuteReader();
                 String mail ="";
                 while (reader.HasRows && reader.Read())
                 {
                     if (reader["Email"].GetType().Name != "DBNull")
                         mail = (String)reader["Email"];
                 }
                 reader.Close();
                 Connection.CloseAsync();
                 return mail;
             }
             return "";
         }

        void m()
        {
            //String sql = "SELECT * FROM Temp WHERE Temp.collection  = ";
            //SqlConnection conn = new SqlConnection(connString);
            //using (SqlCommand cmd = new SqlCommand(sql, conn))
            //{
            //    using (SqlDataReader rdr = cmd.ExecuteReader())
            //    {
            //        if (rdr.Read())
            //        {
            //            Program.defaultCollection = (String)rdr["Column1"];
            //            Program.someOtherVar = (String)rdr["Column2"];
            //        }
            //    }
            //    rdr.Close();
            //}
        }

    }
}