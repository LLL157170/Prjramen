using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.ViewModel;
using prjRemenSuperMarket.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace prjRemenSuperMarket.Controllers
{
    public class ProductInfoController : Controller
    {
        public IActionResult List()
        {
            return View(new CProductDetail().Get_List());
        }
        public IActionResult Detail(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member member = JsonSerializer.Deserialize<Member>(json);
                ViewBag.txtId = member.MemberIdPk;
                ViewBag.IsVIP = member.Check_Member_Is_VIP();
            }

            ViewBag.ProductId = id;

            if (id == null) return RedirectToAction("List");

            RamenSupermarketContext db = new RamenSupermarketContext();

            ProductDetail product = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == id);

            if (product == null) return RedirectToAction("List");

            product.Views += 1;
            CProductDetail cProduct = new CProductDetail();
            cProduct.Product = product;
            db.SaveChanges();
            cProduct.salesInfo = db.SalesInfos.FirstOrDefault(p => p.ProductIdFk == product.ProductIdPk);

            cProduct.Set_Product_Info_ByStates();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
                ViewBag.IsLogin = true;

            return View(cProduct);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                return View();
            }
            return RedirectToAction("Login", "EMList");
        }


        public IActionResult PostEvaluation([FromBody] Evaluation evaluation)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                RamenSupermarketContext db = new RamenSupermarketContext();
                Evaluation a = new Evaluation()
                {
                    EvaluationIdPk = evaluation.EvaluationIdPk,
                    MemberIdFk = evaluation.MemberIdFk,
                    ProductIdFk = evaluation.ProductIdFk,
                    Evaluation1 = evaluation.Evaluation1,
                    Content = evaluation.Content,
                    Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                };

                db.Evaluations.Add(a);
                db.SaveChanges();
                return CreatedAtAction("GetEvaluation", new { id = evaluation.EvaluationIdPk }, evaluation);
            }
            return RedirectToAction("Login", "EMList");
        }

        public IActionResult GetEval(int id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var list = db.Evaluations.Where(p => p.ProductIdFk == id);
            return Json(list);
        }

        public IActionResult EditEval(int id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var list = db.Evaluations.Where(p => p.EvaluationIdPk == id);
            return Json(list);
        }

 
        public IActionResult PutEval(int id ,[FromBody] Evaluation evaluation)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var q = db.Evaluations.FirstOrDefault(p => p.EvaluationIdPk == id);
            q.MemberIdFk = evaluation.MemberIdFk;
            q.Evaluation1 = evaluation.Evaluation1;
            q.Content = evaluation.Content;
            q.Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            q.ProductIdFk = evaluation.ProductIdFk;
            db.SaveChanges();
            return NoContent();
        }

        public IActionResult GetMId()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var list = db.Members;
            return Json(list);
        }

        public IActionResult GetDelete(int id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var q = db.Evaluations.FirstOrDefault(p => p.EvaluationIdPk == id);
            db.Evaluations.Remove(q);
            db.SaveChanges();
            return NoContent();
        }
    }
}
