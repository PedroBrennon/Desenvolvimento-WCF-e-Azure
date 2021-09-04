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
    public class RelacionamentoController : Controller
    {
        private ApiHelper _clientHelper = new ApiHelper();

        // GET: Relacionamento/Friends/5
        public async Task<ActionResult> Friends(int id)
        {
            var response = await _clientHelper.GetRelacionamentoById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<RelacionamentoViewModel>>();

                return View(model);
            }
            return View();
        }

        // GET: Relacionamento/Create
        public async Task<ActionResult> Create()
        {
            var response = await _clientHelper.GetAmigo();

            if (response.IsSuccessStatusCode)
            {
                var model = new RelacionamentoViewModel();
                var amigos = await response.Content.ReadAsAsync<IEnumerable<AmigoViewModel>>();
                var selectlist = new List<SelectListItem>();
                var selectlist2 = new List<SelectListItem>();

                foreach (var amigo in amigos ?? Enumerable.Empty<AmigoViewModel>())
                {
                    var novo = new SelectListItem
                    {
                        Value = ((int)amigo.Id).ToString(),
                        Text = amigo.Nome,
                        Selected = amigo.Id == model.IdAmigo1
                    };
                    selectlist.Add(novo);
                }
                model.Amigo1 = selectlist;
                foreach (var amigo in amigos ?? Enumerable.Empty<AmigoViewModel>())
                {
                    var novo = new SelectListItem
                    {
                        Value = ((int)amigo.Id).ToString(),
                        Text = amigo.Nome,
                        Selected = amigo.Id == model.IdAmigo2
                    };
                    selectlist2.Add(novo);
                }
                model.Amigo2 = selectlist2;



                return View(model);
            }
            return View();
        }

        // POST: Relacionamento/Create
        [HttpPost]
        public async Task<ActionResult> Create(RelacionamentoViewModel model)
        {
            try
            {
                await _clientHelper.InsertRelacionamento(model);

                return RedirectToAction("Index", "Amigo");
            }
            catch
            {
                return View();
            }
        }

        // GET: Relacionamento/Delete/5
        public ActionResult Delete(int id)
        {

            return View();
        }

        // POST: Relacionamento/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id, RelacionamentoViewModel model)
        {
            try
            {
                await _clientHelper.DeleteRelacionamento(Id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

