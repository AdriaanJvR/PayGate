using PayGate.Web.Net.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayGate.Web.Net.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly PayGateSettings payGateSettings;
        private static readonly HttpClient httpClient;
        #endregion

        #region Constructor
        static HomeController()
        {
            httpClient = new HttpClient();
        }

        public HomeController()
        {
            payGateSettings = new PayGateSettings();
            payGateSettings.PayGateID = ConfigurationManager.AppSettings["PayGateID"];
            payGateSettings.PayGateKey = ConfigurationManager.AppSettings["PayGateKey"];
            payGateSettings.InitiateURL = ConfigurationManager.AppSettings["InitiateURL"];
            payGateSettings.ProcessURL = ConfigurationManager.AppSettings["ProcessURL"];
            payGateSettings.Locale = ConfigurationManager.AppSettings["Locale"];
            payGateSettings.Country = ConfigurationManager.AppSettings["Country"];
            payGateSettings.Currency = ConfigurationManager.AppSettings["Currency"];
            payGateSettings.ReturnURL = ConfigurationManager.AppSettings["ReturnURL"];
            payGateSettings.NotifyURL = ConfigurationManager.AppSettings["NotifyURL"];
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> OnceOff(string email)
        {
            var onceOffRequest = new PayGateRequest(payGateSettings.PayGateKey);

            onceOffRequest.PAYGATE_ID = payGateSettings.PayGateID;
            onceOffRequest.LOCALE = payGateSettings.Locale;
            onceOffRequest.COUNTRY = payGateSettings.Country;
            onceOffRequest.CURRENCY = payGateSettings.Currency;
            onceOffRequest.RETURN_URL = payGateSettings.ReturnURL;
            onceOffRequest.NOTIFY_URL = payGateSettings.NotifyURL;

            // Get config setting for testing purposes.
            if (string.IsNullOrEmpty(email))
            {
                email = ConfigurationManager.AppSettings["ConfirmationEmail"];
            }
            // This should be the client's confirmation email address.
            onceOffRequest.EMAIL = email;

            onceOffRequest.AMOUNT = 30000;
            onceOffRequest.REFERENCE = "PayGate MVC Demo - R 300 Option";
            onceOffRequest.TRANSACTION_DATE = DateTime.Now;

            // TODO: Rethink this...
            var requestString = onceOffRequest.ToString();

            var content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync(payGateSettings.InitiateURL, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var results = ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                // TODO: Add proper error handling.
                return View("Error");
            }

            PayGateResponse payGateResponse = new PayGateResponse(payGateSettings.PayGateKey);
            payGateResponse.MapResponse(results);

            if (payGateResponse.CHECKSUM != results["CHECKSUM"])
            {
                // TODO: Add proper error handling.
                return View("Error");
            }

            // TODO: Reconsider this approach. Should we really add this to the session?
            Session.Add("PAY_REQUEST_ID", payGateResponse.PAY_REQUEST_ID);
            Session.Add("REFERENCE", onceOffRequest.REFERENCE);

            // This view should be used for confirmation from user.
            return View("PayWeb", payGateResponse);
        }

        public ActionResult Return([ModelBinder(typeof(PayGateReturnModelBinder))]PayGateReturn payGateReturnViewModel)
        {
            payGateReturnViewModel.SetPassPhrase(payGateSettings.PayGateKey);

            var calculatedChecksum = payGateReturnViewModel.GetCalculatedChecksum(payGateSettings.PayGateID, Session["PAY_REQUEST_ID"].ToString(), Session["REFERENCE"].ToString());

            if (calculatedChecksum != payGateReturnViewModel.CHECKSUM)
            {
                // TODO: Add proper error handling. Error model?
                return View("Error");
            }

            string message = string.Empty;

            switch (payGateReturnViewModel.TRANSACTION_STATUS)
            {
                case TransactionStatus.NotDone:
                    message = "Transaction not completed. Please try again.";
                    break;
                case TransactionStatus.Approved:
                    return View("Success");
                case TransactionStatus.Declined:
                    message = "Transaction declined. Please try again.";
                    break;
                case TransactionStatus.Cancelled:
                    message = "Transaction has been cancelled. Please try again.";
                    break;
                case TransactionStatus.UserCancelled:
                    message = "Transaction has been cancelled by the user.";
                    break;
            }

            ViewBag.Message = message;
            return View("Failure");

        }

        #region Methods
        private Dictionary<string, string> ToDictionary(string response)
        {
            var result = new Dictionary<string, string>();

            var valuePairs = response.Split('&');
            foreach (string valuePair in valuePairs)
            {
                var values = valuePair.Split('=');
                result.Add(values[0], HttpUtility.UrlDecode(values[1]));
            }

            return result;
        } 
        #endregion
    }
}