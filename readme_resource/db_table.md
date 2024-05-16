```sql
CREATE TABLE users
(
	id NVARCHAR(20),
	password NVARCHAR(500) NOT NULL,
	name NVARCHAR(30) NOT NULL,
	PRIMARY KEY(id)
);


CREATE TABLE review
(
	id INT IDENTITY(1, 1),
	title NVARCHAR(200) NOT NULL,
	isbn10 NVARCHAR(10),
	isbn13 NVARCHAR(13),
	book_name NVARCHAR(200),
	user_id	NVARCHAR(20),
	content	NTEXT NOT NULL,
	rating	FLOAT NOT NULL,
	create_date DATE DEFAULT GETDATE(),
	PRIMARY KEY(id),
	FOREIGN KEY(user_id) REFERENCES users(id)
);


CREATE TABLE comment
(
	review_id INT,
	comment_id INT,
	user_id	NVARCHAR(20),
	content	NTEXT NOT NULL,
	create_date DATE DEFAULT GETDATE(),
	PRIMARY KEY(review_id, comment_id),
	FOREIGN KEY(review_id) REFERENCES review(id),
	FOREIGN KEY(user_id) REFERENCES users(id)
);


CREATE TABLE common_code
(
	type NVARCHAR(30),
	code NVARCHAR(30),
	value NVARCHAR(100),
	PRIMARY KEY(type, code)
);


CREATE TABLE menu
(
	id INT IDENTITY(1,1),
	level INT,
	menu_name NVARCHAR(20) NOT NULL UNIQUE,
	controller_name NVARCHAR(30),
	action_name NVARCHAR(30),
	parent_menu_id INT,
	PRIMARY KEY(id)
);
```
