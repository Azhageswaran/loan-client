namespace Loan_Finance_Client_final.Models.ForeignKeyMap
{
    public class LoanWithUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int LoanID { get; set; }

        public int OnRoadPrice { get; set; }

        public int LoanAmount { get; set; }

        public int MonthOfEmi { get; set; }

        public string PanFileName { get; set; }

        public string AadharFileName { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
    }
}
