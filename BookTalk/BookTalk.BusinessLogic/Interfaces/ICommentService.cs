﻿using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;

namespace BookTalk.BusinessLogic.Interfaces;

public interface ICommentService
{/// <summary>
 /// 리뷰 댓글 작성 후 작성한 댓글 가져오기
 /// </summary>
 /// <param name="comment"></param>
    public Comment CreateAndGetComment(Comment comment);

    /// <summary>
    /// 댓글 리스트 가져오기
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public ReviewViewModel GetComments(int reviewId, int page = 1);

    /// <summary>
    /// 리뷰 작성글 댓글 pagination 관련 정보 세팅
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="currentPage"></param>
    /// <returns></returns>
    public Pagination SetCommentInfo(int reviewId, int currentPage = 1);

    /// <summary>
    /// 리뷰 댓글 삭제
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="commentId"></param>
    public void DeleteComment(int reviewId, int commentId);

    /// <summary>
    /// 리뷰 댓글 수정
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="commentId"></param>
    /// <param name="content"></param>
    public void PutComment(int reviewId, int commentId, string content);
}
