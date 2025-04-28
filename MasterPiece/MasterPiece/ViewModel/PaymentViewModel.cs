namespace MasterPiece.ViewModel
{
    public class PaymentViewModel
    {
        //public int AppointmentId { get; set; }
        //public int OwnerId { get; set; }
        //public decimal Amount { get; set; }
        //public string PaymentMethod { get; set; } = "";
        public int AppointmentId { get; set; }
        public int OwnerId { get; set; }
        public decimal Amount { get; set; }

        // لعرض معلومات الموعد
        public string? AppointmentDate { get; set; }
        public string? ClinicName { get; set; }

        // بيانات البطاقة
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

        public string PaymentMethod { get; set; } = "CreditCard";
    }
}
