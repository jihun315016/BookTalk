using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Services;

public class MenuService : IMenuService
{
    private readonly BookTalkDbContext _dbContext;

    public MenuService(BookTalkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Menu> GetAll()
    {
        return _dbContext.Menus.AsNoTracking().ToList<Menu>();
    }
}
