using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;

namespace DotnetCoreServer.Models
{
    public interface IUserDao{
        User FindUserByFUID(string KakaoID);
        User GetUser(long UserID);
        User InsertUser(User user);
        //bool UpdateUser(User user);
    }

    public class UserDao : IUserDao
    {
        public IDB db {get;}

        public UserDao(IDB db){
            this.db = db;
        }

        public User FindUserByFUID(string KakaoID){
            User user = new User();
            using(MySqlConnection conn = db.GetConnection())
            {   
                string query = String.Format(
                    "SELECT user_id, kakao_id, kakao_name, kakao_photo_url, point, access_token FROM tb_user WHERE kakao_id = '{0}'",
                     KakaoID);

                Console.WriteLine(query);

                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        if (reader.Read()) // 해당 유저가 존재한다면
                        {
                            user.UserID = reader.GetInt64(0);
                            user.KakaoID = reader.GetString(1);
                            user.KakaoName = reader.GetString(2);
                            user.KakaoPhotoURL = reader.GetString(3);
                            user.Point = reader.GetInt32(4);
                            user.AccessToken = reader.GetString(5);
                            return user;
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }
        
        public User GetUser(long UserID){
            User user = new User();
            using(MySqlConnection conn = db.GetConnection())
            {   
                string query = String.Format(
                    @"
                    SELECT 
                        user_id, kakao_id, kakao_name, 
                        kakao_photo_url, point, access_token, 
                        item, play_hour, ranking 
                    FROM tb_user 
                    WHERE user_id = {0}",
                     UserID);

                Console.WriteLine(query);

                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.UserID = reader.GetInt64(0);
                            user.KakaoID = reader.GetString(1);
                            user.KakaoName = reader.GetString(2);
                            user.KakaoPhotoURL = reader.GetString(3);
                            user.Point = reader.GetInt32(4);
                            user.AccessToken = reader.GetString(5);
                            
                            user.Item = reader.GetString(6);
                            user.PlayHour = reader.GetInt32(7);
                            user.Ranking = reader.GetInt32(8);
                        }
                    }
                }
                
                conn.Close();
            }
            return user;
        }

        public User InsertUser(User user){
            
            string query = String.Format(
                "INSERT INTO tb_user (kakao_id, kakao_name, kakao_photo_url, point, access_token) VALUES ('{0}','{1}','{2}',{3}, '{4}')",
                    user.KakaoID, user.KakaoName, user.KakaoPhotoURL, 0, user.AccessToken);

            Console.WriteLine(query);

            using(MySqlConnection conn = db.GetConnection())
            using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
            {

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                conn.Close();
            }

            return user;
        }

        /*
        public bool UpdateUser(User user){
            using(MySqlConnection conn = db.GetConnection())
            {
                string query = String.Format(
                    @"
                    UPDATE tb_user SET 
                        health = {0}, defense = {1}, damage = {2}, speed = {3},
                        health_level = {4}, defense_level = {5}, damage_level = {6}, speed_level = {7},
                        diamond = {8}, point = {9}
                    WHERE user_id = {10}
                    ",
                    user.Health, user.Defense, user.Damage, user.Speed,
                    user.HealthLevel, user.DefenseLevel, user.DamageLevel, user.SpeedLevel,
                    user.Diamond, user.Point, user.UserID
                     );

                Console.WriteLine(query);
                
                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    
                }

                conn.Close();
            }
            return true;
        }
        */
    }
}