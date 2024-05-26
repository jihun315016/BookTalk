# BookTalk


## 프로젝트 소개
도서 검색 및 리뷰 웹 애플리케이션  
<br/>


## 작업자
강지훈  
<br/>


## 주요 기능
`사용자`  
회원 가입 및 로그인, 비밀번호 변경 등 사용자 관리 기능  
로그인 시 세션 ID를 쿠키에 저장하고, 세션 ID를 가지는 별도의 DB(MongoDB)에서 로그인 정보 저장  

`도서 검색`  
알라딘 Open API를 활용하여 신간 도서, 베스트셀러 조회 및 검색 구현  

`도서 리뷰`  
도서 리뷰에 관한 CRUD 기능  
리뷰 작성 시 tinyMCE 라이브러리를 통해 서식 구현  
<br/>  


## 사용 기술
`Language`  
- C#, javascript  

`Framework`  
- .NET 8  

`Database`  
- MSSQL, MongoDB  

`Tool`  
- Visual Studio 2022  

`API`  
- Aladin Open API, tinyMCE  
<br/>  


## 개발 화면 및 기능 시연
<video controls src="https://github.com/jihun315016/BookTalk/assets/97544861/72ac90a1-9510-4859-919f-8a7a0821729a.mp4" title="BookTalk01"></video>
<video controls src="https://github.com/jihun315016/BookTalk/assets/97544861/0395c343-d657-4b2e-9fcd-834239c5684f.mp4" title="BookTalk02"></video>
<br/>  


## 실행 방법
### 터미널에서 .NET EF Core CLI 설치  
```
dotnet tool install --local dotnet-ef
dotnet tool update --local dotnet-ef
```
<br/>

### 데이터베이스 생성   
SQL Server에서 데이터베이스를 생성하고 아래 링크에 작성된 쿼리를 실행하여 테이블 및 기초 데이터를 생성한다.  
- [테이블 생성 스크립트](./readme_resource/db_table.md)  
- [기초 데이터 생성 스크립트](./readme_resource/db_data.md)  
- [book_category 테이블 생성 및 기초 데이터](./readme_resource/)  
  - book_category.xlsx 파일의 데이터를 import하여 테이블 및 데이터를 준비한다.  
  - 필드는 category_id, category_name, mall 세 개의 필드로 구성된다.  
<br/>

### 리버스 엔지니어링
BookTalk.Shared 프로젝트 경로로 이동 후 터미널에서 명령을 실행한다.  
Server와 DB 명, 계정, 비밀번호는 각 사용자 환경을 따른다.  
예를 들어, 로컬 환경이라면 127.0.0.1을 입력한다.  
```
dotnet ef dbcontext scaffold "Server={Server};Database={DB명};User Id={계정};Password={비밀번호};TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c BookTalkDbContext --context-dir Contexts --force --project BookTalk.Shared.csproj --no-onconfiguring
```
<br/>

### 알라딘 Open API Key 발급
알라딘 로그인 후 아래 링크에서 API Key를 발급받을 수 있다.  
- [알라딘 API Key 발급받기](https://www.aladin.co.kr/ttb/wblog_manage.aspx)  
<br/>

### MongoDB Alas 사용을 위한 Connection String 발급
MongoDB Alas에 가입 후 데이터베이스와 컬렉션을 만들고 아래 링크를 참고하여 Connection String을 발급받는다.  
- [C#으로 MongoDB Atlas 연결하여 사용하기](https://itsjhstory.tistory.com/38)  
<br/>

### tinyMCE API Key 발급
아래 링크를 참고하여 tinyMCE Key를 발급받는다.  
- [tinyMCE 적용하기](https://itsjhstory.tistory.com/39)  
<br/>

### appsettings.json 설정
BookTalk.Client와 BookTalk.Server 프로젝트에 appsettings.json 파일을 생성하고 값을 입력한다.  
{}로 감싸져 있는 부분은 이 전에 발급받은 값들과 현재 개발 환경을 고려하여 작성한다.  

`BookTalk.Client`  
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ApiSettings": {
    "BaseUrl": "{BookTalk.Server URL}"
  },
  "Session": {
    "id": "SessionId"
  },
  "TinyMCE": {
    "ApiKey": "{tinyMCE API Key}"
  }
}
```

`BookTalk.Server`  
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ConnectionStrings": {
    "BookTalkDb": "Server={IP Address};Database={DB Name};User Id={User Name};Password={Password};TrustServerCertificate=True",
    "MongoDB": {
      "ConnStr": "{MongoDB Connection String}",
      "Database": "{DB Name}",
      "Collections": [ "Session" ]
    }
  },
  "Aladin": {
    "Key": "{Aladin API Key}",
    "ListType": "Book-Api-List",
    "SearchType": "Book-Api-Search",
    "DetailType": "Book-Api-Detail"
  }
}
```

