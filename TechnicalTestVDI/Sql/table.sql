CREATE TABLE DiscountTransactions (
    TransactionId VARCHAR(20) PRIMARY KEY,
    CustomerType VARCHAR(20) NOT NULL,
    PointReward INT NOT NULL,
    TotalPurchase DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) NOT NULL,
    TotalPay DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);