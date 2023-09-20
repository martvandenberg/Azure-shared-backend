using Azure.AI.Vision.Common;
using Azure;
using Azure.Identity;

namespace backendapi.Services
{
    public class AnalyseImage
    {
        private readonly IConfiguration _configuration;

        public AnalyseImage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        


        public void AnalyseImageWithAi(string urlSasString)
        {
            Console.WriteLine("Starting image analysing tool Ai");
            Console.WriteLine($"{urlSasString}");

            var visionEndpoint = _configuration["ComputerVision:Endpoint"];
            var visionKey = _configuration["ComputerVision:ApiKey"];


            var serviceOptions = new VisionServiceOptions(
                visionEndpoint,
                new DefaultAzureCredential());


            using var imageSource = VisionSource.FromUrl(urlSasString);
            Console.WriteLine("imagesource: ", imageSource);

            //Console.WriteLine("image source: ", imageSource.ToString());

            //var analysisOptions = new ImageAnalysisOptions()
            //{
            //    Features = ImageAnalysisFeature.Caption | ImageAnalysisFeature.Text,

            //    Language = "en",

            //    GenderNeutralCaption = true
            //};

            //using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

            //var result = analyzer.Analyze();

            //if (result.Reason == ImageAnalysisResultReason.Analyzed)
            //{
            //    if (result.Caption != null)
            //    {
            //        Console.WriteLine(" Caption:");
            //        Console.WriteLine($"   \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
            //    }

            //    if (result.Text != null)
            //    {
            //        Console.WriteLine($" Text:");
            //        foreach (var line in result.Text.Lines)
            //        {
            //            string pointsToString = "{" + string.Join(',', line.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
            //            Console.WriteLine($"   Line: '{line.Content}', Bounding polygon {pointsToString}");

            //            foreach (var word in line.Words)
            //            {
            //                pointsToString = "{" + string.Join(',', word.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
            //                Console.WriteLine($"     Word: '{word.Content}', Bounding polygon {pointsToString}, Confidence {word.Confidence:0.0000}");
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
            //    Console.WriteLine(" Analysis failed.");
            //    Console.WriteLine($"   Error reason : {errorDetails.Reason}");
            //    Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
            //    Console.WriteLine($"   Error message: {errorDetails.Message}");
            //}
        }
    }
}
