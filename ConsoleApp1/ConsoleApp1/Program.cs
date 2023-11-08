using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;

public class Human
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Strana { get; set; }

   
    public Human() { }
}

public class FileManager
{
   

    public List<Human> Humans { get; set; }

    public string GetFilePathFromUser(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    public void LoadFile(string path)
    {
        Humans = new List<Human>();
        if (path.EndsWith(".txt"))
        {
            ReadTextFile(path);
        }
        else if (path.EndsWith(".json"))
        {
            ReadJsonFile(path);
        }
        else if (path.EndsWith(".xml"))
        {
            ReadXmlFile(path);
        }
    }

    public void SaveFile(string path)
    {
        if (path.EndsWith(".txt"))
        {
            var text = GetHumanText();
            File.WriteAllText(path, text);
            Console.WriteLine("Файл успешно сохранен в формате txt.");
        }
        else if (path.EndsWith(".xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Human>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                xml.Serialize(fs, Humans);
            }

            Console.WriteLine("Файл успешно сохранен в формате xml.");
        }
        else if (path.EndsWith(".json"))
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(Humans));
            Console.WriteLine("Файл успешно сохранен в формате json.");
        }
    }

    private void ReadTextFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        
        foreach (var line in lines)
        {
            var parts = line.Split(":");
            var human = new Human()
            {
                Name = parts[0],
                Age = int.Parse(parts[1]),
                Strana = parts[2]
            };
            Humans.Add(human);
        }
    }

    private void ReadJsonFile(string path)
    {
        var text = File.ReadAllText(path);
        Humans = JsonConvert.DeserializeObject<List<Human>>(text);
    }

    private void ReadXmlFile(string path)
    {
        XmlSerializer xml = new XmlSerializer(typeof(List<Human>));
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            Humans = (List<Human>)xml.Deserialize(fs);
        }
    }

    private string GetHumanText()
    {
        string result = "";
        foreach (var human in Humans)
        {
            result = $"Name: {human.Name}, Age: {human.Age}, Country: {human.Strana}\n";
        }
        return result;
    }

    public void DisplayHumans()
    {
        Console.WriteLine("\nСодержимое файла:");
        foreach (var human in Humans)
        {
            Console.WriteLine($"Имя: {human.Name}, Возраст: {human.Age}, Страна: {human.Strana}");
        }
        Console.WriteLine();
    }
}




class Program
{

    static void Main(string[] args)
    {
        FileManager manager = new FileManager();

        string path = manager.GetFilePathFromUser("Введите путь файла, который хотите прочитать:");
        manager.LoadFile(path);
        manager.DisplayHumans();

        while (true) 
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.F1: 
                    var savePath = manager.GetFilePathFromUser("Где и в каком формате вы хотите сохранить файл?");
                    manager.SaveFile(savePath);
                    break;

                case ConsoleKey.Escape: 
                    return; 

                default:
                    Console.WriteLine("Некорректный ввод! Пожалуйста, пробуйте еще раз...");
                    break;
            }
        }
    }

    
}
