using HoroscopoProyect.Models;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HoroscopoProyect.Controllers
{
    public class HoroscopoController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inyección de dependencias para HttpClient
        public HoroscopoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult SelectGender()
        {
            return View(); 
        }

        // Solo carga la vista
        [HttpGet]
        public IActionResult EnterData()
        {
            return View();  
        }

        // Acción para la carga de datos 
        [HttpPost]
        public IActionResult EnterData(UserData userData)
        {
            if (userData == null || string.IsNullOrEmpty(userData.Name) || userData.BirthDate == default)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos.";
                return View(); 
            }

            string sign = userData.GetZodiacSign();  // obtenemos el signo
            TempData["UserData"] = JsonConvert.SerializeObject(userData);  

            return RedirectToAction("ShowHoroscopo", "Horoscopo", new { sign });
        }

        // Acción para mostrar el horóscopo
        [HttpGet]
        public async Task<IActionResult> ShowHoroscopo(string sign)
        {
            // traemos UserData de TempData
            var userDataJson = TempData["UserData"] as string;
            if (string.IsNullOrEmpty(userDataJson))
            {
                TempData["ErrorMessage"] = "No se encontraron datos del usuario.";
                return RedirectToAction("EnterData");
            }

            var userData = JsonConvert.DeserializeObject<UserData>(userDataJson);

            // set fecha y idioma para la API
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            var lang = "es";

            var requestBody = new
            {
                date = date,
                lang = lang,
                sign = sign
            };

            try
            {
                // solicitud a la API 
                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.GetAsync($"https://newastro.vercel.app/{sign}?date={date}&lang={lang}");

                if (response.IsSuccessStatusCode)
                {
                    var horoscopeData = await response.Content.ReadAsStringAsync();
                    var horoscope = JsonConvert.DeserializeObject<HoroscopeResponse>(horoscopeData);

                    StatsController.AddConsulta(sign);

                    var model = new ShowHoroscopo
                    {
                        Sign = sign,
                        Name = userData.Name,
                        Prediction = horoscope.Horoscope,
                        Icon = horoscope.Icon,
                        DaysUntilBirthday = userData.DaysUntilBirthday()
                    };

                    return View(model);  
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al obtener el horóscopo.";
                    return RedirectToAction("SelectGender");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("SelectGender");
            }
        }
    }
}
