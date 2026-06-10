USE variedadesjennifer_db;
GO
--==========================================================================
-- Module Employee 
--==========================================================================
CREATE OR ALTER PROCEDURE emp.usp_Roles_Upsert
    @Id INT = NULL,
    @RoleCode VARCHAR(20),
    @Name VARCHAR(100),
    @Description NVARCHAR(250) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO emp.Roles (RoleCode, Name, Description, IsActive)
            VALUES (@RoleCode, @Name, @Description, @IsActive);
            SELECT SCOPE_IDENTITY() AS GeneratedRoleId;
        END
        ELSE
        BEGIN
            UPDATE emp.Roles
            SET RoleCode = @RoleCode,
                Name = @Name,
                Description = @Description,
                IsActive = @IsActive
            WHERE Id = @Id;
            SELECT @Id AS GeneratedRoleId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- Procedure Save
-- =========================================================================
CREATE OR ALTER PROCEDURE emp.usp_Employees_Save
    @Id INT = NULL,
    @EmployeeCode VARCHAR(50),
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @IdentificationNumber VARCHAR(30),
    @SocialSecurity VARCHAR(20) = NULL,
    @Phone VARCHAR(20),
    @Address VARCHAR(255) = NULL,
    @IsActive BIT = 1,

    @RoleId INT,
    @SystemUsername VARCHAR(50),
    @PasswordHash VARCHAR(255) = NULL 
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @CurrentEmployeeId INT = @Id;


        IF @Id IS NULL OR @Id = 0
        BEGIN

            IF EXISTS (SELECT 1 FROM emp.session_auth WHERE SystemUsername = @SystemUsername)
            BEGIN
                RAISERROR('El nombre de usuario para el sistema ya se encuentra registrado.', 16, 1);
            END


            INSERT INTO emp.Employees (EmployeeCode, FirstName, LastName, IdentificationNumber, SocialSecurity, Phone, Address, IsActive)
            VALUES (@EmployeeCode, @FirstName, @LastName, @IdentificationNumber, @SocialSecurity, @Phone, @Address, @IsActive);
            
            SET @CurrentEmployeeId = SCOPE_IDENTITY();


            INSERT INTO emp.session_auth (EmployeeId, RoleId, SystemUsername, PasswordHash, IsActive)
            VALUES (@CurrentEmployeeId, @RoleId, @SystemUsername, @PasswordHash, @IsActive);
        END
        

        ELSE
        BEGIN

            UPDATE emp.Employees
            SET EmployeeCode = @EmployeeCode,
                FirstName = @FirstName,
                LastName = @LastName,
                IdentificationNumber = @IdentificationNumber,
                SocialSecurity = @SocialSecurity,
                Phone = @Phone,
                Address = @Address,
                IsActive = @IsActive
            WHERE Id = @CurrentEmployeeId;

            UPDATE emp.session_auth
            SET RoleId = @RoleId,
                SystemUsername = @SystemUsername,
                PasswordHash = ISNULL(@PasswordHash, PasswordHash), 
                IsActive = @IsActive
            WHERE EmployeeId = @CurrentEmployeeId;
        END

        COMMIT TRANSACTION;
        
        SELECT @CurrentEmployeeId AS EmployeeId;

    END TRY
    BEGIN CATCH
       
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- Procedure Auth
-- =========================================================================
CREATE OR ALTER PROCEDURE emp.usp_Auth_Login
    @SystemUsername VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.Id AS EmployeeId,
        e.EmployeeCode,
        e.FirstName,
        e.LastName,
        sa.PasswordHash,
        sa.IsActive AS IsSessionActive,
        e.IsActive AS IsEmployeeActive,
        r.Id AS RoleId,
        r.RoleCode,
        r.Name AS RoleName
    FROM emp.session_auth sa
    INNER JOIN emp.Employees e ON sa.EmployeeId = e.Id
    INNER JOIN emp.Roles r ON sa.RoleId = r.Id
    WHERE sa.SystemUsername = @SystemUsername;
