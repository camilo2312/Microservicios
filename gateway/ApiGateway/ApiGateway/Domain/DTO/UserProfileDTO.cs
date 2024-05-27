namespace ApiGateway.Domain.DTO
{
    public class UserProfileDTO
    {
        public string UserId { get; set; }
        public string PersonalPageUrl { get; set; }
        public string Nickname { get; set; }
        public bool IsContactInfoPublic { get; set; }
        public string MailingAddress { get; set; }
        public string Biography { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string SocialLinks { get; set; }
    }
}
