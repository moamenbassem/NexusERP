🏗️ NexusERP: Full-Scale Enterprise Resource Planning System
NexusERP is a robust backend ecosystem built with ASP.NET Core 8. It manages the end-to-end business lifecycle—from procurement and inventory to high-level financial analytics and CRM.

🛠️ Core Architecture
Repository & Unit of Work: Ensures atomic transactions across multiple modules (e.g., Inventory, Sales, and Finance).
Clean Architecture: Strict separation of concerns using DTOs and manual mapping to keep the Domain Layer pure.
Security: Professional-grade JWT authentication with Role-Based Access Control (RBAC).
Audit Engine: Centralized logging service tracking administrative and financial actions.

🚀 Module Ecosystem
💰 Finance & Business Intelligence
Automated Invoicing: Logic for generating custom invoice numbers (e.g., INV-20260314-0001) and managing payment lifecycles.
Overdue Management: Automated system to flag unpaid invoices as "Overdue" based on DueDate and current UTC time.
Sales Reporting: Detailed logic to calculate Total Revenue, Total Units Sold, and Net Profit per Product or Category using CostPrice vs. Price margins.
Executive Dashboard: Aggregated data engine that provides:
Top 5 selling products by quantity and revenue.
Total profit/revenue within specific date ranges.
Financial exposure (Total Unpaid Invoices amount).

🤝 Customer Relationship Management (CRM)
Interaction Tracking: Logs every touchpoint with a customer, including manual notes by staff and automated system events.
Lifecycle History: Provides a DetailedHistoryDto that maps a customer’s entire journey, including orders, payments, and interactions.
Automation: AutoLog functionality that triggers during order fulfillment to maintain a seamless audit trail.s.

📦 Inventory & Purchasing
Stock Intelligence: Real-time stock valuation and adjustment logic triggered by sales or procurement.
Sales Analytics: Built-in reporting for Total Revenue, Profit, and unit sales per category.
Procurement Workflow: Management of Purchasing Orders (PO) with integrated inventory updates upon receipt.

🔐 Identity & Account Management
Hybrid User Model: A unified Identity system managing both internal Staff and external Customers through EF Core Inheritance.
Professional JWT Flow: Implements secure token generation with custom claims for roles and unique identifiers.

👥 Human Resources & Payroll
Timesheet Lifecycle: Employee submission and Admin approval/rejection workflows.
Dynamic Payroll: Automated salary calculation for Hourly and Monthly employees with integrated payment tracking.

📂 Feature Highlight: Data Continuity
The system is built to ensure no data is ever lost. For example, when an order is paid, the CRM, Finance, and Inventory modules are all updated in sync:
C#
// Seamless integration between CRM and Sales
var interaction = new CreateCustomerInteractionDto {
    CustomerId = order.CustomerId,
    Note = $"Customer paid order of Id: {order.Id}",
    Type = InteractionType.Note
};
await crmService.LogCustomerIntearactionAsync(interaction, UserId);

⚙️ How to Explore the Code
Domain Entities: Check MyERP.Domain to see the complex relationships between Employees, Customers, and Orders.
Service Layer: Check MyERP.Infrastructure for the business logic implementation (e.g., CRMService.cs, InventoryService.cs).
API Security: Look at AccountController to see how JWTs are handled.