END;
GO

-- =========================================================================
-- Module Clients
-- =========================================================================
CREATE OR ALTER PROCEDURE cst.usp_Customers_Upsert
    @Id INT = NULL,
    @CustomerCode VARCHAR(50),
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @IdentificationNumber VARCHAR(30) = NULL,
    @Email VARCHAR(150) = NULL,
    @PhoneNumber VARCHAR(30) = NULL,
    @Address NVARCHAR(500) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO cst.Customers (CustomerCode, FirstName, LastName, IdentificationNumber, Email, PhoneNumber, Address, IsActive)
            VALUES (@CustomerCode, @FirstName, @LastName, @IdentificationNumber, @Email, @PhoneNumber, @Address, @IsActive);
            
            SELECT SCOPE_IDENTITY() AS GeneratedId;
        END
        ELSE
        BEGIN
            UPDATE cst.Customers
            SET CustomerCode = @CustomerCode,
                FirstName = @FirstName,
                LastName = @LastName,
                IdentificationNumber = @IdentificationNumber,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                Address = @Address,
                IsActive = @IsActive
            WHERE Id = @Id;

            SELECT @Id AS GeneratedId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- Module Products
-- =========================================================================
CREATE OR ALTER PROCEDURE prd.usp_Categories_Upsert
    @Id INT = NULL,
    @CategoryCode VARCHAR(20),
    @Name VARCHAR(100),
    @Description NVARCHAR(250) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO prd.Categories (CategoryCode, Name, Description, IsActive)
            VALUES (@CategoryCode, @Name, @Description, @IsActive);
            SELECT SCOPE_IDENTITY() AS GeneratedId;
        END
        ELSE
        BEGIN
            UPDATE prd.Categories
            SET CategoryCode = @CategoryCode, Name = @Name, Description = @Description, IsActive = @IsActive
            WHERE Id = @Id;
            SELECT @Id AS GeneratedId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE prd.usp_Suppliers_Upsert
    @Id INT = NULL,
    @SupplierCode VARCHAR(50),
    @CompanyName VARCHAR(150),
    @TaxIdentification VARCHAR(30),
    @Email VARCHAR(150) = NULL,
    @PhoneNumber VARCHAR(30) = NULL,
    @Address NVARCHAR(500) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO prd.Suppliers (SupplierCode, CompanyName, TaxIdentification, Email, PhoneNumber, Address, IsActive)
            VALUES (@SupplierCode, @CompanyName, @TaxIdentification, @Email, @PhoneNumber, @Address, @IsActive);
            SELECT SCOPE_IDENTITY() AS GeneratedId;
        END
        ELSE
        BEGIN
            UPDATE prd.Suppliers
            SET SupplierCode = @SupplierCode, CompanyName = @CompanyName, TaxIdentification = @TaxIdentification, 
                Email = @Email, PhoneNumber = @PhoneNumber, Address = @Address, IsActive = @IsActive
            WHERE Id = @Id;
            SELECT @Id AS GeneratedId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE prd.usp_Products_Save
    @Id INT = NULL,
    @ProductCode VARCHAR(50),
    @CategoryId INT,
    @SupplierId INT,
    @Name VARCHAR(150),
    @Description NVARCHAR(500) = NULL,
    @BasePrice DECIMAL(18,2),
    @IsVirtualService BIT = 0,
    @IsActive BIT = 1,

    @HealthRegisterNumber VARCHAR(50) = NULL,
    @ActiveIngredient VARCHAR(150) = NULL,
    @ExpirationDateRequired BIT = 1,
    @RequiresPrescription BIT = 0,
    @Brand VARCHAR(100) = NULL,
    @Model VARCHAR(100) = NULL,
    @SerialNumberOrIMEI VARCHAR(100) = NULL,
    @WarrantyPeriodMonths INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @GeneratedProductId INT = @Id;

        -- Insert or Update
        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO prd.Products (ProductCode, CategoryId, SupplierId, Name, Description, BasePrice, IsVirtualService, IsActive)
            VALUES (@ProductCode, @CategoryId, @SupplierId, @Name, @Description, @BasePrice, @IsVirtualService, @IsActive);
            
            SET @GeneratedProductId = SCOPE_IDENTITY();
            
       
        END
        ELSE
        BEGIN
            UPDATE prd.Products
            SET ProductCode = @ProductCode, CategoryId = @CategoryId, SupplierId = @SupplierId,
                Name = @Name, Description = @Description, BasePrice = @BasePrice,
                IsVirtualService = @IsVirtualService, IsActive = @IsActive
            WHERE Id = @Id;
        END

        DELETE FROM prd.MedicineAttributes WHERE ProductId = @GeneratedProductId;
        DELETE FROM prd.DeviceAttributes WHERE ProductId = @GeneratedProductId;

        IF @HealthRegisterNumber IS NOT NULL AND @ActiveIngredient IS NOT NULL
        BEGIN
            INSERT INTO prd.MedicineAttributes (ProductId, HealthRegisterNumber, ActiveIngredient, ExpirationDateRequired, RequiresPrescription)
            VALUES (@GeneratedProductId, @HealthRegisterNumber, @ActiveIngredient, @ExpirationDateRequired, @RequiresPrescription);
        END

        IF @Brand IS NOT NULL AND @Model IS NOT NULL
        BEGIN
            INSERT INTO prd.DeviceAttributes (ProductId, Brand, Model, SerialNumberOrIMEI, WarrantyPeriodMonths)
            VALUES (@GeneratedProductId, @Brand, @Model, @SerialNumberOrIMEI, @WarrantyPeriodMonths);
        END

        COMMIT TRANSACTION;
        SELECT @GeneratedProductId AS ProductId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- Module Inventory
