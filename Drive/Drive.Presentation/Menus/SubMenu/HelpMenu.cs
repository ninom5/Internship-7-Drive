
namespace Drive.Presentation.Menus.SubMenu
{
    public static class HelpMenu
    {
        public static void DisplayHelp()
        {
            Console.WriteLine("\nDostupne komande: \n - stvori mapu/datoteku 'ime mape/datoteku': Stvara novu mapu/datoteku. \n - uđi u mapu 'ime mape': Mijenja se working directory \n" +
                " - uredi datoteku 'ime datoteke': Uredivanje datoteke \n - promjeni naziv mape/datoteke ‘ime mape/datoteke’ u ‘novo ime mape/datoteke’: Mijenja se naziv datoteke/mape \n" +
                " - trenutni_direktorij: Prikaz imena trenutnog direktorija u kojem se nalazite \n - podijeli mapu/datoteku s 'email' \n - prestani dijeliti mapu/datoteku s ‘email’" +
                "\n - povratak: Povratak na user menu i izlazak iz komandnog moda");
        }
    }
}
