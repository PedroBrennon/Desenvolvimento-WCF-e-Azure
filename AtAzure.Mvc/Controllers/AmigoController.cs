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
    public class AmigoController : Controller
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

        // GET: Amigo
        public async Task<ActionResult> Index()
        {
            var response = await _clientHelper.GetAmigo();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<AmigoViewModel>>();

                return View(model);
            }

            return View(new List<AmigoViewModel>());
        }

        // GET: Amigo/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await _clientHelper.GetAmigoById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<AmigoViewModel>();

                return View(model);
            }
            return View();
        }

        // GET: Amigo/Create
        public async Task<ActionResult> Create()
        {
            var responsePais = await _clientHelper.GetPais();
            var responseEstado = await _clientHelper.GetEstado();

            if (responsePais.IsSuccessStatusCode)
            {
                if (responseEstado.IsSuccessStatusCode)
                {
                    var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();
                    var modelEstado = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoViewModel>>();
                    var model = new AmigoCreateEditViewModel();
                    var selectlistPais = new List<SelectListItem>();
                    var selectlistEstado = new List<SelectListItem>();

                    foreach (var pais in modelPais ?? Enumerable.Empty<PaisViewModel>())
                    {
                        var novo = new SelectListItem
                            {
                                Value = ((int)pais.Id).ToString(),
                                Text = pais.Nome,
                                Selected = pais.Id == model.PaisId
                            };
                        selectlistPais.Add(novo);
                    }
                    model.PaisNome = selectlistPais;

                    /*var estadosPais = new List<EstadoViewModel>();
                    foreach (var e in modelEstado ?? Enumerable.Empty<EstadoViewModel>())
                    {
                        foreach (var p in model.PaisNome ?? Enumerable.Empty<SelectListItem>())
                        {
                            if (p.Selected.Equals(e.paisId)){estadosPais.Add(e);}
                        }    
                    }*/
                    foreach (var estado in modelEstado ?? Enumerable.Empty<EstadoViewModel>())
                    {
                        var novo = new SelectListItem
                            {
                                Value = ((int)estado.Id).ToString(),
                                Text = estado.Nome,
                                Selected = estado.Id == model.EstadoId
                            };
                        selectlistEstado.Add(novo);
                    }
                    model.EstadoNome = selectlistEstado;

                    return View(model);
                }
            }

            return View();
        }

        // POST: Amigo/Create
        [HttpPost]
        public async Task<ActionResult> Create(AmigoCreateEditViewModel amigo)
        {
            var responsePais = await _clientHelper.GetPais();
            var responseEstado = await _clientHelper.GetEstado();
            var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();
            var modelEstado = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoViewModel>>();

            foreach (var m in modelPais)
            {
                if (m.Id == amigo.PaisId)
                {
                    amigo.Pais = m.Nome;
                }
            }
            foreach (var m in modelEstado)
            {
                if (m.Id == amigo.EstadoId)
                { 
                    amigo.Estado = m.Nome;
                }
            }
            

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

                amigo.Foto = blob.Uri.ToString();

                await _clientHelper.InsertAmigo(amigo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Amigo/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _clientHelper.GetAmigoById(id);
            var responsePais = await _clientHelper.GetPais();
            var responseEstado = await _clientHelper.GetEstado();

            if (responsePais.IsSuccessStatusCode)
            {
                if (responsePais.IsSuccessStatusCode)
                {
                    var model = await response.Content.ReadAsAsync<AmigoCreateEditViewModel>();
                    var modelEstado = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoViewModel>>();
                    var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();

                    var selectlistPais = new List<SelectListItem>();
                    var selectlistEstado = new List<SelectListItem>();

                    foreach (var pais in modelPais ?? Enumerable.Empty<PaisViewModel>())
                    {
                        var novo = new SelectListItem
                        {
                            Value = ((int)pais.Id).ToString(),
                            Text = pais.Nome,
                            Selected = pais.Id == model.PaisId
                        };
                        selectlistPais.Add(novo);
                    }
                    model.PaisNome = selectlistPais;

                    /*var estadosPais = new List<EstadoViewModel>();
                    foreach (var e in modelEstado ?? Enumerable.Empty<EstadoViewModel>())
                    {
                        foreach (var p in model.PaisNome ?? Enumerable.Empty<SelectListItem>())
                        {
                            if (p.Selected.Equals(e.paisId)){estadosPais.Add(e);}
                        }    
                    }*/
                    foreach (var estado in modelEstado ?? Enumerable.Empty<EstadoViewModel>())
                    {
                        var novo = new SelectListItem
                        {
                            Value = ((int)estado.Id).ToString(),
                            Text = estado.Nome,
                            Selected = estado.Id == model.EstadoId
                        };
                        selectlistEstado.Add(novo);
                    }
                    model.EstadoNome = selectlistEstado;

                    return View(model);
                }
            }
            return View();
        }

        // POST: Amigo/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, AmigoCreateEditViewModel amigo)
        {
            var responsePais = await _clientHelper.GetPais();
            var responseEstado = await _clientHelper.GetEstado();
            var modelPais = await responsePais.Content.ReadAsAsync<IEnumerable<PaisViewModel>>();
            var modelEstado = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoViewModel>>();

            foreach (var m in modelPais)
            {
                if (m.Id == amigo.PaisId)
                {
                    amigo.Pais = m.Nome;
                }
            }
            foreach (var m in modelEstado)
            {
                if (m.Id == amigo.EstadoId)
                {
                    amigo.Estado = m.Nome;
                }
            }

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
                amigo.Foto = blob.Uri.ToString();

                await _clientHelper.UpdateAmigo(amigo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Amigo/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _clientHelper.GetAmigoById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<AmigoViewModel>();

                return View(model);
            }
            return View();
        }

        // POST: Amigo/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, AmigoViewModel amigo)
        {
            try
            {
                var response = await _clientHelper.DeleteAmigo(id);
                if (response.IsSuccessStatusCode)
                {
                    amigo = await response.Content.ReadAsAsync<AmigoViewModel>();
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