-- =========================================================================

CREATE OR ALTER PROCEDURE inv.usp_ProductStocks_Initialize
    @ProductId INT,
    @MinimumRequired INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM inv.ProductStocks WHERE ProductId = @ProductId)
        BEGIN
            INSERT INTO inv.ProductStocks (ProductId, CurrentStock, MinimumRequired)
            VALUES (@ProductId, 0, @MinimumRequired);
        END
        ELSE
        BEGIN
            UPDATE inv.ProductStocks
            SET MinimumRequired = @MinimumRequired,
                LastUpdate = SYSUTCDATETIME()
            WHERE ProductId = @ProductId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE inv.usp_StockMovements_Register
    @ProductId INT,
    @SupplierId INT = NULL,
    @MovementType VARCHAR(20), -- 'IN' / 'OUT'
    @Quantity INT,
    @UnitCost DECIMAL(18,2) = 0.00,
    @Concept VARCHAR(250),
    @ReferenceDocument VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;


        IF NOT EXISTS(SELECT 1 FROM inv.ProductStocks WHERE ProductId = @ProductId)
        BEGIN
            RAISERROR('Control de inventario no inicializado para este producto en el módulo de Stock.', 16, 1);
        END


        IF @MovementType = 'IN'
        BEGIN
            UPDATE inv.ProductStocks
            SET CurrentStock = CurrentStock + @Quantity,
                LastUpdate = SYSUTCDATETIME()
            WHERE ProductId = @ProductId;
        END
        ELSE IF @MovementType = 'OUT'
        BEGIN

            UPDATE inv.ProductStocks
            SET CurrentStock = CurrentStock - @Quantity,
                LastUpdate = SYSUTCDATETIME()
            WHERE ProductId = @ProductId;
        END
        ELSE
        BEGIN
            RAISERROR('El tipo de movimiento debe ser estrictamente ''IN'' o ''OUT''.', 16, 1);
        END


        INSERT INTO inv.StockMovements (ProductId, SupplierId, MovementType, Quantity, UnitCost, Concept, ReferenceDocument)
        VALUES (@ProductId, @SupplierId, @MovementType, @Quantity, @UnitCost, @Concept, @ReferenceDocument);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO

