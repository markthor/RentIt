using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public ActionResult ListComments()
        {
            Comment c = new Comment();
            c.UserName = "TokelOke";
            c.Content = "I AM A VAMPIRE I AM A VAMPIRE";
            Comment c2 = new Comment();
            c2.UserName = "Tokel0ke";
            c2.Content = "I AM A VAMPIRE I AM A VAMPIRE OMPOSEMF POSFM PSODMF PSODMF POSDMF PSODMF PSODMF PSODMF PSODMF ";
            Comment c3 = new Comment();
            c3.UserName = "PETER ";
            c3.Content = "I AM A VAMPIRE I AM A VAMASDASDASDASDPIRE";
            List<Comment> list = new List<Comment>();
            list.Add(c);
            list.Add(c2);
            list.Add(c3);
            return View(list);
        }

        public ActionResult Comment(Comment c)
        {
            return PartialView(c);
        }
    }
}
