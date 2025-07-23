namespace CitysInfo.services
{
    public interface IMailService
    {
        void Send(string subject, string body);
    }
}