using Bonitet.DAL;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Admin
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    error_text.Visible = false;

                    if (IsPostBack == true)
                        CreateUserFunc();
                }
                else
                {
                    Response.Redirect("/Admin/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("/Admin/Default.aspx");
            }
        }

        public void CreateUserFunc()
        {
            string Username = "";
            string Email = "";
            string EMBS = "";

            var error = false;
            if (c_username.Text.Length > 0 && c_email.Text.Length > 0 && e_embs.Text.Length > 0)
            {
                Username = c_username.Text;
                Email = c_email.Text;
                EMBS = e_embs.Text;
            }
            else
            {
                error_text.InnerText = "Пополнете ги сите полиња!";
                error = true;
            }
            if (error == false)
            {

                var Password = Guid.NewGuid().ToString().Substring(0, 8);

                if (DALHelper.CheckIfUserEMBSExists(EMBS))
                {
                    error_text.InnerText = "Внесениот ЕМБС веќе постои!";
                    error_text.Visible = true;
                }
                else
                {
                    var res = DALHelper.CreateUser(Username, Password, Email, EMBS);
                    if (res != null)
                    {
                        int defaultPort = Request.IsSecureConnection ? 443 : 80;

                        var url = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host
                            + (Request.Url.Port != defaultPort ? ":" + Request.Url.Port : "");

                        var terms_link = url + "/img/%D0%9E%D0%BF%D1%88%D1%82%D0%B8%20%D1%83%D1%81%D0%BB%D0%BE%D0%B2%D0%B8%20-%20E%D0%B1%D0%BE%D0%BD%D0%B8%D1%82%D0%B5%D1%82%D0%B8%20%D0%BC%D0%BA.pdf";

                        var Message = "<b>Почитувани</b><br/><br/>Ви благодариме за нарачката на производот <b>Ебонитети.мк</b><br/>Ви ги доставуваме Вашите податоци од регистрацијата. <br/><br/>Корисничко име: " + res.Username + "<br/>Лозинка: " + res.Password + "<br/>Датум на нарачка:" + DateTime.Now + "<br/><br/>Ве молиме да извршите промена на вашата лозинка.<br/><br/><span style=\"color:#808080;\">Нарачката е за временски период од една година и автоматски се продолжува за ист период, доколку по писмен пат не се откаже 30 дена пред истекот. Нарачателот изјавува дека е запознаен со Општите услови за употреба на производот и е согласен дека личните податоци или други поврзани податоци, кои што се содржат во јавната база на деловниот регистер, ќе бидат објавени на интернет страницата <a href=\"http://eboniteti.mk/\">Eboniteti.mk</a>. Компанијата Таргет Груп ДОО Скопје, гарантира дека податоците ќе ги собира, обработува и заштитува во согласност со Законот за заштита на лични податоци. Имате право на пристап до своите лични податоци и право да нè контактирате ако имате желба да ги погледнете, промените или ажурирате своите податоци во базата на податоци на нашата компанија. Исто така, дадената согласност важи во случај на промена на назив или седиште на Таргет Груп ДОО Скопје или пренос на податоци на правен наследник на компанијата. Податоците ќе ги употребиме за сите слични продукти на нашата компанија.</span><br/><br/><span style=\"color:#808080;\">Прочитајте ги <a href=\"" + terms_link + "\">Општите услови</a></span><br/><br/>Таргет Груп ДОО Скопје, Телфон: + 389 (02) 2 3117-100, е-пошта: info@targetgroup.mk";
                        var Subject = "Регистрација на Ебонитети.мк";
                        MailHelper.SendMail(Email, Subject, Message, true);

                        Response.Redirect("/Admin/UserDetails.aspx?id=" + res.ID);
                    }
                    else
                    {
                        error_text.InnerText = "Грешка при креирање!";
                        error_text.Visible = true;
                    }
                }
            }
            else
            {
                error_text.Visible = true;
            }

        }
    }
}