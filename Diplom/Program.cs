using Diplom;
using System.Collections.Generic;

public class Programm
{
    public List<Double> function = new List<double>();
    public List<List<string>> lim = new List<List<string>>();
    public bool minmax;

    void InputData()
    {
        System.Console.WriteLine("Введите условие задачи");

        System.Console.WriteLine("Введите коэффиценты функции разделенные пробелом");
        string[] input = Console.ReadLine().Split(' ');
        foreach (string i in input)
        {
            function.Add(Convert.ToDouble(i));
        }
        while(true)
        {
            
            Console.WriteLine($"Введите ограничение в формате 1 2 <= 0");
            Console.WriteLine($"или нажмите ENTER");
            string data = Console.ReadLine();
            if (data == "")
            {
                break;
            }
            else
            {
                lim.Add(data.Split(' ').ToList());
            
            }
        }
        Console.WriteLine("Введите min или max , в зависимости от условия задачи.");
        switch (Console.ReadLine())
        {
            case "min":
                minmax = false;
                break;
            case "max": 
                minmax = true; 
                break;
        }
        
    }

    public static void Main(String[] args){
        Programm programm = new Programm();
        programm.InputData();
        BandB bandb = new BandB();
        Gomori gomori = new Gomori();
        gomori.SolveGomori(programm.function, programm.lim, programm.minmax, programm.lim);



    }




}