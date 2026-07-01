USE variedadesjennifer_db;
GO
INSERT INTO cst.Customers (CustomerCode, FirstName, LastName, IdentificationNumber, Email, PhoneNumber, Address, IsActive)
VALUES ('CLI-001', 'Juan', 'Pérez', '001-000000-0000U', 'juan@email.com', '8888-8888', 'Managua, Nicaragua', 1);
GO

INSERT INTO emp.Roles (RoleCode, Name, Description)
VALUES
('ADMIN', 'Administrador', 'Acceso total al sistema y configuración general'),
('MANAGER', 'Gerente', 'Supervisa operaciones, inventario y reportes'),
('SUPERVISOR', 'Supervisor', 'Supervisa ventas y personal operativo'),
('CASHIER', 'Cajero', 'Realiza ventas y emisión de facturas'),
('INVENTORY', 'Encargado de Inventario', 'Gestiona existencias y movimientos de inventario'),
('PURCHASING', 'Comprador', 'Gestiona proveedores y abastecimiento'),
('PHARMACIST', 'Farmacéutico', 'Gestiona medicamentos y validación de recetas'),
('AUDITOR', 'Auditor', 'Consulta reportes y auditorías sin modificar datos');
GO
INSERT INTO emp.Employees
(
    EmployeeCode,
    FirstName,
    LastName,
    IdentificationNumber,
    SocialSecurity,
    Phone,
    Address
)
VALUES
('EMP-0001', 'Juan', 'Pérez', '001-010190-0001A', 'SS-0001', '8888-0001', 'Managua'),
('EMP-0002', 'María', 'López', '001-020292-0002B', 'SS-0002', '8888-0002', 'Managua'),
('EMP-0003', 'Carlos', 'García', '001-030395-0003C', 'SS-0003', '8888-0003', 'Managua'),
('EMP-0004', 'Ana', 'Martínez', '001-040498-0004D', 'SS-0004', '8888-0004', 'Masaya'),
('EMP-0005', 'Pedro', 'Sánchez', '001-050188-0005E', 'SS-0005', '8888-0005', 'León');
GO

INSERT INTO emp.Salary (EmployeeId, BaseSalary, IsActive)
VALUES 
(1, 22000.00, 1),  -- 👤 ID 1 (Juan Pérez - Administrador: C$ 22,000)
(2, 25000.00, 1),  -- 👤 ID 2 (María López - Gerente General: C$ 25,000)
(3, 14500.00, 1),  -- 👤 ID 3 (Carlos García - Cajero Central: C$ 14,500)
(4, 16000.00, 1),  -- 👤 ID 4 (Ana Martínez - Encargada Inventario: C$ 16,000)
(5, 18500.00, 1);  -- 👤 ID 5 (Pedro Sánchez - Farmacéutico Regente: C$ 18,500)

