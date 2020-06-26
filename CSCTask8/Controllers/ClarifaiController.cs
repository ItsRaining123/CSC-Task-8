using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Clarifai.API;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Clarifai.DTOs.Inputs;
using System.IO;
using System.Web;
using Clarifai.DTOs.Predictions;

namespace CSCTask8.Controllers
{
    public class ClarifaiController : ApiController
    {
        [HttpPost]
        [Route("api/clarifai")]
        public async Task<IHttpActionResult> PostImage()
        {

            var image = HttpContext.Current.Request.Files[0];

            //Convert image to byte[]
            byte[] convertedImage = null;
            using (var binaryReader = new BinaryReader(image.InputStream))
            {
                convertedImage = binaryReader.ReadBytes(image.ContentLength);
            }

            try
            {
                var client = new ClarifaiClient("");

                var response = await client.Predict<Concept>(
                    client.PublicModels.GeneralModel.ModelID,
                    new List<IClarifaiInput>
                    {
                        new ClarifaiFileImage(convertedImage)
                        //new ClarifaiURLImage("the-url-2")
                    },
                    "aa7f35c01e0642fda5cf400f543e7c40")
                .ExecuteAsync();

                //Retrieve tag name
                var concepts = response.Get()[0].Data.Select(c => $"{c.Name}");
                var body = string.Join("Taggings", concepts);

                return Ok(concepts);
            } 
            
            catch (Exception e)
            {
                return BadRequest($"Something went wrong: {e.Message}");
            }
        }
    }
}
