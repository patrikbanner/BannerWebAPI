namespace BannerWebAPI.Validation
{
    public interface IValidate
    {
        public string[] Validate(in string html);
    }
}
