using Gorros.DTOs.GorrosDTOS;
using Microsoft.AspNetCore.Mvc;

namespace Gorros.AppWebMVC.Controllers
{
    public class GorroController : Controller
    {
        private readonly HttpClient _httpGorroAPI;

        public GorroController(IHttpClientFactory httpGorroFactory)
        {
            _httpGorroAPI = httpGorroFactory.CreateClient("GAPI");
        }

        public async Task<IActionResult> Index(SearchQueryGorrosDTO searchQueryGorroDTO, int CountRow = 0)
        {
            // Establecer valores predeterminados
            if (searchQueryGorroDTO.SendRowCount == 0)
                searchQueryGorroDTO.SendRowCount = 2;
            if (searchQueryGorroDTO.Take == 0)
                searchQueryGorroDTO.Take = 10;

            var result = new SearchResultGorrosDTO();

            // POST: /gorro/search
            var response = await _httpGorroAPI.PostAsJsonAsync("/gorro/search", searchQueryGorroDTO);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<SearchResultGorrosDTO>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error en la llamada a la API: {response.StatusCode}, Contenido: {errorContent}");
                ViewBag.Error = "Error al buscar gorros.";
            }

            result ??= new SearchResultGorrosDTO();

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
