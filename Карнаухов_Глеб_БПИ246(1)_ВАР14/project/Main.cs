
using LibraryForProject69;
using System;
using System.Text;


//Карнаухов БПИ-246(1) Вариант-14.
/// <summary>
/// Главный класс содержащий Main().
/// </summary>
public class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        string? inputFilePath = String.Empty;
        string? outputFilePath = String.Empty;
        OutputMenuInConsole menu = new OutputMenuInConsole();
        int index = 0;
        ElementsObject fragments = new();
        LaunchingTheMenu.LaunchingMenu(ref fragments, ref index, ref menu, ref inputFilePath, ref outputFilePath);

    }
}