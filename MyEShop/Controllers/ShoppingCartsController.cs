﻿using Microsoft.AspNet.Identity;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using MyEShop.Web.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {

            return View(db.ShoppingCart.ToList());
        }



        public ActionResult GetShoppingCartItems()
        {
            if (User.Identity.IsAuthenticated)
            {
                CartVM cart = new CartVM();
                //List<CartItemVM> cartItemList = new List<CartItemVM>();

                var userId = User.Identity.GetUserId();
                var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();

                foreach (var item in itemsInCart)
                {
                    CartItemVM itm = new CartItemVM();
                    itm.Count = item.Count;
                    itm.Price = item.Price;
                    itm.ProductName = item.ProductName;
                    itm.WebId = item.Id;

                    cart.TotalPrice += item.Count * item.Price;
                    if (cart.CartItems == null)
                    {
                        cart.CartItems = new List<CartItemVM>();
                    }
                    cart.CartItems.Add(itm);
                }
                return View(cart);
            }


            return RedirectToAction("Index", "Home");
        }

        public JsonResult UpdateItems(int itemId, int operation)
        {

            if (User.Identity.IsAuthenticated)
            {
                if (itemId > 0)
                {
                    var userId = User.Identity.GetUserId();
                    var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();
                    var item = itemsInCart.Where(i => i.Id == itemId).SingleOrDefault();

                    var productInStock = db.Products.Where(p => p.Id == itemId).SingleOrDefault();
                    var numberAvailable = productInStock.Count;

                    decimal total = 0;
                    foreach (var x in itemsInCart)
                    {
                        total += x.Count * x.Price;
                    }

                    //add one to the count of the item
                    if (operation == 1)
                    {
                        if (numberAvailable > 0)
                        {
                            item.Count += 1;
                            productInStock.Count -= 1;
                            db.Entry(productInStock).State = EntityState.Modified;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            total += item.Price;

                        }
                        else
                        {
                            return Json(new { Message = "There is not enough item in stock." }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else if (operation == 0)
                    {
                        if (item.Count >= 0)
                        {
                            item.Count -= 1;
                            productInStock.Count += 1;
                            db.Entry(productInStock).State = EntityState.Modified;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            total -= item.Price;
                        }
                    }
                    else
                    {
                        return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet);
                    }

                    //calculate item price and total price
                    var itemFee = (item.Count * item.Price);


                    return Json(new { itemPrice = itemFee, totalPrice = total }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet); ;
        }



        public JsonResult DeleteItem(int itemId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (itemId > 0)
                {
                    var userId = User.Identity.GetUserId();
                    var itemInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false && s.Id == itemId).SingleOrDefault();

                    var count = itemInCart.Count;
                    var price = itemInCart.Price;
                    var itemTotal = count * price;

                    var pId = itemInCart.ProductId;
                    var productInStock = db.Products.Where(p => p.Id == pId).SingleOrDefault();
                    productInStock.Count += count;
                    db.Entry(productInStock).State = EntityState.Modified;
                    db.ShoppingCart.Remove(itemInCart);
                    db.SaveChanges();

                    return Json(new { priceSubtract = itemTotal }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = "Something went wrong, Try again." }, JsonRequestBehavior.AllowGet); ;

        }


        public ActionResult Payment()
        {

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var itemsInCart = db.ShoppingCart.Where(s => s.UserId == userId && s.Status == false).ToList();

                // the gateway does not accepts amount less than 1000 Rials
                decimal totalPrice = 1000;
                foreach (var item in itemsInCart)
                {
                    totalPrice += item.Count * item.Price;
                }

                try
                {
                    Payment ob = new Payment();
                    string result = ob.pay(totalPrice.ToString());
                    JsonParameters parameters = JsonConvert.DeserializeObject<JsonParameters>(result);

                    if (parameters.status == 1)
                    {
                        List<TempSale> LstTempSales = new List<TempSale>();
                        foreach (var x in itemsInCart)
                        {
                            TempSale t = new TempSale()
                            {
                                ShoppingCartId = x.Id,
                                BankGetNo = parameters.transId,
                                Date = DateTime.Now,
                                Status = false,
                                Count = x.Count,
                                Price = x.Price,
                                ProductId = x.ProductId,
                                ProductName = x.ProductName
                            };
                            LstTempSales.Add(t);
                        }
                        db.TempSale.AddRange(LstTempSales);
                        db.SaveChanges();

                        Response.Redirect("https://pay.ir/payment/test/gateway/" + parameters.transId);
                    }
                    else
                    {
                        //lblresult.text = "کدخطا : " + parmeters.errorcode + "<br />" + "پیغام خطا : " + parmeters.errormessage;
                    }
                }
                catch (Exception e)
                {
                    //lblresult.text = "خطا در اتصال به درگاه پرداخت";
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public ActionResult PaymentComplete(string transId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(transId))
                {
                    Payment ob = new Payment();
                    string result = ob.verify(transId.ToString());
                    JsonParameters Parmeters = JsonConvert.DeserializeObject<JsonParameters>(result);

                    if (Parmeters.status == 1)
                    {
                        var tmpSales = db.TempSale.Include("Product").Where(t => t.BankGetNo == transId).ToList();

                        foreach (var i in tmpSales)
                        {
                            i.Status = true;
                            db.TempSale.Attach(i);
                            db.Entry(i).State = EntityState.Modified;
                        }

                        db.SaveChanges();

                        List<Sale> LstSale = new List<Sale>();
                        foreach (var j in tmpSales)
                        {
                            Sale s = new Sale()
                            {
                                TrackingCode = 0,
                                Count = j.Count,
                                Date = DateTime.Now,
                                BankNo = "",
                                Payed = true,
                                Price = j.Price,
                                ProductId = j.ProductId,
                                StatusId = SalesStatus.OnHold,
                                UserId = userId,
                                Product = j.Product,
                                TransId = Convert.ToInt32(transId)
                            };
                            LstSale.Add(s);

                        }

                        // find Id of sellers
                        var ids = new List<string>();
                        foreach (var o in tmpSales)
                        {
                            ids.Add(o.Product.UserId);
                        }

                        var bills = db.Bills.Where(b => ids.Contains(b.UserId)).ToList();
                        var userBillId = bills.Select(b => b.UserId).ToList();


                        foreach (var v in LstSale)
                        {
                            Message messageToSeller = new Message()
                            {
                                Date = DateTime.Now,
                                IsRead = false,
                                Text = "You Sold item " + v.Product.Name + " , Price: " + v.Price,
                                Title = "Item Sold",
                                UserRecId = v.Product.UserId
                            };

                            db.Messages.Add(messageToSeller);

                            if (userBillId.Contains(v.Product.UserId))
                            {
                                var currentBill = db.Bills.Where(b => b.UserId == v.Product.UserId).SingleOrDefault();
                                currentBill.LastRec = Convert.ToInt32(v.Count * v.Price);
                                currentBill.TotalRec = currentBill.TotalRec + Convert.ToInt32(v.Count * v.Price);
                                currentBill.TimeOfLastRec = DateTime.Now;
                                db.SaveChanges();

                            }
                            else
                            {
                                Bill bill = new Bill()
                                {
                                    LastRec = Convert.ToInt32(v.Count * v.Price),
                                    TotalRec = Convert.ToInt32(v.Count * v.Price),
                                    TimeOfLastRec = DateTime.Now,
                                    PayMe = false,
                                    PayMeAmount = 0,
                                    UserId = v.Product.UserId,
                                    User = v.Product.User
                                };

                                db.Bills.Add(bill);
                            }

                        }
                        db.Sales.AddRange(LstSale);
                        db.SaveChanges();

                        List<ShoppingCart> tmpShop = new List<ShoppingCart>();
                        foreach (var n in tmpSales)
                        {
                            db.ShoppingCart.RemoveRange(db.ShoppingCart.Where(sh => sh.Id == n.ShoppingCartId));
                            db.TempSale.Remove(n);
                        }

                        db.SaveChanges();

                        return RedirectToAction("GetSalesItemByTrasnId", new { tId = transId, price = Parmeters.amount.ToString() });
                    }
                    else
                    {
                        return Content("error");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult GetSalesItemByTrasnId(int tId, int price)
        {

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.TransID = tId;
                ViewBag.PaidPrice = price;

                var userId = User.Identity.GetUserId();

                var saleItems = db.Sales.Include("Product").Where(s => s.TransId == tId && s.UserId == userId && s.Payed == true).ToList();

                return View("PaymentComplete", saleItems);

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyPurchaseHistory()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                var items = db.Sales.Include("Product").Where(s => s.UserId == userId && s.Payed == true).OrderByDescending(a => a.Date).ToList();

                CartVM cart = new CartVM();

                cart.Count = items.Count();

                foreach (var item in items)
                {
                    cart.TotalPrice += item.Price * item.Count;
                }

                foreach (var cartitem in items)
                {
                    CartItemVM cartItem = new CartItemVM();
                    cartItem.Count = cartitem.Count;
                    cartItem.Price = cartitem.Price;
                    cartItem.ProductName = cartitem.Product.Name;
                    cartItem.WebId = cartitem.Id;
                    cartItem.Date = cartitem.Date;
                    cartItem.StatusId = cartitem.StatusId;
                    cartItem.TrackingCode = cartitem.TrackingCode;

                    if (cart.CartItems == null)
                    {
                        cart.CartItems = new List<CartItemVM>();
                    }
                    cart.CartItems.Add(cartItem);
                }
                return View(cart);
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,ProductName,UserId,Date,Status,Count,Price")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shoppingCart);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,ProductName,UserId,Date,Status,Count,Price")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingCart);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            db.ShoppingCart.Remove(shoppingCart);
            db.SaveChanges();
            return RedirectToAction("Index");
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
