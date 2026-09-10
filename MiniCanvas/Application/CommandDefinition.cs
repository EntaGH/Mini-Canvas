namespace App.Application;

public record CommandDefinition(IReadOnlyList<char> Arguments, Action<string[]> Handler);