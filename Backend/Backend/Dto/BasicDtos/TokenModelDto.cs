namespace Backend.Dto.BasicDtos
{
    public class TokenModelDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}