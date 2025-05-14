-- Seed 15 Cryptocurrencies
INSERT INTO [dbo].[Cryptocurrencies] ([Name], [ShortName], [StartingPrice], [CurrentPrice], [TotalAmount], [AvailableAmount], [CreatedAt])
VALUES 
('Bitcoin', 'BTC', 20000, 30000, 21000000, 20000000, GETDATE()),
('Ethereum', 'ETH', 1000, 1800, 120000000, 115000000, GETDATE()),
('Cardano', 'ADA', 0.1, 0.35, 45000000000, 42000000000, GETDATE()),
('Polkadot', 'DOT', 3, 5, 1000000000, 900000000, GETDATE()),
('Solana', 'SOL', 1, 20, 500000000, 450000000, GETDATE()),
('Litecoin', 'LTC', 40, 90, 84000000, 80000000, GETDATE()),
('Chainlink', 'LINK', 2, 7, 1000000000, 950000000, GETDATE()),
('Stellar', 'XLM', 0.05, 0.1, 50000000000, 48000000000, GETDATE()),
('Ripple', 'XRP', 0.2, 0.5, 100000000000, 95000000000, GETDATE()),
('Dogecoin', 'DOGE', 0.01, 0.07, 130000000000, 120000000000, GETDATE()),
('Shiba Inu', 'SHIB', 0.00001, 0.00002, 1000000000000, 950000000000, GETDATE()),
('Avalanche', 'AVAX', 3, 10, 720000000, 700000000, GETDATE()),
('Tezos', 'XTZ', 1, 2, 900000000, 850000000, GETDATE()),
('VeChain', 'VET', 0.01, 0.03, 86712634466, 86000000000, GETDATE()),
('Uniswap', 'UNI', 2, 6, 1000000000, 900000000, GETDATE());

-- Add Wallets for users
INSERT INTO [dbo].[Wallets] ([Balance], [UserId])
VALUES
(100000, 1),
(50000, 2);

-- Add 2 sample users
INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [WalletId], [CreatedAt])
VALUES 
('Alice Smith', 'alice@example.com', 'hashed_pw_1', 1, GETDATE()),
('Bob Johnson', 'bob@example.com', 'hashed_pw_2', 2, GETDATE());

-- Assign some cryptocurrency holdings to wallets
INSERT INTO [dbo].[WalletCryptocurrencies] ([WalletId], [CryptocurrencyId], [Amount])
VALUES
(1, 1, 0.5),   -- Alice owns 0.5 BTC
(1, 2, 10),    -- Alice owns 10 ETH
(2, 3, 1000),  -- Bob owns 1000 ADA
(2, 5, 50);    -- Bob owns 50 SOL

-- Add some transaction history
INSERT INTO [dbo].[TransactionHistories] ([WalletId], [CryptocurrencyId], [CryptocurrencyPriceAtPurchase], [CryptocurrencyAmount], [TransactionTime])
VALUES
(1, 1, 29000, 0.5, GETDATE()),
(1, 2, 1700, 10, GETDATE()),
(2, 3, 0.3, 1000, GETDATE()),
(2, 5, 18, 50, GETDATE());

-- Add price history entries
INSERT INTO [dbo].[CryptocurrencyHistories] ([PriceAt], [PriceChange], [PriceChangePercent], [CryptocurrencyId], [ChangeAt])
VALUES
(29500, 500, 1.72, 1, DATEADD(DAY, -1, GETDATE())),
(1800, 100, 5.88, 2, DATEADD(DAY, -1, GETDATE())),
(0.35, 0.05, 16.67, 3, DATEADD(DAY, -1, GETDATE())),
(20, -2, -9.09, 5, DATEADD(DAY, -1, GETDATE()));
