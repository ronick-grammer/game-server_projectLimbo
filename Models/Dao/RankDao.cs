using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DotnetCoreServer.Models
{
    public interface IRankDao{
        List<RankUser> TotalRank(int Start, int Count);
        List<RankUser> FriendRank(long UserID, List<string> KakaoIDList);
    }

    public class RankDao : IRankDao
    {
        public IDB db {get;}

        public RankDao(IDB db){
            this.db = db;
        }

        public List<RankUser> TotalRank(int Start, int Count){
            
            List<RankUser> list = new List<RankUser>();
            using(MySqlConnection conn = db.GetConnection())
            {   

                string query = String.Format(
                    @"
                    SELECT 
                    user_id, kakao_id, kakao_name, 
                    kakao_photo_url, point,
                    FROM tb_user
                    ORDER BY point desc
                    LIMIT {0}, {1}", Start, Count );

                Console.WriteLine(query);
                int rank = 0;
                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rank++;
                            RankUser user = new RankUser();
                            user.UserID = reader.GetInt64(0);
                            user.KakaoID = reader.GetString(1);
                            user.KakaoName = reader.GetString(2);
                            user.KakaoPhotoURL = reader.GetString(3);
                            user.Point = reader.GetInt32(4);
                            user.Rank = rank;
                            list.Add(user);
                        }
                    }
                }

                conn.Close();
                
            }
            
            return list;

        }
        
        public List<RankUser> FriendRank(long UserID, List<string> KakaoIDList){

            for(int i = 0; i < KakaoIDList.Count; i++){
                KakaoIDList[i] = string.Format("'{0}'", KakaoIDList[i]);
            }
            
            string StrFacebookIDList = string.Join(",", KakaoIDList);

            List<RankUser> list = new List<RankUser>();
            using(MySqlConnection conn = db.GetConnection())
            {   
                string query = String.Format(
                    @"
                    SELECT 
                        user_id, kakao_id, kakao_name, 
                        kakao_photo_url, point
                    FROM tb_user
                    WHERE kakao_id IN ( {0} ) OR user_id = {1}",
                     StrFacebookIDList, UserID);

                Console.WriteLine(query);
                int rank = 0;

                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rank++;
                            RankUser user = new RankUser();
                            user.UserID = reader.GetInt64(0);
                            user.KakaoID = reader.GetString(1);
                            user.KakaoName = reader.GetString(2);
                            user.KakaoPhotoURL = reader.GetString(3);
                            user.Point = reader.GetInt32(4);
                            user.Rank = rank;
                            list.Add(user);
                        }
                    }
                }
                
                conn.Close();
            }
            return list;
        }



    }
}