CREATE TYPE bil.InvoiceDetailType AS TABLE
(
    ProductId INT,
    Quantity INT,
    PriceBilled DECIMAL(18,2),
    TaxRate DECIMAL(5,2)
);
GO

CREATE PROCEDURE bil.sp_Invoice_Insert
    @InvoiceNumber VARCHAR(50),
    @CustomerId INT,
    @EmployeeId INT,
    @TaxAmount DECIMAL(18,2),
    @SubTotalAmount DECIMAL(18,2),
    @TotalBilled DECIMAL(18,2),
    @PaymentMethod VARCHAR(20),
    @InvoiceDate DATETIME2,
    @Details bil.InvoiceDetailType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO bil.Invoices (InvoiceNumber, CustomerId, TaxAmount, SubTotalAmount, TotalBilled, PaymentMethod, InvoiceDate)
        VALUES (@InvoiceNumber, @CustomerId, @TaxAmount, @SubTotalAmount, @TotalBilled, @PaymentMethod, @InvoiceDate);

        DECLARE @NewInvoiceId INT = SCOPE_IDENTITY();

        INSERT INTO bil.InvoiceDetails (InvoiceId, ProductId, Quantity, PriceBilled, TaxRate)
        SELECT @NewInvoiceId, ProductId, Quantity, PriceBilled, TaxRate
        FROM @Details;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE bil.sp_Invoice_Update
    @Id INT,
    @InvoiceNumber VARCHAR(50),
    @CustomerId INT,
    @EmployeeId INT,
    @TaxAmount DECIMAL(18,2),
    @SubTotalAmount DECIMAL(18,2),
    @TotalBilled DECIMAL(18,2),
    @PaymentMethod VARCHAR(20),
    @InvoiceDate DATETIME2,
    @Details bil.InvoiceDetailType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE bil.Invoices
        SET InvoiceNumber = @InvoiceNumber, CustomerId = @CustomerId, 
            TaxAmount = @TaxAmount, SubTotalAmount = @SubTotalAmount, TotalBilled = @TotalBilled, 
            PaymentMethod = @PaymentMethod, InvoiceDate = @InvoiceDate
        WHERE Id = @Id;

        DELETE FROM bil.InvoiceDetails WHERE InvoiceId = @Id;

        INSERT INTO bil.InvoiceDetails (InvoiceId, ProductId, Quantity, PriceBilled, TaxRate)
        SELECT @Id, ProductId, Quantity, PriceBilled, TaxRate
        FROM @Details;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE bil.sp_Invoice_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE bil.Invoices
    SET IsActive = 0
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE bil.sp_Invoice_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, InvoiceNumber, CustomerId, TaxAmount, SubTotalAmount, TotalBilled, PaymentMethod, InvoiceDate 
    FROM bil.Invoices WHERE Id = @Id;

    SELECT Id, InvoiceId, ProductId, Quantity, PriceBilled, TaxRate, LineTotal 
    FROM bil.InvoiceDetails WHERE InvoiceId = @Id;
END;
GO

CREATE PROCEDURE bil.sp_Invoice_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, InvoiceNumber, CustomerId, TaxAmount, SubTotalAmount, TotalBilled, PaymentMethod, InvoiceDate 
    FROM bil.Invoices ORDER BY InvoiceDate DESC;
END;
GO

CREATE PROCEDURE bil.sp_Invoice_GetByDate
    @InvoiceDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, InvoiceNumber, CustomerId, TaxAmount, SubTotalAmount, TotalBilled, PaymentMethod, InvoiceDate 
    FROM bil.Invoices 
    WHERE CAST(InvoiceDate AS DATE) = CAST(@InvoiceDate AS DATE)
    ORDER BY InvoiceDate DESC;
END;
GO