using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PSchoolApp.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PSchoolApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public StudentController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? ParentID)
        {
            var client = httpClientFactory.CreateClient();

            // Fetch all students
            var students = await client.GetFromJsonAsync<List<StudentResponseDtocs>>("https://localhost:7236/api/Student");

            // Fetch all parents for the dropdown
            var parents = await client.GetFromJsonAsync<List<ParentResponseDto>>("https://localhost:7236/api/Parent");

            if (parents == null)
                parents = new List<ParentResponseDto>();

            ViewBag.Parents = new SelectList(parents, "ParentID", "F_Name");

            // Filter students if ParentID is provided
            if (ParentID.HasValue)
            {
                students = students.Where(s => s.ParentID == ParentID.Value).ToList();
            }

            return View(students);
        }



        [HttpGet]

        public async Task<IActionResult>Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();


            var response = await client.GetFromJsonAsync<StudentResponseDtocs>($"https://localhost:7236/api/Student/{id.ToString()}");

            if (response != null)
            {

                return View(response);
            }
            return View(null);


        }

        [HttpPost]
        public async Task<IActionResult>Add(AddStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7236/api/Student"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };

            try
            {
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<AddStudentViewModel>();

                if (response != null)
                {
                    // Success - Set TempData for success message
                    TempData["Success"] = "Student added successfully!";
                    return RedirectToAction("Add","Student");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and set error message if necessary
                TempData["Error"] = "Something went wrong! " + ex.Message;
            }

            // If we get here, there was an error or the API call didn't work
            return RedirectToAction("Add", "Student");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentResponseDtocs responseDto)
        {
            if (!ModelState.IsValid)
            {
                return View(responseDto); // Return view with validation errors
            }

            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7236/api/Student/{responseDto.StudentID}"),
                Content = new StringContent(JsonSerializer.Serialize(responseDto), Encoding.UTF8, "application/json"),
            };

            try
            {
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<StudentResponseDtocs>();

                if (response != null)
                {
                    TempData["Success"] = "Student updated successfully!";
                    return RedirectToAction("Edit", "Student", new { id = responseDto.StudentID });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Something went wrong! " + ex.Message;
            }

            return RedirectToAction("Edit", "Student", new { id = responseDto.StudentID });
        }
        [HttpPost]

        public async Task<IActionResult> Delete(StudentResponseDtocs responseDto)
        {

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = await client.DeleteAsync($"https://localhost:7236/api/Student/{responseDto.StudentID}");

                httpRequestMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index","Student");
            }
            catch (Exception)
            {

                //  throw;

                return View("Index");
            }

        }


    }
}
