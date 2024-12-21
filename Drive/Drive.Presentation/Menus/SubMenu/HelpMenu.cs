
namespace Drive.Presentation.Menus.SubMenu
{
    public static class HelpMenu
    {
        private static void DisplayHelp()
        {
            Console.WriteLine("\nDostupne komande:");
            Console.WriteLine(" - napravi_mapu [ime_mape]: Kreira novu mapu.");
            Console.WriteLine(" - napravi_file [ime_fajla]: Kreira novi fajl.");
            Console.WriteLine(" - help: Prikazuje listu dostupnih komandi.");
            Console.WriteLine(" - exit: Izlaz iz komandnog moda.");
        }
    }
}
