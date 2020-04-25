using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        
        [BindProperty]
        public CartViewModel CartVM { get; set; }

        public CartController(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
            
            CartVM = new CartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ServiceList = new List<Service>()
            };
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                foreach(int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_unitOfwork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }

            return View(CartVM);
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);

            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_unitOfwork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }

            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                
                CartVM.ServiceList = new List<Service>();
                
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_unitOfwork.Service.Get(serviceId));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = SD.StatusSubmitted;
                CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;

                _unitOfwork.OrderHeader.Add(CartVM.OrderHeader);
                _unitOfwork.Save();

                foreach(var item in CartVM.ServiceList)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        ServiceId = item.Id,
                        OrderHeaderId = CartVM.OrderHeader.Id,
                        ServiceName = item.Name,
                        Price = item.Price
                    };

                    _unitOfwork.OrderDetails.Add(orderDetails);
                }

                _unitOfwork.Save();

                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction(nameof(OrderConfirmation), "Cart", new { id = CartVM.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}