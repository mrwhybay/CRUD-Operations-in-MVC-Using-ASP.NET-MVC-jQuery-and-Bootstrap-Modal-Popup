using Antlr.Runtime.Misc;
using Curd_Task_Kolte.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Curd_Task_Kolte.Controllers
{
    public class ContactController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> LoadContacts()
        {
            var contacts = await db.Contacts.ToListAsync();
            return Json(new { data = contacts }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Views/Shared/Modals/CreateEdit.cshtml", contact);
        }

        // CreateEdit // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdit(Contact contact)
        {
            if (contact != null)
            {
                
                DateTime dobDate = DateTime.ParseExact(contact.DOB, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                string formattedDOB = dobDate.ToString("MM/dd/yyyy");
               


                if (contact.ContactId == 0) // New contact
                {

                    contact.DOB = formattedDOB;
                    db.Contacts.Add(contact);
                    int a =  await db.SaveChangesAsync();
                    if (a > 0)
                    {
                        TempData["success"] = "Contact Created Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Failed"] = "Failed to create , Contact to admin";
                        return RedirectToAction("Index");
                    }
                }
                else // Edit contact
                {
                    if(contact.DOB == null)
                    {
                        var dob = (from c in db.Contacts
                                   where c.ContactId == contact.ContactId
                                   select c.DOB).FirstOrDefault();
                        contact.DOB = dob;
                        contact.DOB = formattedDOB; 
                    }
                    db.Entry(contact).State = EntityState.Modified;
                    int a = await db.SaveChangesAsync();
                    if (a > 0)
                    {
                        TempData["success"] = "Contact Updated successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Failed"] = "Failed to Update , Contact to admin";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(contact);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Views/Shared/Modals/Delete.cshtml", contact);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var contact = await db.Contacts.FindAsync(id);
            if (contact != null)
            {
                db.Contacts.Remove(contact);
                int a = await db.SaveChangesAsync();
                if (a > 0)
                {
                    TempData["success"] = "Successfully Deleted Contact data";
                }
                else
                {
                    TempData["Failed"] = "Failed to Delete , Contact admin";
                }
            }
            else
            {
                TempData["Failed"] = "Contact not found";
            }
            return RedirectToAction("Index");
        }


        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {        
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
