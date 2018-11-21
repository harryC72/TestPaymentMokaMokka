using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using TestPaymentMokaMokka.Models;

namespace TestPaymentMokaMokka.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Pay()
        {
            var gateway = GetGetAway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;

            return View();
        }


        private BraintreeGateway GetGetAway()
        {
            return new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "yjqdcvfx6xcc77gm",
                PublicKey = "yhqkr7q7nxcggvsc",
                PrivateKey = "24d2109885c9087a39a728a56ab59171"
            };
        }


        [HttpPost]
        public ActionResult CreatePurchase()
        {

            var gateway = GetGetAway();


            var request = new TransactionRequest
            {
                Amount = 15M,
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
