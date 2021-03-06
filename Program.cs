List<string> ans_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\words.txt").ToList();
List<string> pos_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\possible.txt").ToList();


void not_given_loop(List<string> list, string not_given)
{
    for (int i = 0; i < list.Count; i++)
    {
        foreach (char letter in not_given)
        {
            if (list[i].Contains(letter))
            {
                list.RemoveAt(i);
                i--; // will skip a word if i-- not called
                break;
            }
        }
    }
}


void pos_unsure_loop(List<string> pos_words, string unsure)
{
    for (int i = 0; i < pos_words.Count; i++)
    {
        for (int a = 0; a < 5; a++)
        {
            if (unsure[a].ToString() == "_")
            {
                continue;
            }
            else if (!pos_words[i].Contains(unsure[a]))
            {
                pos_words.RemoveAt(i);
                i--;
                break;
            }
            else if (unsure[a] == pos_words[i][a])
            {
                pos_words.RemoveAt(i);
                i--;
                break;
            }
        }
    }
}


void ans_unsure_loop(List<string> ans_words, string unsure)
{
    for (int i = 0; i < ans_words.Count; i++)
    {
        for (int a = 0; a < 5; a++)
        {
            if (unsure[a].ToString() == "_")
            {
                continue;
            }
            else if (unsure[a] == ans_words[i][a])
            {
                ans_words.RemoveAt(i);
                i--;
                break;
            }
        }
    }
}


void pos_sure_loop(List<string> pos_words, string sure)
{
    for (int i = 0; i < pos_words.Count; i++)
    {
        for (int a = 0; a < 5; a++)
        {
            if (sure[a].ToString() == "_")
            {
                continue;
            }
            else if (pos_words[i][a] != sure[a])
            {
                pos_words.RemoveAt(i);
                i--;
                break;
            }
        }
    }
}


void ans_sure_loop(List<string> ans_words, string sure)
{
    for (int i = 0; i < ans_words.Count; i++)
    {
        for (int a = 0; a < 5; a++)
        {
            if (sure[a].ToString() == "_")
            {
                continue;
            }
            else if (ans_words[i].Contains(sure[a]))
            {
                ans_words.RemoveAt(i);
                i--;
                break;
            }
        }
    }
}


void best_choice(List<string> ans_words, List<string> pos_words)
{
    Dictionary<string, int> d = new();    

    if (ans_words.Count != 0)
    {
        foreach (string word in ans_words)
        {
            int score = 0;
            string prev = "";
            
            foreach (char letter in "eariotn")
            {
                if (word.Contains(letter) && !prev.Contains(letter))
                {
                    score += 1;
                    prev += letter;
                }
            }
            d.Add(word, score);
        }
        if (d.Values.Max() < 3)
        {
            d.Clear();
            foreach (string word in ans_words)
            {
                int score = 0;
                string prev = "";

                foreach (char letter in "eariotnslcudp")
                {
                    if (word.Contains(letter) && !prev.Contains(letter))
                    {
                        score += 1;
                        prev += letter;
                    }
                }
                d.Add(word, score);
            }
        }
        string key = d.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        Console.WriteLine("Best choice: " + key);
    }
    else
    {
        foreach (string word in pos_words)
        {
            Console.Write(word + ", ");
        }
    }
}

while (true)
{
    Console.Write("Sure: ");
    string sure = Console.ReadLine(); // ___r_
    Console.Write("Unsure: ");
    string unsure = Console.ReadLine(); // _a_r_
    Console.Write("Not: ");
    string not_given = Console.ReadLine();

    if (!string.IsNullOrEmpty(not_given))
    {
        not_given_loop(pos_words, not_given);
        not_given_loop(ans_words, not_given);
    }

    if (!string.IsNullOrEmpty(unsure))
    {
        pos_unsure_loop(pos_words, unsure);
        ans_unsure_loop(ans_words, unsure);
    }

    if (!string.IsNullOrEmpty(sure))
    {
        pos_sure_loop(pos_words, sure);
        ans_sure_loop(ans_words, sure);
    }    

    if (pos_words.Count == 1)
    {
        Console.WriteLine("Solution: " + pos_words[0]);
        Console.WriteLine("Restart? (Y/n) ");
        string restart = Console.ReadLine();

        if (restart == "n")
        {
            break;
        }
        else
        {
            pos_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\possible.txt").ToList();
            ans_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\words.txt").ToList();
            Console.Clear();
            continue;
        }
    }
    else if (pos_words.Count == 0)
    {
        Console.WriteLine("Something went wrong");
        string stop = Console.ReadLine();
        break;
    }
    else
    {
        best_choice(ans_words, pos_words);
    }

    Console.Write("Continue? Y/n: ");
    string end = Console.ReadLine();
    if (end == "n")
    {
        Console.WriteLine("Restart? (Y/n) ");
        string restart = Console.ReadLine();

        if (restart == "n")
        {
            break;
        }
        else
        {
            pos_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\possible.txt").ToList();
            ans_words = File.ReadAllLines(@"C:\Users\denis.boone\programming\c#\wordle-solver-CS\words.txt").ToList();
            Console.Clear();
            continue;
        }
    }
}