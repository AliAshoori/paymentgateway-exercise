﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaymentGateway.ApplicationServices.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidatorErrors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidatorErrors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PaymentGateway.ApplicationServices.Resources.ValidatorErrors", typeof(ValidatorErrors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Card information is required.
        /// </summary>
        public static string CardCannotBeNull {
            get {
                return ResourceManager.GetString("CardCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The card has been expired.
        /// </summary>
        public static string ExpiredCard {
            get {
                return ResourceManager.GetString("ExpiredCard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid amount provided - the amount must be greater than zero.
        /// </summary>
        public static string InvalidAmount {
            get {
                return ResourceManager.GetString("InvalidAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid card number - please provide one that comes with 16 digits and is space-separated.
        /// </summary>
        public static string InvalidCardNumber {
            get {
                return ResourceManager.GetString("InvalidCardNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid currency - please provide a valid currency symbol that has three characters.
        /// </summary>
        public static string InvalidCurrency {
            get {
                return ResourceManager.GetString("InvalidCurrency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid CVV - please provide the 3-digit number appeared on the back of your card.
        /// </summary>
        public static string InvalidCvv {
            get {
                return ResourceManager.GetString("InvalidCvv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid merchant id provided - this needs to be an integer greater than zero.
        /// </summary>
        public static string InvalidMerchantId {
            get {
                return ResourceManager.GetString("InvalidMerchantId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value provided for the money - it requires a currency and an amount.
        /// </summary>
        public static string InvalidMoney {
            get {
                return ResourceManager.GetString("InvalidMoney", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid payment id provided - this needs to be an integer greater than zero.
        /// </summary>
        public static string InvalidPaymentId {
            get {
                return ResourceManager.GetString("InvalidPaymentId", resourceCulture);
            }
        }
    }
}
