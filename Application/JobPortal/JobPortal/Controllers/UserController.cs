using DatabaseLayer;
using JobPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace JobPortal.Controllers
{
    public class UserController : Controller
    {
        private JobshuntDbEntities Db = new JobshuntDbEntities();
          
        // GET: User
        public ActionResult NewUser()
        {
            return View(new UserMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var checkuser = Db.UserTables.Where(u => u.EmailAddress == userMV.EmailAddress).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("EmailAddress", "Email is already registered");
                    return View(userMV);

                }

                checkuser = Db.UserTables.Where(u => u.UserName == userMV.UserName).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("UserName", "UserName is already registered");
                    return View(userMV);

                }

                using(var trans = Db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new UserTable();
                        user.UserName = userMV.UserName;
                        user.Password = userMV.Password;
                        user.ContactNo = userMV.ContactNo;
                        user.EmailAddress = userMV.EmailAddress;
                        user.UserTypeID = userMV.AreYouProvider == true ? 2 : 3;
                        Db.UserTables.Add(user);
                        Db.SaveChanges();
                        if (userMV.AreYouProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;
                            if (string.IsNullOrEmpty(userMV.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.PhoneNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.PhoneNo", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required*");
                                return View(userMV);
                            }
                            company.EmailAddress = userMV.Company.EmailAddress;
                            company.CompanyName = userMV.Company.CompanyName;
                            company.ContactNo = userMV.ContactNo;
                            company.PhoneNo = userMV.Company.PhoneNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            company.Description = userMV.Company.Description;

                            Db.CompanyTables.Add(company);
                            Db.SaveChanges();
                        }
                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Please Provide Correct Details!");
                        trans.Rollback();
                    }
               }
            }
            return View(userMV);
        }

        public ActionResult Login()
        {
            return View(new UserLoginMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMV userLoginMV)
        {
            if (ModelState.IsValid)
            {
                var user = Db.UserTables.Where(u => u.UserName == userLoginMV.UserName && u.Password == userLoginMV.Password).FirstOrDefault();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "UserName or Password is Incorrect!");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["UserTypeID"] = user.UserTypeID;
                if (user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }

                return RedirectToAction("Index", "Home");

            }
            return View(userLoginMV);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AllUsers()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var users = Db.UserTables.ToList();
            return View(users);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordMV forgotPasswordMV)
        {
            try
            {
                var user = Db.UserTables.Where(u => u.EmailAddress == forgotPasswordMV.Email).SingleOrDefault();

                if (user != null && forgotPasswordMV.Password == forgotPasswordMV.ConfirmPassword)
                {

                    user.EmailAddress = forgotPasswordMV.Email;
                    user.Password = forgotPasswordMV.Password;
                    Db.SaveChanges();

                    return RedirectToAction("Login", "User");
                } 

                else if(user != null && forgotPasswordMV.Password != forgotPasswordMV.ConfirmPassword)
                {
                    TempData["msg"] = "Password do not match";
                }

                else
                {
                    TempData["msg"] = "Account not updated";
                }
 
            }
            catch (Exception ex)
            {

                TempData["msg"] = ex;
            }

            return View();
        }

        

    }
}