namespace ResourceBibleStudy.Core.Util
{
    public static class ApplicationConstants
    { 

        #region -- Application Logs --
        public const string SuccessfullyDeleted = "Log(s) has been successfully deleted.";
        public const string LogRefresh = "Log(s) has been successfully refreshed.";
        public const string ProductDisable = "Product has been successfully disabled";
        public const string SuccessfullySaved = " Settings was successfully updated";
        public const string UnableToSave = "Unable to save settings ";
        public const string NoProductFound = "No Product Found";

        #endregion

        /// <summary>
        /// Regular expression for valid mobile number
        /// </summary>
        public const string MobileNumberRegex = @"^((234)(\d{10}))|([0]?(\d{10}))$";

        public const string LeaderArea = "Leader";

        public const string AdministratorArea = "Administrator";
        public const string MemberArea = "Member"; 

        public enum PaymentProcessors
        {
            Stripe = 0
        }

        public enum DeviceNames
        {
            WebBrowser = 0,
            Mobile = 1,
            Api = 2
        }
        public enum GroupName
        {
            AppSettings = 0,
            PaymentSettings = 1
        }

        public enum ActionType
        {
            SuccessfulLogin = 0,
            SuccessfulLoggedOut,
            Registration,
            UnableToRegistration,
            Checkout,
            ChargeCreated,
            ProductedCreated,
            ChargeUpdate,
            Delete,
            Update,
            View,
            Payment,
            CreateOrder,
            CreateOrderWithPayment,
            UnableToCreateOrderWithPayment,
            AppSettingsCreated
        }
    }


}
