-- User Tables
-- done
-- UserRole Table
CREATE TABLE UserRole (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleName VARCHAR(50) NOT NULL UNIQUE, -- e.g., "Admin", "Auditor", "User"
    Description TEXT, -- Optional: Description of the role
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- Company Table
CREATE TABLE Company (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name VARCHAR(100)
);

-- done
-- Department Table
CREATE TABLE Department (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name VARCHAR(100),
    CompanyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Company(ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- SubDepartment Table
CREATE TABLE SubDepartment (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name VARCHAR(100),
    DepartmentID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Department(ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);


-- done
-- User Table
CREATE TABLE[User] (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserName VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE,
    UserRoleID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES UserRole(ID),
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    CompanyID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Company(ID),
    DepartmentID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Department(ID),
    SubDepartmentID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES SubDepartment(ID),
    LastLogin DATETIME,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- Admin and User Management Tables
CREATE TABLE AdminSettings (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- User Management Table
CREATE TABLE UserManagement (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    UserID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    Designation VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);


-- Done
-- Domain Table
CREATE TABLE Domain (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DomainName VARCHAR(255), -- strategic, operational, reporting, compliance
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- Audit Universe Table
CREATE TABLE AuditUniverse (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BusinessObjective VARCHAR(500),
    IndustryUpdate TEXT,
    CompanyUpdate TEXT,
    DomainID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Domain(ID),
    IsFinancialQuantifiable BIT,
    IsSpecialProject BIT DEFAULT 0, -- if true its means this SpecialProject
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- Audit Universe Objectives Table
CREATE TABLE AuditUniverseObjectives (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AuditUniverseID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AuditUniverse(ID),
    SerialNumber INT,
    Impact VARCHAR(5000),
    Amount INT,
    ImpactAmount INT,
    Percentage DECIMAL(5,2) DEFAULT 100.00,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- done
-- Compliance Checklist Table
CREATE TABLE ComplianceChecklist (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Area VARCHAR(255),
    Subject VARCHAR(255),
    Particulars TEXT,
    SerialNumber INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Compliance Audit Link Table
CREATE TABLE ComplianceAuditLink (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ComplianceID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES ComplianceChecklist(ID),
    AuditUniverseID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AuditUniverse(ID),
    IdentifiedThrough VARCHAR(255),
    InitiatedByID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Risk Assessments Table
CREATE TABLE RiskAssessments (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    BusinessObjective TEXT,
    NatureThrough TEXT,
    PerformRiskAssessment TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Specific Risk Factors Table
CREATE TABLE SpecificRiskFactors (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RiskAssessmentID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES RiskAssessments(ID),
    RiskFactor TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Audit Jobs Table
CREATE TABLE AuditJobs (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AuditUniverseID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AuditUniverse(ID),
    SerialNumber INT,
    JobName VARCHAR(255),
    JobType VARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Business Objective Table
CREATE TABLE BusinessObjective (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    Impact VARCHAR(5000),
    Amount INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Job Prioritization Table
CREATE TABLE JobPrioritization (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    AuditableUnit VARCHAR(255),
    BusinessObjectiveID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES BusinessObjective(ID),
    RiskRating VARCHAR(50),
    SelectForAudit BIT,
    Comments VARCHAR(200),
    SelectYear DATE,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Job Scheduling Table
CREATE TABLE JobScheduling (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    AuditableUnit VARCHAR(255),
    AuditYear DATE,
    PlannedStartDate DATE,
    PlannedEndDate DATE,
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Audit Plan Summary Table
CREATE TABLE AuditPlanSummary (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Component VARCHAR(255),
    Description TEXT,
    ExampleDetails TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Audit Engagement Table
CREATE TABLE AuditEngagement (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    JobName VARCHAR(255),
    PlannedStartDate DATE,
    PlannedEndDate DATE,
    JobType VARCHAR(100),
    SubLocation VARCHAR(255),
    Status VARCHAR(50),
    JobStatus VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Reporting Follow-Up Table
CREATE TABLE ReportingFollowUp (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Reporting TEXT,
    FollowUp TEXT,
    Status TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Internal Audit Consolidation Report Table
CREATE TABLE InternalAuditConsolidationReport (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    JobName VARCHAR(255),
    ReportName VARCHAR(255),
    ReportDate DATE,
    PreparedByID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Planning Report Table
CREATE TABLE PlanningReport (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    ReportName VARCHAR(255),
    ReportDate DATE,
    CreatedByID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Here Im

-- Done
-- Audit Exception Report Table
CREATE TABLE AuditExceptionReport (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    ReportName VARCHAR(255),
    ReportDate DATE,
    CreatedBy VARCHAR(255),
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Job Time Allocation Report Table
CREATE TABLE JobTimeAllocationReport (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    JobName VARCHAR(255),
    ReportName VARCHAR(255),
    ReportDate DATE,
    CreatedByID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Audit Plan Summary Report Table
CREATE TABLE AuditPlanSummaryReport (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    ReportName VARCHAR(255),
    ReportDate DATE,
    CreatedByID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    Status VARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Tasks Management Table
CREATE TABLE TasksManagement (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SerialNumber INT,
    Requirement VARCHAR(255),
    DueDate DATE,
    JobName VARCHAR(255),
    Assignee VARCHAR(255),
    AssignedBy UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [User](ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

---------------------------------------------------
-- Auditor Tables --
---------------------------------------------------

-- Done 
-- Risk Factor Table
CREATE TABLE RiskFactor (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    Factor TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Location and Process Tables
CREATE TABLE Location (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    Name VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- SubLocation and SubProcess Tables
CREATE TABLE SubLocation (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LocationID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Location(ID),
    Name VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);


-- Done
-- Process Table
CREATE TABLE Process (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    ProcessName VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- SubProcess Table
CREATE TABLE SubProcess (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProcessID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Process(ID),
    Particular VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Risk Control Matrix Table
CREATE TABLE RiskControlMatrix (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SubProcessID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES SubProcess(ID),
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Objective
CREATE TABLE Objective (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RiskControlMatrixID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES RiskControlMatrix(ID),
    Rating VARCHAR(50),
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Risk Table
CREATE TABLE Risk (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ObjectiveID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Objective(ID),
    Rating VARCHAR(50),
    RiskName VARCHAR(255),
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Risk Control Table
CREATE TABLE RiskControl (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RiskID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Risk(ID),
    Rating VARCHAR(50),
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Program Table
CREATE TABLE Program (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RiskControlID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES RiskControl(ID),
    Rating VARCHAR(50),
    ProgramName VARCHAR(255),
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Checklist Table
CREATE TABLE Checklist (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Area VARCHAR(255),
    Particulars VARCHAR(255),
    Observation TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Checklist Management Table
CREATE TABLE ChecklistManagement (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    ChecklistID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Checklist(ID),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Remark Table
CREATE TABLE Remark (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ChecklistManagementID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES ChecklistManagement(ID),
    RemarkType TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);


-- Done
-- Skills Master Table
CREATE TABLE Skill (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name VARCHAR(100) NOT NULL,
    Category VARCHAR(50), -- e.g., Technical, Soft Skills, Industry Knowledge
    Description TEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);

-- Done
-- Modified SkillSet Table
CREATE TABLE SkillSet (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserManagementID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES UserManagement(ID),
    SkillID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Skill(ID),
    ProficiencyLevel VARCHAR(20), -- e.g., Beginner, Intermediate, Expert
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);


-- Done
-- Supporting Document Table
CREATE TABLE SupportingDoc (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdminSettingsID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES AdminSettings(ID),
    FileName VARCHAR(255),
    FileSize INT,
    URL VARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
);
