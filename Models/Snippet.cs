namespace Snipper_Snippets_API.Models
{
    public class Snippet
    {
        public int Id { get; set; }
        public required string Language { get; set; }
        public required string Code { get; set; }
        public int UserId { get; set; }
    }

    public class CreateSnippetRequest
    {
        public required string Language { get; set; }
        public required string Code { get; set; }
    }
}