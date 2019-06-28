using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Proj.Models;
using Proj.Helper;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace Proj.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Contact")]
        public IActionResult contact(){
            return View();
        }
        [HttpGet("DinningTables")]
        public IActionResult Tables(){
            ViewBag.allTables = dbContext.Items.ToList();
            return View();
        }

        [HttpGet("addToC/{itemId}")]
        public IActionResult addItem(int ItemId){
           Item item = dbContext.Items.FirstOrDefault(i=>i.ItemId==ItemId);
           if(SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart")==null){
               List<Cart> cart = new List<Cart>();
               cart.Add(new Cart {item = item , Quant = 1});
               SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
           }
           else{
               List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
               if(isExist(ItemId)==-1){
                   cart.Add(new Cart {item = item , Quant = 1});
               }
               else{
                   cart[isExist(ItemId)].Quant++;
               }
               SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
           }
           return RedirectToAction("Tables");
        }

        [HttpGet("cart")]
        public IActionResult cart(){
            ViewBag.cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            double sum = 0;
            if(cart!=null){
            for (int i = 0; i < cart.Count; i++)
            {
                sum+=cart[i].Quant*cart[i].item.Price;
            }
            ViewBag.total = sum;}
            return View();
        }
        [HttpPost("update/{IID}")]
        public IActionResult update(int IID, int Quant){
            Item item = dbContext.Items.FirstOrDefault(t=>t.ItemId==IID);
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            foreach (var val in cart.ToList())
            {
                if(val.item.ItemId==IID){
                    if(Quant<=0){
                        cart.Remove(val);
                    }else{
                    val.Quant=Quant;}
                }
            }
            if(cart.Count==0)
                cart=null;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("cart");
        }

        [HttpGet("delete/{IID}")]
        public IActionResult delete(int IID){
            Item item = dbContext.Items.FirstOrDefault(t=>t.ItemId==IID);
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            foreach (var val in cart.ToList())
            {
                if(val.item.ItemId==IID)
                    cart.Remove(val);
            }
            if(cart.Count==0)
                cart=null;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("cart");
        }
        [HttpPost("message")]
        public IActionResult sendEmail(string name, string email, string content){
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress(name, email);
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress("Shop","");
            message.To.Add(to);
            message.Subject = "Email from website";
            BodyBuilder bod = new BodyBuilder();
            bod.TextBody = content;
            message.Body = bod.ToMessageBody();
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com",587,SecureSocketOptions.StartTls);
            client.Authenticate("","");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
            return View("contact");
        }
        private int isExist(int id){
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if(cart[i].item.ItemId==id){
                    return i;
                }
            }
            return -1;
        }
    }
}
