using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using TechnicalTestVDI.Data;
using TechnicalTestVDI.Models;

namespace TechnicalTestVDI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public string InputText { get; set; }
        [BindProperty(SupportsGet = true)] public string SplitResult { get; set; }
        [BindProperty] public string FirstWords { get; set; }
        [BindProperty] public string SecondWords { get; set; }
        [BindProperty(SupportsGet = true)] public string AnagramResult { get; set; }
        [BindProperty] public string CustomerType { get; set; }
        [BindProperty] public int PointReward { get; set; }
        [BindProperty] public decimal TotalPurchase { get; set; }
        [BindProperty(SupportsGet = true)] public string DiscountResult { get; set; }

        public void OnGet()
        {
            // Init empty properties if needed for GET requests
        }

        public IActionResult OnPostSplitReverse()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                int len = InputText.Length;
                string result;

                if (len % 2 == 0)
                {
                    int mid = len / 2;
                    string left = new string(InputText.Substring(0, mid).Reverse().ToArray());
                    string right = new string(InputText.Substring(mid).Reverse().ToArray());
                    result = left + right;
                }
                else
                {
                    int mid = len / 2;
                    string leftPart = InputText.Substring(0, mid);
                    char middleChar = InputText[mid];
                    string rightPart = InputText.Substring(mid + 1);

                    string leftReversed = new string(leftPart.Reverse().ToArray());
                    string rightReversed = new string(rightPart.Reverse().ToArray());

                    result = leftReversed + middleChar + rightReversed;
                }

                SplitResult = result;
            }

            return Page();
        }

        public IActionResult OnPostAnagramCheck()
        {
            var result = new StringBuilder();
            var firstWordsList = FirstWords.Split(",").Select(x => x.Trim()).ToList();
            var secondWordsList = SecondWords.Split(",").Select(x => x.Trim()).ToList();

            for (int i = 0; i < firstWordsList.Count; i++)
            {
                var fw = new string(firstWordsList[i].OrderBy(c => c).ToArray());
                var sw = new string(secondWordsList[i].OrderBy(c => c).ToArray());
                result.Append(fw == sw ? "1" : "0");
            }
            AnagramResult = result.ToString();
            return Page();
        }

        public IActionResult OnPostDiscount()
        {
            decimal percent = 0, bonus = 0;
            switch (CustomerType.ToLower())
            {
                case "platinum":
                    percent = 0.5m;
                    bonus = PointReward <= 300 ? 35 :
                            PointReward <= 500 ? 50 : 68;
                    break;
                case "gold":
                    percent = 0.25m;
                    bonus = PointReward <= 300 ? 25 :
                            PointReward <= 500 ? 34 : 52;
                    break;
                case "silver":
                    percent = 0.10m;
                    bonus = PointReward <= 300 ? 12 :
                            PointReward <= 500 ? 27 : 39;
                    break;
            }

            var discount = TotalPurchase * percent + bonus;
            var totalPay = TotalPurchase - discount;

            var today = DateTime.Now.ToString("yyyyMMdd");
            var todayCount = _context.DiscountTransactions.Count(x => x.TransactionId.StartsWith(today));
            var transactionId = $"{today}_{(todayCount + 1).ToString("D5")}";

            var transaction = new DiscountTransaction
            {
                TransactionId = transactionId,
                CustomerType = CustomerType,
                PointReward = PointReward,
                TotalPurchase = TotalPurchase,
                Discount = discount,
                TotalPay = totalPay,
                CreatedAt = DateTime.Now
            };

            _context.DiscountTransactions.Add(transaction);
            _context.SaveChanges();

            DiscountResult = $"Diskon : {discount}<br />Total Bayar : {totalPay}";

            return Page();
        }
    }
}
