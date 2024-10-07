using Gorros.DTOs.GorrosDTOS;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Gorros.AppWebMVC.Controllers
{
    public class GorroController : Controller
    {
        private readonly HttpClient _httpGorroAPI;

        public GorroController(IHttpClientFactory httpGorroFactory)
        {
            _httpGorroAPI = httpGorroFactory.CreateClient("GorroApi");
        }

        public async Task<IActionResult> Index(SearchQueryGorrosDTO searchQueryGorrosDTO, int CountRow = 0)
        {
            // Initialize searchQueryGorrosDTO if null
            searchQueryGorrosDTO ??= new SearchQueryGorrosDTO();

            // Set default values for SendRowCount and Take
            if (searchQueryGorrosDTO.SendRowCount == 0)
                searchQueryGorrosDTO.SendRowCount = 2;
            if (searchQueryGorrosDTO.Take == 0)
                searchQueryGorrosDTO.Take = 10;

            var result = new SearchResultGorrosDTO();

            try
            {
                // Perform the API call
                var response = await _httpGorroAPI.PostAsJsonAsync("/gorro/search", searchQueryGorrosDTO);

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadFromJsonAsync<SearchResultGorrosDTO>()
                             ?? new SearchResultGorrosDTO(); // Ensure result is not null
                }
                else
                {
                    ViewBag.Error = "Error retrieving data from the server.";
                }
            }
            catch (JsonException ex)
            {
                ViewBag.Error = $"Error deserializing response: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred: {ex.Message}";
            }

            // Set CountRow if needed
            if (result.CountRow == 0 && searchQueryGorrosDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            // Set ViewBag properties
            ViewBag.CountRow = result.CountRow;
            ViewBag.SearchQuery = searchQueryGorrosDTO;

            return View(result);
        }





        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultGorrosDTO();

            // GET: /gorro/{id}
            var response = await _httpGorroAPI.GetAsync($"/gorro/{id}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();
            }
            else
            {
                ViewBag.Error = "Error al obtener los detalles del gorro.";
            }

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
                // POST: /gorro
                var response = await _httpGorroAPI.PostAsJsonAsync("/gorro", createGorrosDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar guardar el registro.";
                return View(createGorrosDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error: {ex.Message}";
                return View(createGorrosDTO);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultGorrosDTO();

            // GET: /gorro/{id}
            var response = await _httpGorroAPI.GetAsync($"/gorro/{id}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();
            }
            else
            {
                ViewBag.Error = "Error al intentar editar el registro.";
            }

            return View(new EditGorrosDTO(result ?? new GetIdResultGorrosDTO()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGorrosDTO editGorroDTO)
        {
            try
            {
                // PUT: /gorro
                var response = await _httpGorroAPI.PutAsJsonAsync("/gorro", editGorroDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar editar el registro.";
                return View(editGorroDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error: {ex.Message}";
                return View(editGorroDTO);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultGorrosDTO();

            // GET: /gorro/{id}
            var response = await _httpGorroAPI.GetAsync($"/gorro/{id}");

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<GetIdResultGorrosDTO>();
            }
            else
            {
                ViewBag.Error = "Error al intentar eliminar el registro.";
            }

            return View(result ?? new GetIdResultGorrosDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultGorrosDTO getIdResultGorroDTO)
        {
            try
            {
                // DELETE: /gorro/{id}
                var response = await _httpGorroAPI.DeleteAsync($"/gorro/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar eliminar el registro.";
                return View(getIdResultGorroDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error: {ex.Message}";
                return View(getIdResultGorroDTO);
            }
        }
    }
}