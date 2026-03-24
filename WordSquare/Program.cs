// See https://aka.ms/new-console-template for more information
using WordSquare;

string[] dict = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Dict.txt");

WordTree wordTree = new WordTree(dict);

Menu();

void Menu()
{
    while (true)
    {
        Console.ResetColor();
        Console.Clear();
        Console.WriteLine("Press 'A' to demonstrate Graft.\nPress 'B' to play WordSquare.\nPress 'C' to auto-play WordSquare.\nRemember to turn of capslock.\nESC will escape...");
        ConsoleKeyInfo key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.A: 
                DemonstrateGraft(wordTree);
                break;
            case ConsoleKey.B:
                PlaySquare(wordTree);
                break;
            case ConsoleKey.C:
                AutoplaySquare(wordTree);
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}

void DemonstrateGraft(WordTree wordTree)
{
    Graft graft = wordTree.Prune();
    while (true)
    {
        DisplayGraft(graft);
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Backspace) graft.Back();
        else if (key.Key == ConsoleKey.Escape) break;
        else graft.Advance(key.KeyChar);
    }
}
void DisplayGraft(Graft graft)
{
    Console.Clear();
    SetColors(
        graft.IsValidWord ? ConsoleColor.Green : graft.IsPartial ? ConsoleColor.Yellow : ConsoleColor.DarkRed,
        ConsoleColor.Black
        );
    Console.Write(graft.Word);
}

void PlaySquare(WordTree wordTree)
{
    Square square = new(4, 3, wordTree);

    while (true)
    {
        DisplaySquare(square);
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Backspace) square.Back();
        else if (key.Key == ConsoleKey.Escape) break;
        else square.Advance(key.KeyChar);
    }

}
void DisplaySquare(Square square)
{
    Console.Clear();

    string words = square.Words.PadRight(square.Width * square.Height);

    SetColor(square.IsComplete() ? ConsoleColor.Green : square.IsValid() ? ConsoleColor.Yellow : ConsoleColor.Red);
    Console.Write(' ');
    for (int i = 0; i < square.Width; i++)
    {
        SetColors(
            ConsoleColor.Black,
            square.ColumnComplete(i) ? ConsoleColor.Green : square.ColumnDeveloping(i) ? ConsoleColor.Yellow : ConsoleColor.Red
            );
        Console.Write(i + 1);
    }
    for (int y = 0; y < square.Height; y++)
    {
        SetColors(
            ConsoleColor.Black,
            square.RowComplete(y) ? ConsoleColor.Green : square.RowDeveloping(y) ? ConsoleColor.Yellow : ConsoleColor.Red
            );
        Console.Write("\n" + (y + 1));
        for (int x = 0; x < square.Width; x++)
        {
            SetColors(ConsoleColor.Black, ConsoleColor.White);
            Console.Write(words[square.Width * y + x]);
        }
    }
    Console.WriteLine();
    Console.ResetColor();
}

void SetColor(ConsoleColor color) => SetColors(color, color);
void SetColors(ConsoleColor foregrund, ConsoleColor background)
{
    Console.ForegroundColor = foregrund;
    Console.BackgroundColor = background;
}

void AutoplaySquare(WordTree wordTree)
{
    while (true)
    {
        int? x = null;
        int? y = null;
        while (x is null || y is null)
        {
            Console.Clear();
            Console.WriteLine("Choose Dimensions (1-9): [{1, 1}, {1, 2}, {1, 3}, {2, 1}, {2, 2}, {2, 3}, {3, 1}, {3, 2}, {3, 3}]");
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) break;
            else if (key.Key == ConsoleKey.D1) { x = 1; y = 1; }
            else if (key.Key == ConsoleKey.D2) { x = 1; y = 2; }
            else if (key.Key == ConsoleKey.D3) { x = 1; y = 3; }
            else if (key.Key == ConsoleKey.D4) { x = 2; y = 1; }
            else if (key.Key == ConsoleKey.D5) { x = 2; y = 2; }
            else if (key.Key == ConsoleKey.D6) { x = 2; y = 3; }
            else if (key.Key == ConsoleKey.D7) { x = 3; y = 1; }
            else if (key.Key == ConsoleKey.D8) { x = 3; y = 2; }
            else if (key.Key == ConsoleKey.D9) { x = 3; y = 3; }
        }
        if (x is null || y is null) return;
        Square square = new(x.Value, y.Value, wordTree);

        var chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        Console.Clear();
        Console.WriteLine("Processing...");
        List<string> solutions = new List<string>();
        GetSolutions(square, chars, ref solutions);
        Console.Clear();

        int total = solutions.Count;
        Console.WriteLine($"{total} solutions.");
        if (total > 0) Console.WriteLine("Press any button to display solutions...");
        else Console.WriteLine("Press any button to go back...");
        Console.ReadKey(true);
        for (int i = 0; i < total; i++)
        {
            string solution = solutions[i];
            while (square.Words.Length > 0) square.Back();
            foreach (char c in solution) square.Advance(c);

            DisplaySquare(square);
            Console.WriteLine($"{i + 1}/{total}");
            Console.WriteLine("1-9 advances: [1, 5, 10, 50, 100, 500, 1000, 5000, 10000]");
            Console.WriteLine("Any button to advance by 1...");

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) break;
            else if (key.Key == ConsoleKey.D1) i += 1 - 1;
            else if (key.Key == ConsoleKey.D2) i += 5 - 1;
            else if (key.Key == ConsoleKey.D3) i += 10 - 1;
            else if (key.Key == ConsoleKey.D4) i += 50 - 1;
            else if (key.Key == ConsoleKey.D5) i += 100 - 1;
            else if (key.Key == ConsoleKey.D6) i += 500 - 1;
            else if (key.Key == ConsoleKey.D7) i += 1000 - 1;
            else if (key.Key == ConsoleKey.D8) i += 5000 - 1;
            else if (key.Key == ConsoleKey.D9) i += 10000 - 1;
        }
    }

}

void GetSolutions(Square square, char[] chars, ref List<string> solutions)
{
    if (square.Words.Length < square.Width * square.Height)
    {
        foreach (char c in chars)
        {
            if (square.Advance(c)) GetSolutions(square, chars, ref solutions);
        }
    }
    else
    {
        solutions.Add(square.Words);
    }
    square.Back();
}