USE ProjectManagerDB

create table Employees(
	Id INT not null primary key IDENTITY,
	FirstName NVARCHAR(MAX) not null,
	SurName NVARCHAR(MAX) not null,
	PatronymicName NVARCHAR(MAX),
	Email NVARCHAR(MAX) not null
	)

	create table Projects(
	Id INT not null primary key IDENTITY,
	Name NVARCHAR(MAX) not null,
	CustomerCompanyName NVARCHAR(MAX) not null,
	ExecutorCompanyName NVARCHAR(MAX),
	ProjectManagerId int,
	CreatedAd datetime NOT NULL,
	DateStart datetime not null,
	DateEnd datetime not null,
	PriorityId int not null,
	Priority int not null,
	PriorityUpdate datetime not null,
	CONSTRAINT FK_ProjectManager FOREIGN KEY (ProjectManagerId) 
        REFERENCES Employees (Id) ON UPDATE CASCADE ON DELETE SET NULL
	)

	create table EmployeeProjects(
	EmployeeId int not null,
	ProjectId int not null,
	CreatedAd datetime NOT NULL,
	INDEX ProjectId (ProjectId),
    INDEX EmployeeId (EmployeeId),
	CONSTRAINT FK_Project FOREIGN KEY (ProjectId) 
        REFERENCES Projects (Id) ON DELETE CASCADE,
	CONSTRAINT FK_Employee FOREIGN KEY (EmployeeId) 
        REFERENCES Employees (Id) ON DELETE CASCADE,
	CONSTRAINT "PK_EmployeeProject" PRIMARY KEY(EmployeeId, ProjectId)
	)


