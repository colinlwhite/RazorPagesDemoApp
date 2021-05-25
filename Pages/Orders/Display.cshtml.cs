using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemoApp.Pages.Models;

namespace RazorPagesDemoApp.Pages.Orders
{
    public class DisplayModel : PageModel
    {
        private readonly IOrderData _orderData;
        private readonly IFoodData _foodData;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderUpdateModel UpdateModel { get; set; }

        public OrderModel Order { get; set; }

        public string ItemsPurchased { get; set; }
        public DisplayModel(IOrderData orderData, IFoodData foodData)
        {
            _orderData = orderData;
            _foodData = foodData;
        }
        public async Task<IActionResult> OnGet()
        {
            Order = await _orderData.GetOrderById(Id);

            if (Order != null)
            {
                var food = await _foodData.GetFood();

                ItemsPurchased = food.Where(x => x.Id == Order.FoodId).FirstOrDefault()?.Title;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            await _orderData.UpdateOrder(UpdateModel.Id, UpdateModel.OrderName);

            return RedirectToPage("./Display", new { UpdateModel.Id });
        }
    }
}
