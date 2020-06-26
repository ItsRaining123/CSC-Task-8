using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CSCTask8.Controllers
{
    public class GoogleVisionController : ApiController
    {
        [HttpPost]
        [Route("api/googlevision")]
        public IHttpActionResult ImageToText()
        {
            string credential_path = "";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            var image = HttpContext.Current.Request.Files[0];

            //Convert image to byte[]
            byte[] convertedImage = null;
            using (var binaryReader = new BinaryReader(image.InputStream))
            {
                convertedImage = binaryReader.ReadBytes(image.ContentLength);
            }

            ////Convert byte[] to base64
            //string base64Img = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    base64Img = Convert.ToBase64String(convertedImage);
            //}

            try
            {
                var client = ImageAnnotatorClient.Create();

                var i2 = Image.FromBytes(convertedImage);
                var response = client.DetectLabels(i2);


                foreach (var annotation in response)
                {
                    Console.WriteLine(annotation.Description);
                }

                return Ok(response);
            }

            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}
