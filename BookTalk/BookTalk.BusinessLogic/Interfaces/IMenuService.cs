﻿using BookTalk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IMenuService
{
    /// <summary>
    /// 메뉴 리스트 조회
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Menu> GetAll();
}
