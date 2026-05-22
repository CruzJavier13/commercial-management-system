USE variedadesjennifer_db;
GO

-- =========================================================================
-- MÓDULO DE CLIENTES (cst)
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
-- MÓDULO DE PRODUCTOS / CATÁLOGO (prd)
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

-- Guarda el producto base, gestiona subtipos (Medicina/Dispositivo) e inicializa stock
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
    -- Atributos Opcionales (Subtipos)
    @HealthRegisterNumber VARCHAR(50) = NULL,
    @ActiveIngredient VARCHAR(150) = NULL,
    @ExpirationDateRequired BIT = 1,
    @RequiresPrescription BIT = 0,
    @Brand VARCHAR(100) = NULL,
    @Model VARCHAR(100) = NULL,
    @SerialNumberOrIMEI VARCHAR(100) = NULL,
    @WarrantyPeriodMonths INT = 0,
    -- Configuración Inventario
    @MinimumRequired INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @GeneratedProductId INT = @Id;

        -- Insertar o Actualizar Tabla Base
        IF @Id IS NULL OR @Id = 0
        BEGIN
            INSERT INTO prd.Products (ProductCode, CategoryId, SupplierId, Name, Description, BasePrice, IsVirtualService, IsActive)
            VALUES (@ProductCode, @CategoryId, @SupplierId, @Name, @Description, @BasePrice, @IsVirtualService, @IsActive);
            
            SET @GeneratedProductId = SCOPE_IDENTITY();

            -- Inicializar Stock en 0 de forma obligatoria
            INSERT INTO inv.ProductStocks (ProductId, CurrentStock, MinimumRequired)
            VALUES (@GeneratedProductId, 0, @MinimumRequired);
        END
        ELSE
        BEGIN
            UPDATE prd.Products
            SET ProductCode = @ProductCode, CategoryId = @CategoryId, SupplierId = @SupplierId,
                Name = @Name, Description = @Description, BasePrice = @BasePrice,
                IsVirtualService = @IsVirtualService, IsActive = @IsActive
            WHERE Id = @Id;
            
            UPDATE inv.ProductStocks 
            SET MinimumRequired = @MinimumRequired 
            WHERE ProductId = @GeneratedProductId;
        END

        -- Limpieza y mapeo de Atributos Especializados (Patrón Mesa de Atributos)
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
-- MÓDULO DE INVENTARIO (inv)
-- =========================================================================
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

        -- Validar existencia física en la tabla de control de stocks
        IF NOT EXISTS(SELECT 1 FROM inv.ProductStocks WHERE ProductId = @ProductId)
        BEGIN
            RAISERROR('Control de inventario no inicializado para este producto.', 16, 1);
        END

        --  Insertar el movimiento histórico
        INSERT INTO inv.StockMovements (ProductId, SupplierId, MovementType, Quantity, UnitCost, Concept, ReferenceDocument)
        VALUES (@ProductId, @SupplierId, @MovementType, @Quantity, @UnitCost, @Concept, @ReferenceDocument);

        -- Afectar el stock real acumulado
        IF @MovementType = 'IN'
        BEGIN
            UPDATE inv.ProductStocks
            SET CurrentStock = CurrentStock + @Quantity, LastUpdate = SYSUTCDATETIME()
            WHERE ProductId = @ProductId;
        END
        ELSE IF @MovementType = 'OUT'
        BEGIN
            -- Nota: Si CurrentStock - @Quantity < 0, saltará automáticamente el CHECK CK_PositiveStock de la tabla
            UPDATE inv.ProductStocks
            SET CurrentStock = CurrentStock - @Quantity, LastUpdate = SYSUTCDATETIME()
            WHERE ProductId = @ProductId;
        END

        COMMIT TRANSACTION;
        SELECT OBJECT_IDENTITY = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        -- Personalización de mensajes si el stock queda en negativo
        IF ERROR_NUMBER() = 547 AND ERROR_MESSAGE() LIKE '%CK_PositiveStock%'
        BEGIN
            RAISERROR('Transacción Cancelada: No hay stock suficiente en inventario para realizar esta salida.', 16, 1);
        END
        ELSE
        BEGIN
            THROW;
        END
    END CATCH
END;
GO

