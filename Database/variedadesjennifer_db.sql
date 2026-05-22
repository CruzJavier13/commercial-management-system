CREATE DATABASE variedadesjennifer_db;
GO
USE variedadesjennifer_db;
GO

-- =========================================================================
-- SCHEMAS DEFINITION (VERTICAL CORE BUSINESS BOUNDARIES)
-- =========================================================================
CREATE SCHEMA cst; -- Customers Module Schema
GO
CREATE SCHEMA prd; -- Products / Catalog Module Schema
GO
CREATE SCHEMA inv; -- Inventory / Stock Movement Module Schema
GO
CREATE SCHEMA sal; -- Sales / Orders Module Schema
GO
CREATE SCHEMA bil; -- Billing / Fiscal Invoicing Module Schema
GO

-- =========================================================================
-- CUSTOMERS MODULE (cst)
-- =========================================================================
CREATE TABLE cst.Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerCode VARCHAR(50) NOT NULL UNIQUE,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    IdentificationNumber VARCHAR(30) NULL, -- Identification Card (Cédula) / RUC
    Email VARCHAR(150) NULL,
    PhoneNumber VARCHAR(30) NULL,
    Address NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- =========================================================================
-- PRODUCTS MODULE (prd)
-- =========================================================================
-- TABLE: prd.Categories
CREATE TABLE prd.Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryCode VARCHAR(20) NOT NULL UNIQUE, 
    Name VARCHAR(100) NOT NULL, -- 'Medicaments', 'Electronics', 'Food', 'Virtual Services'
    Description NVARCHAR(250) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- TABLE: prd.Suppliers
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

-- TABLE: prd.Products
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

-- TABLE: prd.MedicineAttributes
CREATE TABLE prd.MedicineAttributes (
    ProductId INT PRIMARY KEY,
    HealthRegisterNumber VARCHAR(50) NOT NULL, 
    ActiveIngredient VARCHAR(150) NOT NULL, 
    ExpirationDateRequired BIT NOT NULL DEFAULT 1,
    RequiresPrescription BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_MedicineAttributes_Products FOREIGN KEY (ProductId) REFERENCES prd.Products(Id)
);
GO

-- TABLE: prd.DeviceAttributes
CREATE TABLE prd.DeviceAttributes (
    ProductId INT PRIMARY KEY,
    Brand VARCHAR(100) NOT NULL, 
    Model VARCHAR(100) NOT NULL,
    SerialNumberOrIMEI VARCHAR(100) NULL,
    WarrantyPeriodMonths INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_DeviceAttributes_Products FOREIGN KEY (ProductId) REFERENCES prd.Products(Id)
);
GO

-- PERFOMANCE INDEXES
CREATE INDEX IX_Products_Category ON prd.Products(CategoryId);
CREATE INDEX IX_Products_Supplier ON prd.Products(SupplierId);
GO

-- =========================================================================
-- INVENTORY MODULE (inv)
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
-- PERFORMANCE INDEXES
CREATE INDEX IX_StockMovements_Product ON inv.StockMovements(ProductId);
CREATE INDEX IX_StockMovements_Supplier ON inv.StockMovements(SupplierId);
CREATE INDEX IX_StockMovements_Date ON inv.StockMovements(MovementDate);
GO
-- =========================================================================
-- SALES MODULE (sal)
-- =========================================================================
CREATE TABLE sal.Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber VARCHAR(50) NOT NULL UNIQUE,
    CustomerId INT NOT NULL, 
    OrderStatus VARCHAR(20) NOT NULL, 
    SubTotal DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalAmount DECIMAL(18,2) NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_OrderStatus CHECK (OrderStatus IN ('Pending', 'Invoiced', 'Cancelled'))
);
GO

CREATE TABLE sal.OrderDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL, 
    Quantity INT NOT NULL,
    PriceAtSale DECIMAL(18,2) NOT NULL, 
    LineTotal AS (Quantity * PriceAtSale), 
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES sal.Orders(Id),
    CONSTRAINT CK_PositiveSaleQuantity CHECK (Quantity > 0)
);
GO
-- PERFORMANCE INDEXES
CREATE INDEX IX_Orders_Customer ON sal.Orders(CustomerId);
CREATE INDEX IX_Orders_Date ON sal.Orders(OrderDate);
CREATE INDEX IX_OrderDetails_Order ON sal.OrderDetails(OrderId);
CREATE INDEX IX_OrderDetails_Product ON sal.OrderDetails(ProductId);
GO

-- =========================================================================
-- BILLING MODULE (bil)
-- =========================================================================
CREATE TABLE bil.Invoices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber VARCHAR(50) NOT NULL UNIQUE, 
    OrderId INT NOT NULL, 
    CustomerId INT NOT NULL, 
    TaxAmount DECIMAL(18,2) NOT NULL, 
    SubTotalAmount DECIMAL(18,2) NOT NULL,
    TotalBilled DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(20) NOT NULL, 
    InvoiceDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
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
    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceId) REFERENCES bil.Invoices(Id),
    CONSTRAINT CK_PositiveInvoiceQuantity CHECK (Quantity > 0)
);
GO
-- PERFORMANCE INDEXES
CREATE INDEX IX_Invoices_Order ON bil.Invoices(OrderId);
CREATE INDEX IX_Invoices_Customer ON bil.Invoices(CustomerId);
CREATE INDEX IX_Invoices_Date ON bil.Invoices(InvoiceDate);
CREATE INDEX IX_InvoiceDetails_Invoice ON bil.InvoiceDetails(InvoiceId);
GO

-- =========================================================================