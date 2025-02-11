using HoroscopoProyect.Models;
using Microsoft.AspNetCore.Mvc;

public class StatsController : Controller
{
    private static List<string> _consultaHistorial = new List<string>();
    private static Dictionary<string, int> _signosBuscados = new Dictionary<string, int>();

    public IActionResult Index()
    {
        // Contador de signo
        var maxBuscado = _signosBuscados.OrderByDescending(x => x.Value).FirstOrDefault();

        var model = new StatsView
        {
            ConsultaHistorial = _consultaHistorial,
            SignoMasBuscado = maxBuscado.Key,
            CantidadBuscado = maxBuscado.Value
        };

        return View("~/Views/Horoscopo/Stats.cshtml", model);
    }

    public static void AddConsulta(string signo)
    {
        _consultaHistorial.Add(signo);

        if (_signosBuscados.ContainsKey(signo))
        {
            _signosBuscados[signo]++;
        }
        else
        {
            _signosBuscados[signo] = 1;
        }
    }
}
