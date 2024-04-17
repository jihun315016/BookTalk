using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;

namespace BookTalk.BusinessLogic.Interfaces;

public interface ICommentService
{/// <summary>
 /// 리뷰 댓글 작성 또는 수정
 /// </summary>
 /// <param name="comment"></param>
    public Comment CreateOrUpdate(Comment comment);

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
    public void Delete(int reviewId, int commentId);
}
