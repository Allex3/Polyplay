namespace PolyplayAPI.Models
{
    public class ErrorsJson(string[] errors)
    {
        public List<string> Errors { get; set; } = errors.ToList();
    }
}
