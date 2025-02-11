namespace HoroscopoProyect.Models
{
    public class UserData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        // Calcula los días restantes para el cumpleaños
        public int DaysUntilBirthday()
        {
            DateTime today = DateTime.Today;
            DateTime nextBirthday = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
            if (nextBirthday < today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }
            return (nextBirthday - today).Days;
        }

        public string GetZodiacSign()
        {
            int day = BirthDate.Day;
            int month = BirthDate.Month;

            return month switch
            {
                1 => (day <= 19) ? "capricorn" : "aquarius",     
                2 => (day <= 18) ? "aquarius" : "pisces",        
                3 => (day <= 20) ? "pisces" : "aries",           
                4 => (day <= 19) ? "aries" : "taurus",            
                5 => (day <= 20) ? "taurus" : "gemini",           
                6 => (day <= 20) ? "gemini" : "cancer",          
                7 => (day <= 22) ? "cancer" : "leo",              
                8 => (day <= 22) ? "leo" : "virgo",              
                9 => (day <= 22) ? "virgo" : "libra",            
                10 => (day <= 22) ? "libra" : "scorpio",         
                11 => (day <= 21) ? "scorpio" : "sagittarius",   
                12 => (day <= 21) ? "sagittarius" : "capricorn", 
                _ => "unknown"
            };
        }

    }
}
