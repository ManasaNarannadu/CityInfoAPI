namespace CitysInfo.services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string body)
        {
            Console.WriteLine($"mail from {_mailFrom} to {_mailTo},with {nameof(CloudMailService)}");
            Console.WriteLine($"subject : {subject}");
            Console.WriteLine($"message : {body}");
        }
    }
}
