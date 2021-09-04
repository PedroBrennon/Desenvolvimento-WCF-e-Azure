using AtAzure.Mvc.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Net.Http;
using AtAzure.Mvc.Models;
using System.IO;
using Microsoft.Azure;

namespace AtAzure.Mvc.Controllers
{
    public class PaisController : Controller
    {
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;

        private const string _blobContainerName = "pedro-paiva";

        private ApiHelper _clientHelper = new ApiHelper();

        public async Task SetupCloudBlob()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(_blobContainerName);

            await _blobContainer.CreateIfNotExistsAsync();
            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await _blobContainer.SetPermissionsAsync(permissions);
        }

        public string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        // GET: Pais
        public async Task<ActionResult> Index()
        {
            var response = await _clientHelper.GetPais();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();

                return View(model);
            }

            return View(new List<PaisViewModel>());
        }

        // GET: Pais/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await _clientHelper.GetPaisById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PaisViewModel>();

                return View(model);
            }
            return View();
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pais/Create
        [HttpPost]
        public async Task<ActionResult> Create(PaisViewModel pais)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                int fileCount = files.Count;

                if (fileCount == 0)
                {
                    return View();
                }

                await SetupCloudBlob();

                var fileName = GetRandomBlobName(files[0].FileName);
                var blob = _blobContainer.GetBlockBlobReference(fileName);
                await blob.UploadFromStreamAsync(files[0].InputStream);

                pais.Foto = blob.Uri.ToString();
                await _clientHelper.InsertPais(pais);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pais/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _clientHelper.GetPaisById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PaisViewModel>();
                return View(model);
            }
            return View();
        }

        // POST: Pais/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, PaisViewModel pais)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                int fileCount = files.Count;

                if (fileCount == 0)
                {
                    return View();
                }

                await SetupCloudBlob();

                var fileName = GetRandomBlobName(files[0].FileName);
                var blob = _blobContainer.GetBlockBlobReference(fileName);

                await blob.UploadFromStreamAsync(files[0].InputStream);
                pais.Foto = blob.Uri.ToString();
                await _clientHelper.UpdatePais(pais);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pais/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _clientHelper.GetPaisById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<PaisViewModel>();
                return View(model);
            }
            return View();
        }

        // POST: Pais/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, PaisViewModel pais)
        {
            try
            {
                var response = await _clientHelper.DeletePais(id);
                if (response.IsSuccessStatusCode)
                {
                    pais = await response.Content.ReadAsAsync<PaisViewModel>();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
