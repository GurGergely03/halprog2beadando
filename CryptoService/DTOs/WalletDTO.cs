namespace CryptoService.DTOs;

public class WalletGetByIdDTO
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public List<WalletTransactionHistoryGetDTO> Transactions { get; set; }
}

public class WalletUpdateDTO
{
    public decimal Balance { get; set; }
}

public class WalletTransactionHistoryGetDTO
{
    public int Id { get; set; }
    public string CryptoName { get; set; }
    public decimal Amount { get; set; }
}