using Snipper_Snippets_API.Models;

namespace Snipper_Snippets_API.Services
{
    public class SnippetService
    {
        private readonly List<Snippet> _snippets = new()
        {
            new Snippet { Id = 1, Language = "Python", Code = "print('Hello, World!')" },
            new Snippet { Id = 2, Language = "Python", Code = "def add(a, b):\n    return a + b" },
            new Snippet { Id = 3, Language = "Python", Code = "class Circle:\n    def __init__(self, radius):\n        self.radius = radius\n\n    def area(self):\n        return 3.14 * self.radius ** 2" },
            new Snippet { Id = 4, Language = "JavaScript", Code = "console.log('Hello, World!');" },
            new Snippet { Id = 5, Language = "JavaScript", Code = "function multiply(a, b) {\n    return a * b;\n}" },
            new Snippet { Id = 6, Language = "JavaScript", Code = "const square = num => num * num;" },
            new Snippet { Id = 7, Language = "Java", Code = "public class HelloWorld {\n    public static void main(String[] args) {\n        System.out.println(\"Hello, World!\");\n    }\n}" },
            new Snippet { Id = 8, Language = "Java", Code = "public class Rectangle {\n    private int width;\n    private int height;\n\n    public Rectangle(int width, int height) {\n        this.width = width;\n        this.height = height;\n    }\n\n    public int getArea() {\n        return width * height;\n    }\n}" }
        };
        private int _nextId = 9;

        public Snippet AddSnippet(Snippet snippet)
        {
            snippet.Id = _nextId++;
            _snippets.Add(snippet);
            return snippet;
        }

        public IEnumerable<Snippet> GetAll(string? lang = null)
        {
            if (!string.IsNullOrEmpty(lang))
                return _snippets.Where(s => s.Language.Equals(lang, StringComparison.OrdinalIgnoreCase));
            return _snippets;
        }

        public Snippet? GetById(int id)
        {
            return _snippets.FirstOrDefault(s => s.Id == id);
        }
    }
}