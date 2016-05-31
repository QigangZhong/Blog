using System.Web.Mvc;
using System.Web.Security;
using MVCBlog.Website.Models.InputModels.Login;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MVCBlog.Website.Models.QQ;
using Newtonsoft.Json;
using MVCBlog.Core.Database;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Entities;
using System.Data.Entity;
using System.Linq;

namespace MVCBlog.Website.Controllers
{
    /// <summary>
    /// Controller for user authentication tasks.
    /// </summary>
    public partial class LoginController : Controller
    {
        private readonly IRepository repository;

        public LoginController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Shows the login form.
        /// </summary>
        /// <returns>View showing the login form.</returns>
        public virtual ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.RedirectToAction(MVC.Blog.Index());
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Validates the login and redirects to the return URL.
        /// </summary>
        /// <param name="loginFormInput">The form input.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Redirect to the return URL.</returns>
        [Palmmedia.Common.Net.Mvc.ReferrerAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Index(LoginFormInput loginFormInput, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            if (!FormsAuthentication.Authenticate(loginFormInput.Username, loginFormInput.Password))
            {
                ModelState.AddModelError("login", Properties.Common.LoginFailure);
                return this.View();
            }

            FormsAuthentication.SetAuthCookie(loginFormInput.Username, loginFormInput.RememberMe);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(MVC.Login.Index());
            }
        }

        /// <summary>
        /// Shows the logout form.
        /// </summary>
        /// <returns>View showing the logout form.</returns>
        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction(MVC.Login.Index());
        }

        string callback = System.Web.HttpUtility.UrlEncode("http://www.aspnetfan.com/Login/QQCallBack", Encoding.UTF8);

        public ActionResult RedirectToQQLogin()
        {
            string state = new Random(100000).Next(99, 99999).ToString();
            HttpContext.Session["state"] = state;
            string url_GetAuthorizationCode = string.Format("https://graph.qq.com/oauth2.0/authorize?client_id={0}&response_type=code&redirect_uri={1}&state={2}",
                "101222281", callback, state);
            return Redirect(url_GetAuthorizationCode);
        }

        public async Task<ActionResult> QQCallBack()
        {
            string code = Request["code"];
            string state = Request["state"];

            string url_GetAccessToken = string.Format("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}",
                "101222281", "12f11b73e98aefd14ad1a489b3f0fe09", code, callback);
            string access_token = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync(url_GetAccessToken);
                access_token = result.Split('&')[0].Split('=')[1];
            }

            string url_GetOpenID = string.Format("https://graph.qq.com/oauth2.0/me?access_token={0}",access_token);
            string openID = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync(url_GetOpenID);//callback( {"client_id":"101222281","openid":"22B31763AA6E6098272A062E859993DA"} );
                Regex reg = new Regex("\"openid\":\"(?<openid>.*)\"");
                Match match = reg.Match(result);
                openID = match.Groups["openid"].Value;
            }

            string url_GetUserInfo = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}",
                access_token, "101222281", openID);
            QQUserInfo qqUserInfo = null;
            using (HttpClient client = new HttpClient())
            {
                string userInfoJson = await client.GetStringAsync(url_GetUserInfo);
                qqUserInfo = JsonConvert.DeserializeObject<QQUserInfo>(userInfoJson);
            }

            OAuthUserInfo oAuthUserInfo = await this.repository.OAuthUserInfos.SingleOrDefaultAsync(o => o.OpenId == openID);
            if (oAuthUserInfo == null)
            {
                OAuthUserInfo entity = new OAuthUserInfo() { 
                    LoginType="qq",
                    OpenId=openID,
                    NickName=qqUserInfo.nickname,
                    Avatar=qqUserInfo.figureurl_qq_1,
                    Status=0
                };
                this.repository.OAuthUserInfos.Add(entity);
                await this.repository.SaveChangesAsync();
            }

            FormsAuthentication.SetAuthCookie(string.Format("{0}|{1}", qqUserInfo.nickname, qqUserInfo.figureurl_qq_1), true);

            return RedirectToAction(MVC.Blog.Index());
        }
    }
}
