namespace Deliverable_2_StudentBookExchangeMVC.Models
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = "";
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
