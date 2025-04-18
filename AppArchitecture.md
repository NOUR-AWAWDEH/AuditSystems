# Audit System Architecture

## Solution Structure

```mermaid
graph TD
    A[AuditSystem Solution] --> B[Core]
    A --> C[Infrastructure]

    B --> D[AuditSystem.Domain]
    B --> E[AuditSystem.Application]
    B --> F[AuditSystem.Contract]
    B --> G[AuditSystem.BusinessLogic]

    C --> H[AuditSystem.DataAccess]
    C --> I[AuditSystem.Host]