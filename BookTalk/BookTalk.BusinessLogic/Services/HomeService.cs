using BookTalk.Shared.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Services;

public class HomeService
{
    private readonly BookTalkDbContext _context;

    public HomeService(BookTalkDbContext context)
    {
        _context = context;
    }
}
