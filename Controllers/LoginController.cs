using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreServer.Models;
using System.Net;
using AppInformation;
using Newtonsoft.Json.Linq;

namespace DotnetCoreServer.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        IUserDao userDao;
        public LoginController(IUserDao userDao){
            this.userDao = userDao;
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user = userDao.GetUser(id);
            return user;
        }

        // GET Login/KakaLogin
        [HttpGet]
        public string KakaoLogin(object sender, EventArgs e){ // redirect url 에 데이터가 전해짐
            
            // QueryString 얻기
            string authorization_code = Request.Query["code"];

            if(authorization_code == null){
                return "fail to get authorization_code";
            }

            // access_token 얻기
            string access_token = get_access_code(authorization_code);

            // 카카오톡 사용자 정보  얻기
            User user = get_userInfo(access_token);

            var json = JObject.FromObject(user);

            // 회원 가입하기
            Kakao(user);
            
            return json.ToString();
        }

        // POST Login/Kakao
        [HttpPost]
        public LoginResult Kakao([FromBody] User requestUser)
        {
 
            LoginResult result = new LoginResult();
            
            User user = userDao.FindUserByFUID(requestUser.KakaoID);
            
            if(user != null && user.UserID > 0){ // 이미 가입되어 있음
                
                result.Data = user;
                result.Message = "OK";
                result.ResultCode = 1;

                return result;

            } else { // 회원가입 해야함
                userDao.InsertUser(requestUser);
                user = userDao.FindUserByFUID(requestUser.KakaoID);
                result.Data = user;
                result.Message = "New User";
                result.ResultCode = 2;

                return result;
            }
        }
        public string get_access_code(string authorization_code){
            // POST할 데이터를 Request Stream에 쓴다
            StringBuilder postParam = new StringBuilder();
            postParam.Append("grant_type=authorization_code");
            postParam.Append("&client_id=" + Info.rest_api_key);
            postParam.Append("&redirect_uri=" + Info.redirect_url);
            postParam.Append("&code=" + authorization_code);

            byte[] bytes = Encoding.UTF8.GetBytes(postParam.ToString());
            
            string request_url = Info.base_url + "/oauth/token";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(request_url);
            
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Method = "POST";
            request.ContentLength = bytes.Length; // 바이트수 지정
            
            Stream postDataStream = request.GetRequestStream();
            postDataStream.Write(bytes, 0, bytes.Length);
            postDataStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responsePostStream = response.GetResponseStream();
            StreamReader readerPost= new StreamReader(responsePostStream, Encoding.GetEncoding("UTF-8"));
                
            string status = response.StatusCode.ToString();

            string json = readerPost.ReadToEnd();

            // 액세스 토큰 얻기    
            var jObj = JObject.Parse(json);
            string access_token = jObj.SelectToken("access_token").ToString();
            
            return access_token;
        }

        public User get_userInfo(string access_token){

            string request_url = "https://kapi.kakao.com/v2/user/me";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(request_url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + access_token);
            
            User user = new User();
            using(HttpWebResponse responce = (HttpWebResponse)request.GetResponse()){
                Stream responseStream = responce.GetResponseStream();
                using(StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8)){
                    string json = responseStreamReader.ReadToEnd();
                    var jObj = JObject.Parse(json); // json 객체 받기

                    user.KakaoID = jObj["id"].ToString();
                    user.KakaoName = jObj["properties"]["nickname"].ToString();
                    user.KakaoPhotoURL = jObj["properties"]["profile_image"].ToString();
                    user.AccessToken = access_token;
                    user.Point = 0;
                    user.Item = "";
                    user.PlayHour = 0;
                    user.Ranking = 0;
                }
           }
            return user;
        }
    }
}