-- =========================================================================
-- MÓDULO DE VENTAS (sal)
-- =========================================================================
CREATE OR ALTER PROCEDURE sal.usp_Orders_Create
    @OrderNumber VARCHAR(50),
    @CustomerId INT,
    @OrderStatus VARCHAR(20),
    @SubTotal DECIMAL(18,2),
    @Discount DECIMAL(18,2) = 0.00,
    @TotalAmount DECIMAL(18,2),
    @DetailsXml XML -- <Details><Item ProductId="x" Quantity="y" PriceAtSale="z"/></Details>
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OrderId INT;

        -- Registrar Encabezado de la Orden
        INSERT INTO sal.Orders (OrderNumber, CustomerId, OrderStatus, SubTotal, Discount, TotalAmount)
        VALUES (@OrderNumber, @CustomerId, @OrderStatus, @SubTotal, @Discount, @TotalAmount);

        SET @OrderId = SCOPE_IDENTITY();

        -- Desglosar Detalles desde el XML enviado por ADO.NET
        INSERT INTO sal.OrderDetails (OrderId, ProductId, Quantity, PriceAtSale)
        SELECT 
            @OrderId,
            Param.Node.value('@ProductId', 'INT'),
            Param.Node.value('@Quantity', 'INT'),
            Param.Node.value('@PriceAtSale', 'DECIMAL(18,2)')
        FROM @DetailsXml.nodes('/Details/Item') AS Param(Node);

        COMMIT TRANSACTION;
        
        SELECT @OrderId AS OrderId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sal.usp_Orders_UpdateStatus
    @OrderId INT,
    @OrderStatus VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE sal.Orders
    SET OrderStatus = @OrderStatus
    WHERE Id = @OrderId;
END;
GO

-- =========================================================================
-- MÓDULO DE FACTURACIÓN (bil) - AUTOMATIZADO CON INVENTARIO
-- =========================================================================
CREATE OR ALTER PROCEDURE bil.usp_Invoices_Generate
    @InvoiceNumber VARCHAR(50),
    @OrderId INT,
    @CustomerId INT,
    @TaxAmount DECIMAL(18,2),
    @SubTotalAmount DECIMAL(18,2),
    @TotalBilled DECIMAL(18,2),
    @PaymentMethod VARCHAR(20),
    @DetailsXml XML -- <Details><Item ProductId="x" Quantity="y" PriceBilled="z" TaxRate="15.00"/></Details>
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @InvoiceId INT;

        -- Guardar Cabecera de Factura Fiscal
        INSERT INTO bil.Invoices (InvoiceNumber, OrderId, CustomerId, TaxAmount, SubTotalAmount, TotalBilled, PaymentMethod)
        VALUES (@InvoiceNumber, @OrderId, @CustomerId, @TaxAmount, @SubTotalAmount, @TotalBilled, @PaymentMethod);

        SET @InvoiceId = SCOPE_IDENTITY();

        -- Insertar Detalle de Factura
        INSERT INTO bil.InvoiceDetails (InvoiceId, ProductId, Quantity, PriceBilled, TaxRate)
        SELECT 
            @InvoiceId,
            Param.Node.value('@ProductId', 'INT'),
            Param.Node.value('@Quantity', 'INT'),
            Param.Node.value('@PriceBilled', 'DECIMAL(18,2)'),
            ISNULL(Param.Node.value('@TaxRate', 'DECIMAL(5,2)'), 15.00)
        FROM @DetailsXml.nodes('/Details/Item') AS Param(Node);

        -- Marcar la Orden de Venta como Facturada ('Invoiced')
        UPDATE sal.Orders 
        SET OrderStatus = 'Invoiced' 
        WHERE Id = @OrderId;

        -- Descontar Inventario automáticamente usando un cursor interno seguro
        -- Solo aplica a productos físicos (Se excluyen los servicios/recargas virtuales)
        DECLARE @ProdId INT, @Qty INT;
        DECLARE @ConceptText VARCHAR(250) = 'Salida automática por Factura N°: ' + @InvoiceNumber;

        DECLARE cur_InventoryCursor CURSOR LOCAL FAST_FORWARD FOR 
            SELECT det.ProductId, det.Quantity 
            FROM bil.InvoiceDetails det
            INNER JOIN prd.Products prod ON det.ProductId = prod.Id
            WHERE det.InvoiceId = @InvoiceId AND prod.IsVirtualService = 0;

        OPEN cur_InventoryCursor;
        FETCH NEXT FROM cur_InventoryCursor INTO @ProdId, @Qty;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Ejecuta la reducción de stock reutilizando el SP del módulo inv
            EXEC inv.usp_StockMovements_Register 
                @ProductId = @ProdId, 
                @SupplierId = NULL, 
                @MovementType = 'OUT', 
                @Quantity = @Qty, 
                @UnitCost = 0.00, 
                @Concept = @ConceptText, 
                @ReferenceDocument = @InvoiceNumber;

            FETCH NEXT FROM cur_InventoryCursor INTO @ProdId, @Qty;
        END

        CLOSE cur_InventoryCursor;
        DEALLOCATE cur_InventoryCursor;

        COMMIT TRANSACTION;
        
        -- Retorna el número de factura procesado con éxito
        SELECT @InvoiceId AS InvoiceId;
    END TRY
    BEGIN CATCH
   
        IF CURSOR_STATUS('local', 'cur_InventoryCursor') >= 0
        BEGIN
            CLOSE cur_InventoryCursor;
            DEALLOCATE cur_InventoryCursor;
        END

        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO