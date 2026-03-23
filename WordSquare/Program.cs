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
        Console.WriteLine("Press 'A' to demonstrate Graft.\nPress 'B' to play WordSquare.\nRemember to turn of capslock.\nESC will escape...");
        ConsoleKeyInfo key = Console.ReadKey();
        switch (key.Key)
        {
            case ConsoleKey.A: 
                DemonstrateGraft(wordTree);
                break;
            case ConsoleKey.B:
                PlaySquare(wordTree);
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

    SetColors(ConsoleColor.Black, ConsoleColor.White);
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
            SetColors(ConsoleColor.White, ConsoleColor.Black);
            Console.Write(words[square.Width * y + x]);
        }
    }
    Console.WriteLine();
}

void SetColors(ConsoleColor foregrund, ConsoleColor background)
{
    Console.ForegroundColor = foregrund;
    Console.BackgroundColor = background;
}