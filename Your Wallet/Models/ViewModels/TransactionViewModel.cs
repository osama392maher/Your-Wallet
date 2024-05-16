namespace Your_Wallet.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string? Note { get; set; }

        public int CategoryId { get; set; }


        public static explicit operator Transaction(TransactionViewModel viewModel)
        {
            return new Transaction
            {
                Id = viewModel.Id,
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Note = viewModel.Note,
                CategoryId = viewModel.CategoryId
            };
        }
    }
}
