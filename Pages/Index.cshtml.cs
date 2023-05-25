using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(
            string WeekStarting, int Store, string Brand, int Advert,
            decimal Price, double Age60, double COLLEGE, double INCOME,
            double Hincome150, double LargeHH, double Minorities, double WorkingWoman,
            double SSTRDIST, double SSTRVOL, double CPDIST5, double CPWVOL5
            )
        {
            var requestBody = BuildRequestBody(WeekStarting, Store, Brand, Advert, Price, Age60, COLLEGE, INCOME,
                                        Hincome150, LargeHH, Minorities, WorkingWoman, SSTRDIST, SSTRVOL,
                                        CPDIST5, CPWVOL5);


            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            using (var client = new HttpClient(handler))
            {

                const string apiKey = "5LtULpriHQZGkZjcWJnReWPWTWAnZfK3";
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("A key should be provided to invoke the endpoint");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("http://155cfaf6-45d0-4e33-9b15-240e6afa3804.westeurope.azurecontainer.io/score");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    double value = result.Results[0];

                    return RedirectToPage("SuccessPage", new { result = value });
                }
                else
                {
                    return RedirectToPage("Error");
                }
            }
        }

        private string BuildRequestBody(string weekStarting, int store, string brand, int advert,
                                decimal price, double age60, double college, double income,
                                double hincome150, double largeHH, double minorities, double workingWoman,
                                double sstrdist, double sstrvol, double cpdist5, double cpvol5)
        {
            var weekStartingString = weekStarting.ToString(CultureInfo.InvariantCulture);
            var storeString = store.ToString(CultureInfo.InvariantCulture);
            var brandString = brand.ToString(CultureInfo.InvariantCulture);
            var advertString = advert.ToString(CultureInfo.InvariantCulture);
            var priceString = price.ToString(CultureInfo.InvariantCulture);
            var ageString = age60.ToString(CultureInfo.InvariantCulture);
            var collegeString = college.ToString(CultureInfo.InvariantCulture);
            var incomeString = income.ToString(CultureInfo.InvariantCulture);
            var hincomeString = hincome150.ToString(CultureInfo.InvariantCulture);
            var largeHHString = largeHH.ToString(CultureInfo.InvariantCulture);
            var minoritiesString = minorities.ToString(CultureInfo.InvariantCulture);
            var workingWomanString = workingWoman.ToString(CultureInfo.InvariantCulture);
            var sstrdistString = sstrdist.ToString(CultureInfo.InvariantCulture);
            var sstrvolString = sstrvol.ToString(CultureInfo.InvariantCulture);
            var cpdist5String = cpdist5.ToString(CultureInfo.InvariantCulture);
            var cpvol5String = cpvol5.ToString(CultureInfo.InvariantCulture);

            var requestBody = $@"{{
                ""Inputs"": {{
                    ""data"": [
                        {{
                            ""WeekStarting"": ""{weekStartingString}"",
                            ""Store"": {storeString},
                            ""Brand"": ""{brandString}"",
                            ""Advert"": {advertString},
                            ""Price"": {priceString},
                            ""Age60"": {ageString},
                            ""COLLEGE"": {collegeString},
                            ""INCOME"": {incomeString},
                            ""Hincome150"": {hincomeString},
                            ""Large HH"": {largeHHString},
                            ""Minorities"": {minoritiesString},
                            ""WorkingWoman"": {workingWomanString},
                            ""SSTRDIST"": {sstrdistString},
                            ""SSTRVOL"": {sstrvolString},
                            ""CPDIST5"": {cpdist5String},
                            ""CPWVOL5"": {cpvol5String}
                        }}
                    ]
                }},
                ""GlobalParameters"": 0.0
            }}";

            return requestBody;
        }
    }
}