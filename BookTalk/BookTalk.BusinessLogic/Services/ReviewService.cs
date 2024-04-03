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

public class ReviewService : IReviewService
{
    private readonly BookTalkDbContext _dbContext;

    public ReviewService(BookTalkDbContext bookTalkDbContext)
    {
        _dbContext = bookTalkDbContext;
    }

    public void Create(Review review)
    {
        _dbContext.Reviews.Add(review);
        _dbContext.SaveChanges();
    }
}
