using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceBibleStudy.Controllers
{
    public class DiscussionForumController : BaseController
    {
        //
        // GET: /DiscussionForum/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AutoLoginUser()
        {
            var user = GetCurrentUser();
            return Json(new
            {
                user.UserName,
                user.UserImageUrl,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

	}
}