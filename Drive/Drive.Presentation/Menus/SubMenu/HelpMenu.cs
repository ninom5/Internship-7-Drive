
namespace Drive.Presentation.Menus.SubMenu
{
    public static class HelpMenu
    {
        public static void DisplayHelp()
        {
            Console.WriteLine("\nDostupne komande: \n - stvori mapu/datoteku 'ime mape/datoteku': Stvara novu mapu/datoteku. \n - uđi u mapu 'ime mape': Mijenja se working directory \n" +
                " - uredi datoteku 'ime datoteke': Uredivanje datoteke \n - promjeni naziv mape/datoteke ‘ime mape/datoteke’ u ‘novo ime mape/datoteke’: Mijenja se naziv datoteke/mape \n" +
                " - trenutni_direktorij: Prikaz imena trenutnog direktorija u kojem se nalazite \n - podijeli mapu/datoteku s 'email' \n - prestani dijeliti mapu/datoteku s ‘email’" +
                "\n - ls: ispis svih vasih foldera i fileova \n - povratak: Povratak na user menu i izlazak iz komandnog moda");
        }
        public static void DisplaySharedFilesHelp()
        {
            Console.WriteLine("Dostupne komande: \n - izbrisi mapu/datoteku 'ime mape/datoteke' : brise se samo kod vas \n " +
                "- uredi datoteku 'ime datoteke' : promjene unutar datoteke se dogadaju kod vas i kod vlasnika te datoteke" +
                "\n - udi u datoteku 'ime datoteke' : prikazuje se sadrzaj dokumenta i komentari, te se otvara komandni mod za upravljanje komentarima" +
                "\n - povratak: povratak na user menu ");
        }
        public static void DisplayCommentHelp()
        {
            Console.WriteLine("Dostupne komande: \n - dodaj komentar \n - izbrisi komentar \n - uredi komentar \n - povratak");
        }
    }
}
