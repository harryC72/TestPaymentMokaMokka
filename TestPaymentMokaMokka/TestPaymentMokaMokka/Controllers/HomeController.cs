using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using TestPaymentMokaMokka.Models;
using Microsoft.Extensions.Configuration;

namespace TestPaymentMokaMokka.Controllers
{
    public class PaymentController : Controller
    {
        public IConfiguration Configuration { get; set; }


        public PaymentController(IConfiguration config)
        {
            Configuration = config;
        }


        public ActionResult Pay()
        {
            var gateway = GetGetAway();
            string clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;

            return View();
        }


        public BraintreeGateway GetGetAway()
        {
            return new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = Configuration["Braintree:MerchantId"],
                PublicKey = Configuration["Braintree:PublicKey"],
                PrivateKey =Configuration["Braintree:PrivateKey"]
            };
        }


        [HttpPost]
        public ActionResult CreatePurchase()
        {

            var gateway = GetGetAway();


            var request = new TransactionRequest
            {
                Amount = 15,
                PaymentMethodNonce = Request.Query["payment_method_nonce"],
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };


            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return View("Completed");
            }
            else
            {
                return RedirectToAction("Unsuccessful");
            }

        }
    }
}
