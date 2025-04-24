using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PSchoolApp.Models.DTO;

namespace PSchoolApp.Controllers
{
    public class ParentController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ParentController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<ParentResponseDto> response = new List<ParentResponseDto>();
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpresponseMessage = await client.GetAsync("https://localhost:7236/api/Parent");


                httpresponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpresponseMessage.Content.ReadFromJsonAsync<IEnumerable<ParentResponseDto>>());
            }
            catch (Exception ex)
            {

                // throw;

               
            }
            return View(response);
        }

        [HttpGet]

        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddPaerntViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7236/api/Parent"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };

            try
            {
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<AddPaerntViewModel>();

                if (response != null)
                {
                    // Success - Set TempData for success message
                    TempData["Success"] = "Parent added successfully!";
                    return RedirectToAction("Add", "Parent");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and set error message if necessary
                TempData["Error"] = "Something went wrong! " + ex.Message;
            }

            // If we get here, there was an error or the API call didn't work
            return RedirectToAction("Add", "Parent");
        }

        [HttpGet]
        public async Task<IActionResult>Edit(Guid id)
        {

            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<ParentResponseDto>($"https://localhost:7236/api/Parent/{id.ToString()}");

            if (response != null)
            {

                return View(response);
            }
            return View(null);

        }

      

        [HttpPost]
        public async Task<IActionResult> Edit(ParentResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return View(responseDto); // Return view with validation errors
            }

            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7236/api/Parent/{responseDto.ParentID}"),
                Content = new StringContent(JsonSerializer.Serialize(responseDto), Encoding.UTF8, "application/json"),
            };

            try
            {
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<ParentResponseDto>();

                if (response != null)
                {
                    TempData["Success"] = "Parent updated successfully!";
                    return RedirectToAction("Edit", "Parent", new { id = responseDto.ParentID });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Something went wrong! " + ex.Message;
            }

            return RedirectToAction("Edit", "Parent", new { id = responseDto.ParentID });
        }

        [HttpPost] 

        public async Task<IActionResult> Delete (ParentResponseDto responseDto)
        {

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = await client.DeleteAsync($"https://localhost:7236/api/Parent/{responseDto.ParentID}");

                httpRequestMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

              //  throw;

                return View("Index");
            }
            
        }

    }

   


}

