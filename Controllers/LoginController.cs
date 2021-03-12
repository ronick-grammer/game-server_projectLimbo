﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreServer.Models;

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

        // POST Login/Facebook
        [HttpPost]
        public LoginResult Face([FromBody] User requestUser)
        {

            LoginResult result = new LoginResult();
            
            User user = userDao.FindUserByFUID(requestUser.KakaoID);
            
            if(user != null && user.UserID > 0){ // 이미 가입되어 있음
                
                result.Data = user;
                result.Message = "OK";
                result.ResultCode = 1;

                return result;

            } else { // 회원가입 해야함
                // 임의 문자열 생성
                string AccessToken = Guid.NewGuid().ToString();

                requestUser.AccessToken = AccessToken;
                userDao.InsertUser(requestUser);
                user = userDao.FindUserByFUID(requestUser.KakaoID);
                result.Data = user;
                result.Message = "New User";
                result.ResultCode = 2;

                return result;

            }

        }

    }
}
