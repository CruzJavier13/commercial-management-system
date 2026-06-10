CREATE DATABASE variedadesjennifer_db;
GO
USE variedadesjennifer_db;
GO

-- =========================================================================
-- DEFINICIÓN DE ESQUEMAS (LÍMITES DE CONTEXTOS / BOUNDED CONTEXTS)
-- =========================================================================
CREATE SCHEMA emp; -- Module Employee
GO
CREATE SCHEMA cst; -- Module Customers
GO
CREATE SCHEMA prd; -- Module Products
GO
CREATE SCHEMA inv; -- Module Inventory
GO
CREATE SCHEMA bil; -- Module Billing
GO

--==========================================================================
-- Module Employee (emp)
--==========================================================================

CREATE TABLE emp.Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleCode VARCHAR(20) NOT NULL UNIQUE, -- 'CASHIER', 'SUPERVISOR', 'ADMIN'
    Name VARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE emp.Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeCode VARCHAR(50) NOT NULL UNIQUE, 
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    IdentificationNumber VARCHAR(30) NOT NULL UNIQUE,
    SocialSecurity VARCHAR(20) NULL,
    Phone VARCHAR(20) NOT NULL,
    Address VARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
);
GO

CREATE TABLE emp.session_auth(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    RoleId INT NOT NULL,
    SystemUsername VARCHAR(50) NULL UNIQUE, 
    PasswordHash VARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Employees_Roles FOREIGN KEY (RoleId) REFERENCES emp.Roles(Id),
    CONSTRAINT FK_Session_auth_Employee FOREIGN KEY (EmployeeId) REFERENCES emp.Employees(Id)
)

CREATE INDEX IX_Session_auth_Role ON emp.session_auth(RoleId);
CREATE INDEX IX_Session_auth_Username ON emp.session_auth(SystemUsername);
GO

-- =========================================================================
-- Module Customers (cst)
-- =========================================================================
CREATE TABLE cst.Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerCode VARCHAR(50) NOT NULL UNIQUE,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    IdentificationNumber VARCHAR(30) NULL, -- Cédula / RUC
    Email VARCHAR(150) NULL,
    PhoneNumber VARCHAR(30) NULL,
    Address NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- =========================================================================
-- Module Products (prd)
-- =========================================================================
CREATE TABLE prd.Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryCode VARCHAR(20) NOT NULL UNIQUE, 
    Name VARCHAR(100) NOT NULL, 
    Description NVARCHAR(250) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE prd.Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplierCode VARCHAR(50) NOT NULL UNIQUE, 
    CompanyName VARCHAR(150) NOT NULL,
    TaxIdentification VARCHAR(30) NOT NULL UNIQUE, 
    Email VARCHAR(150) NULL,
    PhoneNumber VARCHAR(30) NULL,
    Address NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE prd.Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductCode VARCHAR(50) NOT NULL UNIQUE, 
    CategoryId INT NOT NULL, 
    SupplierId INT NOT NULL, 
    Name VARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    BasePrice DECIMAL(18,2) NOT NULL, 
    IsVirtualService BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES prd.Categories(Id),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierId) REFERENCES prd.Suppliers(Id)
);
GO

CREATE TABLE prd.MedicineAttributes (
    ProductId INT PRIMARY KEY,
    HealthRegisterNumber VARCHAR(50) NOT NULL, 
    ActiveIngredient VARCHAR(150) NOT NULL, 
    ExpirationDateRequired BIT NOT NULL DEFAULT 1,
    RequiresPrescription BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    --CONSTRAINT FK_MedicineAttributes_Products FOREIGN KEY (ProductId) REFERENCES prd.Products(Id) ON DELETE CASCADE
);
GO

CREATE TABLE prd.DeviceAttributes (
    ProductId INT PRIMARY KEY,
    Brand VARCHAR(100) NOT NULL, 
    Model VARCHAR(100) NOT NULL,
    SerialNumberOrIMEI VARCHAR(100) NULL,
    WarrantyPeriodMonths INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_DeviceAttributes_Products FOREIGN KEY (ProductId) REFERENCES prd.Products(Id) ON DELETE CASCADE
);
GO

CREATE INDEX IX_Products_Category ON prd.Products(CategoryId);
CREATE INDEX IX_Products_Supplier ON prd.Products(SupplierId);
GO

-- =========================================================================
-- Module Inventory (inv)
-- =========================================================================
CREATE TABLE inv.ProductStocks (
    ProductId INT PRIMARY KEY, 
    CurrentStock INT NOT NULL DEFAULT 0,
    MinimumRequired INT NOT NULL DEFAULT 5,
    LastUpdate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_PositiveStock CHECK (CurrentStock >= 0) 
);
GO

CREATE TABLE inv.StockMovements (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL, 
    SupplierId INT NULL,  
    MovementType VARCHAR(20) NOT NULL,
    Quantity INT NOT NULL,
    UnitCost DECIMAL(18,2) NOT NULL DEFAULT 0.00, 
    TotalCost AS (Quantity * UnitCost),
    Concept VARCHAR(250) NOT NULL, 
    ReferenceDocument VARCHAR(50) NULL, 
    MovementDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_PositiveQuantity CHECK (Quantity > 0),
    CONSTRAINT CK_MovementType CHECK (MovementType IN ('IN', 'OUT'))
);
GO

CREATE INDEX IX_StockMovements_Product ON inv.StockMovements(ProductId);
CREATE INDEX IX_StockMovements_Supplier ON inv.StockMovements(SupplierId);
CREATE INDEX IX_StockMovements_Date ON inv.StockMovements(MovementDate);

GO

-- =========================================================================
-- Module bill (bil)
-- =========================================================================
CREATE TABLE bil.Invoices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber VARCHAR(50) NOT NULL UNIQUE, 
    CustomerId INT NOT NULL,
    EmployeeId INT NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL, 
    SubTotalAmount DECIMAL(18,2) NOT NULL,
    TotalBilled DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(20) NOT NULL, 
    InvoiceDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_PaymentMethod CHECK (PaymentMethod IN ('Cash', 'Card', 'Transfer'))
);
GO

CREATE TABLE bil.InvoiceDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    PriceBilled DECIMAL(18,2) NOT NULL,
    TaxRate DECIMAL(5,2) NOT NULL DEFAULT 15.00,
    LineTotal AS (Quantity * PriceBilled),
    --CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceId) REFERENCES bil.Invoices(Id) ON DELETE CASCADE,
    CONSTRAINT CK_PositiveInvoiceQuantity CHECK (Quantity > 0)
);
GO

CREATE INDEX IX_Invoices_Customer ON bil.Invoices(CustomerId);
CREATE INDEX IX_Invoices_Date ON bil.Invoices(InvoiceDate);
CREATE INDEX IX_InvoiceDetails_Invoice ON bil.InvoiceDetails(InvoiceId);
CREATE INDEX IX_InvoiceDetails_Product ON bil.InvoiceDetails(ProductId); 
GO