GO
INSERT INTO emp.session_auth
(
    EmployeeId,
    RoleId,
    SystemUsername,
    PasswordHash
)
VALUES
(1, 1, 'admin', '$2a$12$AdminHash'),
(2, 2, 'gerente', '$2a$12$ManagerHash'),
(3, 4, 'cajero1', '$2a$12$CashierHash'),
(4, 5, 'inventario', '$2a$12$InventoryHash'),
(5, 7, 'farmaceutico', '$2a$12$PharmacistHash');
GO
INSERT INTO prd.Categories (CategoryCode, Name, Description)
VALUES
('MED', 'Medicamentos', 'Productos farmacéuticos'),
('DEV', 'Dispositivos Médicos', 'Equipos y dispositivos médicos'),
('COS', 'Cosméticos', 'Productos de cuidado personal'),
('SUP', 'Suplementos', 'Vitaminas y suplementos alimenticios'),
('SRV', 'Servicios', 'Servicios prestados por la farmacia');
GO
INSERT INTO prd.Suppliers
(
    SupplierCode,
    CompanyName,
    TaxIdentification,
    Email,
    PhoneNumber,
    Address
)
VALUES
('SUP-001', 'Distribuidora Farmacéutica Nacional', 'J031000001', 'ventas@dfn.com', '22550001', 'Managua'),
('SUP-002', 'MedSupply Nicaragua', 'J031000002', 'contacto@medsupply.com', '22550002', 'Managua'),
('SUP-003', 'Laboratorios Centroamericanos', 'J031000003', 'info@labca.com', '22550003', 'León');
GO
INSERT INTO prd.Products
(
    ProductCode,
    CategoryId,
    SupplierId,
    Name,
    Description,
    BasePrice,
    IsVirtualService
)
VALUES
('PRD-0001', 1, 1, 'Paracetamol 500mg', 'Caja de 100 tabletas', 2.50, 0),
('PRD-0002', 1, 1, 'Ibuprofeno 400mg', 'Caja de 50 tabletas', 4.25, 0),
('PRD-0003', 2, 2, 'Termómetro Digital', 'Termómetro electrónico', 12.00, 0),
('PRD-0004', 4, 3, 'Vitamina C 1000mg', 'Frasco de 60 cápsulas', 8.50, 0),
('PRD-0005', 5, 1, 'Consulta Farmacéutica', 'Servicio de orientación farmacéutica', 5.00, 1);
GO
INSERT INTO prd.MedicineAttributes
(
    ProductId,
    HealthRegisterNumber,
    ActiveIngredient,
    ExpirationDateRequired,
    RequiresPrescription,
    IsActive
)
VALUES
(1, 'REG-MED-0001', 'Paracetamol', 1, 0, 1),
(2, 'REG-MED-0002', 'Ibuprofeno', 1, 0, 1),
(4, 'REG-MED-0003', 'Ácido Ascórbico', 1, 0, 1);
GO
INSERT INTO prd.DeviceAttributes
(
    ProductId,
    Brand,
    Model,
    SerialNumberOrIMEI,
    WarrantyPeriodMonths
)
VALUES
(3, 'Omron', 'MC-246', NULL, 12);
GO
INSERT INTO inv.ProductStocks
(
    ProductId,
    CurrentStock,
    MinimumRequired
)
VALUES
(1, 300, 50),
(2, 150, 30),
(3, 20, 5),
(4, 80, 20);
GO
INSERT INTO inv.StockMovements
(
    ProductId,
    SupplierId,
    MovementType,
    Quantity,
    UnitCost,
    Concept,
    ReferenceDocument
)
VALUES
(1, 1, 'IN', 300, 1.50, 'Compra inicial', 'FAC-0001'),
(2, 1, 'IN', 150, 2.75, 'Compra inicial', 'FAC-0002'),
(3, 2, 'IN', 20, 8.50, 'Compra inicial', 'FAC-0003'),
(4, 3, 'IN', 80, 5.00, 'Compra inicial', 'FAC-0004');
GO
INSERT INTO cst.Customers
(
    CustomerCode,
    FirstName,
    LastName,
    IdentificationNumber,
    Email,
    PhoneNumber,
    Address
)
VALUES
('CUS-0001', 'Carlos', 'Ramírez', '001-010190-1001A', 'carlos@gmail.com', '88880001', 'Managua'),
('CUS-0002', 'María', 'González', '001-020292-1002B', 'maria@gmail.com', '88880002', 'Masaya'),
('CUS-0003', 'José', 'López', '001-030395-1003C', 'jose@gmail.com', '88880003', 'León');
GO
INSERT INTO bil.Invoices
(
    InvoiceNumber,
    CustomerId,
    EmployeeId,
    TaxAmount,
    SubTotalAmount,
    TotalBilled,
    PaymentMethod
)
VALUES
(
    'INV-000001',
    1,
    3,
    1.13,
    7.50,
    8.63,
    'Cash'
);
GO
INSERT INTO bil.InvoiceDetails
(
    InvoiceId,
    ProductId,
    Quantity,
    PriceBilled,
    TaxRate
)
VALUES
(
    1,
    1,
    3,
    2.50,
    15.00
);
