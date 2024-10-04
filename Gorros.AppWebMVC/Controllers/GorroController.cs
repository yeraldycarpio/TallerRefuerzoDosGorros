using Gorros.DTOs.GorrosDTOS;
using Microsoft.AspNetCore.Mvc;

namespace Gorros.AppWebMVC.Controllers
{
    public class GorroController : Controller
    {
        private readonly HttpClient _httpGorroCRMAPI;

        public GorroController(IHttpClientFactory httpGorroFactory)
        {
            _httpGorroCRMAPI = httpGorroFactory.CreateClient("CRMAPI");
        }

        public async Task<IActionResult> Index(SearchQueryGorrosDTO searchQueryGorroDTO, int CountRow = 0)
        {
            if (searchQueryGorroDTO.SendRowCount == 0)
                searchQueryGorroDTO.SendRowCount = 2;
            if (searchQueryGorroDTO.Take == 0)
                searchQueryGorroDTO.Take = 10;

            var result = new SearchResultGorrosDTO();

            var response = await _httpGorroCRMAPI.PostAsJsonAsync("/gorro/search", searchQueryGorroDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultGorrosDTO>();

            result = result != null ? result : new SearchResultGorrosDTO();

            if (result.CountRow == 0 && searchQueryGorroDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            searchQueryGorroDTO.SendRowCount = 0;
            ViewBag.SearchQuery = searchQueryGorroDTO;

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultGorrosDTO();

            var response = await _httpGorroCRMAPI.GetAsync("/gorro/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();

            return View(result ?? new GetIdResultGorrosDTO());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGorrosDTO createGorrosDTO)
        {
            try
            {
                var response = await _httpGorroCRMAPI.PostAsJsonAsync("/gorro", createGorrosDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar guardar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Método para mostrar el formulario de edición de un cliente
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultGorrosDTO();
            var response = await _httpGorroCRMAPI.GetAsync("/gorro/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();

            return View(new EditGorrosDTO(result ?? new GetIdResultGorrosDTO()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGorrosDTO editGorroDTO)
        {
            try
            {
                var response = await _httpGorroCRMAPI.PutAsJsonAsync("/gorro", editGorroDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar editar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultGorrosDTO();
            var response = await _httpGorroCRMAPI.GetAsync("/gorro/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();

            return View(result ?? new GetIdResultGorrosDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultGorrosDTO getIdResultGorroDTO)
        {
            try
            {
                var response = await _httpGorroCRMAPI.DeleteAsync("/gorro/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultGorroDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultGorroDTO);
            }
        }
    }
}
