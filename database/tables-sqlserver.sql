create table [User]
(
	ID bigint identity primary key,
	UserName nvarchar(20) not null unique,
	Password nvarchar(32) not null,
	Email nvarchar(100) not null unique,
	WebSite nvarchar(100),
	Location nvarchar(100),
	Signature nvarchar(200),
	CreateDate datetime not null,
	Status tinyint not null
)

create table Topic
(
	ID bigint identity primary key,
	Category nvarchar(50) not null,
	Title nvarchar(100) not null,
	Content nvarchar(max) not null,
	CreateUser bigint not null,
	CreateDate datetime not null,
	UpdateDate datetime,	
	IsLock bit not null,
	IsRecommand bit not null,
	IsTop bit not null,
	IsDelete bit not null,
	LastReplyDate datetime,
	LastReplyUserID bigint
)

create table Comment
(
	ID bigint identity primary key,
	TopicID bigint not null,
	ReplyID bigint,
	Content nvarchar(max) not null,
	IsDelete bit not null,
	Createuser bigint not null,
	CreateDate datetime not null
)

create table TopicCollect
(
	UserID bigint not null,
	TopicID bigint not null,
	CreateDate datetime not null
	primary key(UserID, TopicID)
)

create table CommentVote
(
	UserID bigint not null,
	CommentID bigint not null,
	CreateDate datetime not null
	primary key(UserID, CommentID)
)