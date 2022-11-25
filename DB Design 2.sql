insert into dbo.UserType (UserTypeCode, UserTypeName)
values	('A', 'Admin'), 
		('S', 'Assessee');


insert into dbo.[User](UserName, UserEmail, UserTypeCode)
values ('akumar', 'anuj.kumar@greychaindesign.com', 'S');


insert into dbo.QuestionType(QuestionTypeCode, QuestionTypeName)
values	('M', 'Multiple Choice'),
		('T', 'Text'), 
		('U', 'Yes No or Unable to answer');

select * from dbo.[User]

select * from dbo.AssessmentAnswer;
select * from dbo.Assessment;
select * from dbo.[User];
select * from dbo.UserType;
select * from dbo.Answer;
select * from dbo.AnswerType;
select * from dbo.Question;
select * from dbo.QuestionType;
select * from dbo.Inventory;