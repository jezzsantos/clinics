﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicsDomain.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClinicsDomain.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The location details for the clinic have not yet been defined.
        /// </summary>
        internal static string ClinicEntity_NotLocated {
            get {
                return ResourceManager.GetString("ClinicEntity_NotLocated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ownership of the clinic is not yet been defined.
        /// </summary>
        internal static string ClinicEntity_NotOwned {
            get {
                return ResourceManager.GetString("ClinicEntity_NotOwned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The registration for the clinic is not yet been defined.
        /// </summary>
        internal static string ClinicEntity_NotRegistered {
            get {
                return ResourceManager.GetString("ClinicEntity_NotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The jurisdiction is not known.
        /// </summary>
        internal static string ClinicLicense_UnknownJurisdiction {
            get {
                return ResourceManager.GetString("ClinicLicense_UnknownJurisdiction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Country of location must be between {0} and {1}.
        /// </summary>
        internal static string Location_InvalidCountry {
            get {
                return ResourceManager.GetString("Location_InvalidCountry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The City of the location is unknown.
        /// </summary>
        internal static string Location_UnknownCity {
            get {
                return ResourceManager.GetString("Location_UnknownCity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Street of the location is unknown.
        /// </summary>
        internal static string Location_UnknownStreet {
            get {
                return ResourceManager.GetString("Location_UnknownStreet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The slot overlaps and existing slot with a different cause or reference.
        /// </summary>
        internal static string Unavailability_OverlappingSlot {
            get {
                return ResourceManager.GetString("Unavailability_OverlappingSlot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The slot is reserved without a reservation reference.
        /// </summary>
        internal static string Unavailability_ReservationWithoutReference {
            get {
                return ResourceManager.GetString("Unavailability_ReservationWithoutReference", resourceCulture);
            }
        }
    }
}
