namespace RentItServer
{
    public partial class SMUuser
    {
        public SMU.User GetUser()
        {
            return new SMU.User(id, email, username, password, isAdmin, SMUrentals);
        }
    }
}