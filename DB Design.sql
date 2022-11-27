drop table dbo.AssessmentAnswer;
drop table dbo.Assessment;
drop table dbo.[User];
drop table dbo.UserType;
drop table dbo.Answer;
drop table dbo.AnswerType;
drop table dbo.QuestionSuggestedAnswer;
drop table dbo.Question;
drop table dbo.QuestionType;
drop table dbo.Inventory;

go

Create table dbo.Inventory
(
	InventoryId				bigint identity(1, 1), 
	SectionName				nvarchar(50), 
	InventoryName			nvarchar(100), 
	InventoryDescription	nvarchar(1000), 
	TimeRequiredInMinutes	int, 
	ParentInventoryId		bigint,

	constraint pk_Inventory primary key (InventoryId),
    constraint fk_Inventory_ParentInventoryId foreign key (ParentInventoryId) references dbo.Inventory(InventoryId)
	
);
go


create table dbo.QuestionType
(
	QuestionTypeCode	char(1) not null,
	QuestionTypeName	nvarchar(100), 

	constraint pk_QuestionType primary key(QuestionTypeCode)
);
go


Create table dbo.Question
(
	QuestionId			bigint identity(1, 1), 
	InventoryId			bigint not null, 
	Title				nvarchar(500), 
	[Description]		nvarchar(4000),
	QuestionTypeCode	char(1) not null, 
	QuestionCategory	nvarchar(50) null, 
	TimeRequiredInMinutes	int, 
	DisplaySequence		int, 

	constraint pk_Question primary key(QuestionId), 
	constraint uk_Question unique(InventoryId, DisplaySequence), 
	constraint fk_Question_InventoryId foreign key (InventoryId) references dbo.Inventory(InventoryId), 
	constraint fk_Question_QuestionTypeCode foreign key (QuestionTypeCode) references dbo.QuestionType(QuestionTypeCode)
);
go

create table dbo.QuestionSuggestedAnswer
(
	QuestionSuggestedAnswerId		bigint identity(1, 1), 
	QuestionId		bigint not null, 
	Title			nvarchar(500), 
	[Description]	nvarchar(3000),
	Score			decimal(28, 8),
	DisplaySequence		int, 

	constraint uk_QuestionSuggestedAnswer unique(QuestionId, QuestionSuggestedAnswerId), 
	constraint uk_QuestionSuggestedAnswer_Display unique(QuestionId, DisplaySequence), 
	constraint pk_QuestionSuggestedAnswer primary key(QuestionSuggestedAnswerId), 
	constraint fk_QuestionSuggestedAnswer_QuestionId foreign key (QuestionId) references dbo.Question(QuestionId)
);
go

create table dbo.AnswerType
(
	AnswerTypeCode	char(1) not null,
	AnswerTypeName	nvarchar(100), 

	constraint pk_AnswerType primary key(AnswerTypeCode)
);
go

create table dbo.Answer
(
	AnswerId		bigint identity(1, 1), 
	QuestionId		bigint not null, 
	Title			nvarchar(500), 
	[Description]	nvarchar(3000),
	AnswerTypeCode	char(1) not null, 
	Score			decimal(28, 8)

	constraint pk_Answer primary key(AnswerId), 
	constraint fk_Answer_QuestionId foreign key (QuestionId) references dbo.Question(QuestionId), 
	constraint fk_Answer_AnswerTypeCode foreign key (AnswerTypeCode) references dbo.AnswerType(AnswerTypeCode)
);
go


create table dbo.UserType
(
	UserTypeCode char(1) not null, 
	UserTypeName nvarchar(100), 

	constraint pk_UserTypeCode primary key(UserTypeCode)
);
go


create table dbo.[User]
(
	UserId		bigint identity(1, 1), 
	UserName	varchar(100) not null, 
	UserEmail	varchar(200) not null,
	UserTypeCode	char(1) not null,

	constraint pk_User primary key(UserId), 
	constraint fk_User_UserTypeCode foreign key (UserTypeCode) references dbo.UserType(UserTypeCode)
);
go

create table dbo.Assessment
(
	AssessmentId bigint identity(1, 1), 
	UserId			bigint not null,
	QuestionId		bigint not null,
	StartedOn		datetime2 not null, 
	CompletedOn		datetime2 null, 
	IsPaused		bit not null default(0), 
	LastPausedAt	datetime2 null, 
	PauseCount		int null, 
	EarnedScore		decimal(28, 8), 

	constraint pk_Assessment primary key(AssessmentId), 
	constraint fk_Assessment_QuestionId foreign key (QuestionId) references dbo.Question(QuestionId), 
	constraint fk_Assessment_UserId foreign key (UserId) references dbo.[User](UserId)
);


create table dbo.AssessmentAnswer
(
	AssessmentAnswerId		bigint identity(1, 1), 
	AssessmentId			bigint not null,
	AnswerId				bigint null,
	SubmittedOn				datetime2 not null, 
	
	constraint pk_AssessmentAnswerId primary key(AssessmentAnswerId), 
	constraint fk_AssessmentAnswer_AssessmentId foreign key (AssessmentId) references dbo.Assessment(AssessmentId), 
	constraint fk_Assessment_AnswerId foreign key (AnswerId) references dbo.Answer(AnswerId)
)
