using Microsoft.AspNetCore.Mvc;

namespace RNInageApp.Controllers
{
    public class MyMvcAppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Message = "Du glömde nog att lägga till en bild";
                return View("UploadImage");
            }
            // konvertera bilden till en bytearray
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            // Skapa en modelinput och sätta en bildkälla
            var modelInput = new BildKlassModel.ModelInput
            {
                ImageSource = imageBytes
            };
            //Använd denna modellen för att göra en forutsägelse
            var prediction = BildKlassModel.Predict(modelInput);
            //Hämta och visa resultatet
            ViewBag.PredictionLabel = prediction.PredictedLabel;
            ViewBag.Scores = BildKlassModel.GetSortedScoresWithLabels(prediction);

            return View("UploadImage");
        }
    }
}
