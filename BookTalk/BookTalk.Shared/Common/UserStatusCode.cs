using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Common;

public enum UserStatusCode
{
    // 사용자 정의 에러 코드
    // UndefinedError : 서버에서 발생한 예외
    // ValidationError : 유효성 검사 실패
    // NoDataException : 객체 조회 중 데이터가 없어서 발생하는 오류
    UndefinedError = -1, ValidationError = -2, NoDataException = -3
}
