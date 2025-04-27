# AuditSystems - Audit Management System

## Overview
AuditSystems is a comprehensive audit management solution designed to streamline and automate audit processes within organizations. The system provides robust features for managing audit domains, departments, job scheduling, job prioritization, task management, and reporting.

## Project Structure
The project follows a clean architecture pattern with the following main components:

- **Core**
  - `AuditSystem.Application`: Contains application logic, CQRS handlers, and business rules
  - `AuditSystem.Contract`: Defines interfaces, models, and contracts used across the system

- **Infrastructure**
  - `AuditSystem.Host`: API controllers and web hosting infrastructure

## Key Features

### Audit Management
- Audit domain management
- Audit plan summaries
- Audit processes and sub-processes

### Job Management
- Job scheduling
- Job prioritization
- Audit job tracking
- Time allocation reporting

### Task Management
- Task creation and assignment
- Task tracking and status updates

### Skills Management
- Skill categories
- Skill sets
- Individual skills tracking

### Checklist Management
- Customizable checklists for audit procedures

## Technical Implementation
- Built with .NET using C# 12 features including primary constructors
- Implements CQRS pattern with MediatR
- Uses Ardalis.Result for standardized result handling
- RESTful API with proper authorization
- Pagination support for efficient data retrieval

## API Endpoints
The system exposes various API endpoints organized by domain:

- `/api/AuditDomain` - Audit domain management
- `/api/JobScheduling` - Job scheduling operations
- `/api/JobPrioritization` - Job prioritization management
- `/api/AuditJob` - Audit job tracking
- `/api/TaskManagement` - Task management
- `/api/SkillSet`, `/api/Skills`, `/api/SkillCategory` - Skills management
- `/api/Checklist` - Checklist management
- `/api/JobTimeAllocationReport` - Reporting
