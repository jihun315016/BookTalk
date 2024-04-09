// Pagination 요소 업데이트
// id가 pagination-container인 ul 태그 안에 현재 페이지 - 2부터 + 2 까지 페이지 번호를 생성한다
function updatePagination(selectedPage, minPage, maxPage, func) {
    var paginationContainer = document.getElementById('pagination-container');
    paginationContainer.innerHTML = '';
    selectedPage = parseInt(selectedPage);
    
    for (let i = selectedPage - 2; i <= selectedPage + 2; i++) {
        if (i >= minPage && i <= maxPage) {
            let pageItem = document.createElement('li');
            pageItem.className = 'page-item';

            let pageLink = document.createElement('a');
            pageLink.className = 'page-link';

            pageLink.href = '#';
            pageLink.setAttribute('data-page', i);
            pageLink.textContent = i;

            if (i === selectedPage) {
                pageLink.style.color = "black";
                pageLink.style.backgroundColor = "transparent";
                pageLink.style.pointerEvents = "none";
                pageLink.setAttribute('data-current-page', 'true');
            }

            pageItem.appendChild(pageLink);
            paginationContainer.appendChild(pageItem);

            // 각 페이지 번호에 이벤트 리스너를 추가해야해서 함수로 따로 뺐음
            pageLink.addEventListener('click', function (event) {
                event.preventDefault();
                var selectedPage = this.getAttribute('data-page');
                func(selectedPage)
            })
        }
    }
}


// 댓글  요소 가져오기
function getCommentElementString(data, currentUserId, author) {
    let buttons = "";
    if (currentUserId === author) {
        buttons = getCommentButtonElementString(1, data.commentId);
    }    

    return `
        <div class="card mb-3">
            <div class="card-body">
                <div class="d-flex justify-content-between">
                    <h6 class="text-muted">작성자 : ${data.userId}</h6>
                    <h6 class="text-muted">작성일 : ${data.createDate}</h6>
                </div>
                <p class="card-text">${data.content}</p>
                <div class="d-flex justify-content-end">
                    ${buttons}
                </div>
            </div>
        </div>
    `;
    // * javascript:void(0) : 링크 기본 동작 방지
}


function getCommentButtonElementString(mode, commentId) {
    if (mode == 1) { // 평소
        return `
            <button type="button" class="btn btn-outline-secondary btn-sm me-2 edit-comment" data-comment-id="${commentId}"><i class="fas fa-edit"></i> 수정</button>
            <button type="button" class="btn btn-outline-danger btn-sm delete-comment" data-comment-id="${commentId}"><i class="fas fa-trash-alt"></i> 삭제</button>
        `;
    }
    else if (mode == 2) { // 수정할 때 버튼
        return `
            <button type="button" class="btn btn-primary btn-sm save-edit-comment me-2" data-comment-id="${commentId}">수정 완료</button>
            <button type="button" class="btn btn-secondary btn-sm cancel-edit-comment" data-comment-id="${commentId}">취소</button>
        `
    }
}
