using AtAzure.Mvc.Helper;
using AtAzure.Mvc.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AtAzure.Mvc.Controllers
{
    public class EstadoController : Controller
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

        // GET: Estado
        public async Task<ActionResult> Index()
        {
            var response = await _clientHelper.GetEstado();
            var responsePais = await _clientHelper.GetPais();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<EstadoViewModel>>();
                var paises = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();

                foreach (var m in model)
                {
                    foreach (var p in paises)
                    {
                        if (m.paisId == p.Id)
                        {
                            m.paisNome = p.Nome;
                        }
                    }
                }

                return View(model);
            }

            return View(new List<PaisViewModel>());
        }

        // GET: Estado/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await _clientHelper.GetEstado();
            var responsePais = await _clientHelper.GetPais();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<EstadoViewModel>();
                var paises = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();

                foreach (var p in paises)
                {
                    if (model.paisId == p.Id)
                    {
                        model.paisNome = p.Nome;
                    }
                }

                return View(model);
            }
            return View();
        }

        // GET: Estado/Create
        public async Task<ActionResult> Create()
        {
            var responsePais = await _clientHelper.GetPais();

            if (responsePais.IsSuccessStatusCode)
            {
                var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();
                var model = new EstadoCreateEditViewModel();
                var selectList = new List<SelectListItem>();

                foreach (var pais in modelPais)
                {
                    var novo = new SelectListItem
                    {
                        Value = ((int)pais.Id).ToString(),
                        Text = pais.Nome,
                        Selected = pais.Id == model.paisId
                    };
                    selectList.Add(novo);
                }
                model.paisNome = selectList;

                return View(model);
            }

            return View();
        }

        // POST: Estado/Create
        [HttpPost]
        public async Task<ActionResult> Create(EstadoCreateEditViewModel estado)
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

                estado.Foto = blob.Uri.ToString();

                await _clientHelper.InsertEstado(estado);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Estado/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _clientHelper.GetEstadoById(id);
            var responsePais = await _clientHelper.GetPais();

            if (response.IsSuccessStatusCode && responsePais.IsSuccessStatusCode)
            {
                var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();
                var model = await response.Content.ReadAsAsync<EstadoViewModel>();

                var estado = new EstadoCreateEditViewModel
                {
                    Id = model.Id,
                    Foto = model.Foto,
                    Nome = model.Nome,
                    paisId = model.paisId
                };

                var selectList = new List<SelectListItem>();

                foreach (var pais in modelPais)
                {
                    var novo = new SelectListItem
                    {
                        Value = ((int)pais.Id).ToString(),
                        Text = pais.Nome,
                        Selected = pais.Id == model.paisId
                    };
                    selectList.Add(novo);
                }
                estado.paisNome = selectList;

                return View(estado);
            }

            return View();
        }

        // POST: Estado/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, EstadoCreateEditViewModel estado)
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
                estado.Foto = blob.Uri.ToString();
                await _clientHelper.UpdateEstado(estado);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Estado/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _clientHelper.GetEstadoById(id);
            var responsePais = await _clientHelper.GetPais();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<EstadoViewModel>();
                var modelPais = await response.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();

                var oi = model.paisNome;

                return View(model);
            }
            return View();
        }

        // POST: Estado/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, EstadoViewModel estado)
        {
            try
            {
                var response = await _clientHelper.DeleteEstado(id);
                if (response.IsSuccessStatusCode)
                {
                    estado = await response.Content.ReadAsAsync<EstadoViewModel>();
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
