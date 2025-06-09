namespace WheelDeal_API.Models
{
    public class SignUp
    {
        public int Id {  get; set; }  
        public  string? Name {  get; set; }  
        public  string? Email {  get; set; }  
        public  string? PasswordHash {  get; set; }  
        public  int IsActive {  get; set; }  
        public  int IsDeleted {  get; set; }  
        //public  string? CreatedAt {  get; set; }
        public string? Phone { get; set; }
        
    }
}
// ye model hai