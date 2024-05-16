```sql
-- menu
INSERT INTO menu (level, menu_name, controller_name, action_name, parent_menu_id) VALUES
(0, N'사용자', '', '', 0),
(1, N'프로필', 'User', 'Index', 1),
(0, N'도서', '', '', 0),
(1, N'도서 검색', 'Book', 'SearchList', 3),
(0, N'게시판', '', '', 0),
(1, N'도서 리뷰', 'Review', 'Index', 5)


-- common_code 
INSERT INTO common_code (type, code, value) VALUES
('Review-Search-Type', N'1', N'검색 조건'),
('Review-Search-Type', N'2', N'제목 검색'),
('Review-Search-Type', N'3', N'도서 검색'),
('Book-Api-List', N'BaseUrl', N'https://www.aladin.co.kr/ttb/api/ItemList.aspx?'),
('Book-Api-List', N'Output', N'js'),
('Book-Api-List', N'MaxResult', N'6'),
('Book-Api-List', N'SearchTarget', N'Book'),
('Book-Api-List', N'Cover', N'Big'),
('Book-Api-Search', N'BaseUrl', N'http://www.aladin.co.kr/ttb/api/ItemSearch.aspx?'),
('Book-Api-Search', N'Output', N'js'),
('Book-Api-Search', N'MaxResult', N'6'),
('Book-Api-Search', N'SearchTarget', N'Book'),
('Book-Api-Search', N'QueryType', N'Title'),
('Book-Api-Search', N'Cover', N'Big'),
('Book-Api-Search', N'MinPage', N'1'),
('Book-Api-Search', N'MaxPage', N'6'),
('Book-Api-Detail', N'BaseUrl', N'http://www.aladin.co.kr/ttb/api/ItemLookUp.aspx?'),
('Book-Api-Detail', N'Output', N'js'),
('Book-Api-Detail', N'OptResult', N'ebookList,usedList,reviewList'),
('Book-Api-Detail', N'Cover', N'Big'),
('Comment-Info', N'PageUnit', N'5')
```