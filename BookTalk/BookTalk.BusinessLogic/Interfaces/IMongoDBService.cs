﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IMongoDBService
{
    /// <summary>
    /// 접속된 MongoDB 데이터베이스에서 원하는 컬렉션 가져오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public IMongoCollection<T> GetCollection<T>(int index);

    /// <summary>
    /// 세션 저장 or 업데이트
    /// </summary>
    /// <param name="userId"></param>
    public void CreateOrUpdateSession(string userId);

    /// <summary>
    /// 모든 세션 삭제
    /// </summary>
    public void DeleteAllSessions();
}