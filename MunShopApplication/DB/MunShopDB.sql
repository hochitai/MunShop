CREATE DATABASE MunShop
GO

USE MunShop
GO

CREATE TABLE roles (
  [id] int NOT NULL PRIMARY KEY,
  [name] nvarchar(255),
  [created_at] DATETIME2(3) CONSTRAINT DF_Roles_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

CREATE TABLE users (
  [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [username] nvarchar(255),
  [password] nvarchar(255),
  [salt] nvarchar(255),
  [email] nvarchar(255),
  [role_id] int,
  [created_at] DATETIME2(3) CONSTRAINT DF_Users_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

CREATE TABLE orders (
  [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [total] integer,
  [user_id] UNIQUEIDENTIFIER,
  [created_at] DATETIME2(3) CONSTRAINT DF_Orders_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

CREATE TABLE products (
  [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [name] nvarchar(255),
  [price] integer,
  [category_id] UNIQUEIDENTIFIER,
  [created_at] DATETIME2(3) CONSTRAINT DF_Products_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

CREATE TABLE categories (
  [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [name] nvarchar(255),
  [created_at] DATETIME2(3) CONSTRAINT DF_Categories_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

CREATE TABLE orderItems (
  [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [order_id] UNIQUEIDENTIFIER,
  [product_id] UNIQUEIDENTIFIER,
  [price] integer,
  [quantity] integer,
  [created_at] DATETIME2(3) CONSTRAINT DF_OrderItems_Created DEFAULT (SYSDATETIME()),
  [updated_at] DATETIME2(3)
)
GO

ALTER TABLE [users] ADD FOREIGN KEY ([role_id]) REFERENCES [roles] ([id])
GO

ALTER TABLE [orders] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [products] ADD FOREIGN KEY ([category_id]) REFERENCES [categories] ([id])
GO

ALTER TABLE [orderItems] ADD FOREIGN KEY ([order_id]) REFERENCES [orders] ([id])
GO

ALTER TABLE [orderItems] ADD FOREIGN KEY ([product_id]) REFERENCES [products] ([id])
GO

CREATE TRIGGER updateRoles
ON [roles]
AFTER UPDATE 
AS
   UPDATE [roles]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [roles].id = i.id
GO

CREATE TRIGGER updateUsers
ON [users]
AFTER UPDATE 
AS
   UPDATE [users]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [users].id = i.id
GO

CREATE TRIGGER updateOrders
ON [orders] 
AFTER UPDATE 
AS
   UPDATE [orders]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [orders].id = i.id
GO

CREATE TRIGGER updateProducts
ON [products] 
AFTER UPDATE 
AS
   UPDATE [products]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [products].id = i.id
GO

CREATE TRIGGER updateCategories
ON [categories] 
AFTER UPDATE 
AS
   UPDATE [categories]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [categories].id = i.id
GO

CREATE TRIGGER updateOrderItems
ON [orderItems] 
AFTER UPDATE 
AS
   UPDATE [orderItems]
   SET updated_at = SYSDATETIME()
   FROM Inserted i
   WHERE [orderItems].id = i.id
GO

INSERT INTO roles(id, name) VALUES (1, 'admin'), (2, 'user');