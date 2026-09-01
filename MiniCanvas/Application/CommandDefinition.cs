namespace App.Application;

public record CommandDefinition(ICollection<char> Arguments, Action<string[]> Handler);