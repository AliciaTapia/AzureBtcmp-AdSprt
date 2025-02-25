
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 0.12.1.0
//   Input filename:  Monitoring\EntityAccess.bond
//   Output filename: EntityAccess_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------


// suppress "Missing XML comment for publicly visible type or member"
#pragma warning disable 1591


#region ReSharper warnings
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameter.Local
// ReSharper disable RedundantUsingDirective
#endregion

namespace ContosoAdsSupport
{
    using System.Collections.Generic;

    [System.CodeDom.Compiler.GeneratedCode("gbc", "0.12.1.0")]
    public enum AccessType
    {
        create,
        read,
        update,
        delete,
    }

    [global::Bond.Schema]
    [System.CodeDom.Compiler.GeneratedCode("gbc", "0.12.1.0")]
    public partial class EntityAccess
        : global::Ifx.OperationSchema
    {
        [global::Bond.Id(10), global::Bond.Type(typeof(global::Bond.Tag.wstring)), global::Bond.Required]
        public string entityType { get; set; }

        [global::Bond.Id(20), global::Bond.Type(typeof(global::Bond.Tag.wstring)), global::Bond.Required]
        public string entityId { get; set; }

        [global::Bond.Id(30), global::Bond.Required]
        public int pageNumber { get; set; }

        [global::Bond.Id(40), global::Bond.Type(typeof(global::Bond.Tag.wstring)), global::Bond.Required]
        public string filter { get; set; }

        [global::Bond.Id(50), global::Bond.Required]
        public AccessType? accessType { get; set; }

        [global::Bond.Id(60), global::Bond.Required]
        public ushort response { get; set; }

        public EntityAccess()
            : this("ContosoAdsSupport.EntityAccess", "EntityAccess")
        {}

        protected EntityAccess(string fullName, string name)
        {
            entityType = "";
            entityId = "";
            filter = "";
        }
    }
} // ContosoAdsSupport
