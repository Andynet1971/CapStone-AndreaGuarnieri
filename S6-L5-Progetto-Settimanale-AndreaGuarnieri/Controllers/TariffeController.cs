using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Controllers
{
    public class TariffeController : Controller
    {
        private readonly ITariffeService _tariffeService;

        public TariffeController(ITariffeService tariffeService)
        {
            _tariffeService = tariffeService;
        }

        public IActionResult Index()
        {
            var tariffe = _tariffeService.GetAllTariffe();
            return View(tariffe);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tariffa = _tariffeService.GetTariffaById(id);
            return View(tariffa);
        }

        [HttpPost]
        public IActionResult Edit(TariffaViewModel tariffa)
        {
            if (ModelState.IsValid)
            {
                _tariffeService.UpdateTariffa(tariffa);
                return RedirectToAction("Index");
            }
            return View(tariffa);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TariffaViewModel tariffa)
        {
            if (ModelState.IsValid)
            {
                _tariffeService.CreateTariffa(tariffa);
                return RedirectToAction("Index");
            }
            return View(tariffa);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _tariffeService.DeleteTariffa(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SalvaModifiche(List<TariffaViewModel> tariffe)
        {
            if (tariffe == null || tariffe.Count == 0)
            {
                return RedirectToAction("Index");
            }

            foreach (var tariffa in tariffe)
            {
                _tariffeService.UpdateTariffa(tariffa);
            }

            return RedirectToAction("Index");
        }

    }
}
